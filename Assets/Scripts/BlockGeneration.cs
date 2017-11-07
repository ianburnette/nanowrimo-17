﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManagement))]
public class BlockGeneration : MonoBehaviour {

    #region Private Variables
    [SerializeField] GridManagement columnManagement;
    [SerializeField] EZObjectPools.EZObjectPool blockPool;

    [Header("Beginning of Run")]
    [SerializeField] int blocksToCreate;
    [SerializeField] int minRowToGenerateIn;
    [SerializeField] float likelihoodOfGeneratingBlock;
#endregion

#region Public Properties

#endregion

#region Unity Functions
	void Start () {
        StartCoroutine(GenerateBeginningOfRunBlocks());
	}
	
	void Update () {
		
	}
#endregion

#region Custom Functions
    IEnumerator GenerateBeginningOfRunBlocks()
    {
        yield return new WaitForEndOfFrame();
        int blocksCreated = 0;
        int timesThroughLoop = 0;
        while (blocksCreated < blocksToCreate)
        {
            if (timesThroughLoop > 100)
            {
                blocksCreated = blocksToCreate;
            //    print("timed out of loop");
            }
            //loop through available cells and create blocks in them as long as they're not empty
            for (int i = columnManagement.FirstActiveRow; i > minRowToGenerateIn; i--)          //FOR EVERY ROW, STARTING AT THE BOTTOM AND ENDING AT THE STATED MINIMUM
            {
                for (int j = 0; j < columnManagement.ColumnCount; j++)
                {
                //    print("placing in column "+ j);
                    if (Random.value < likelihoodOfGeneratingBlock && columnManagement.CellEmpty(i, j))
                    {
                      //  print("placing block at " + GridManagement.publicGrid.GridOrigin);
                        GameObject thisBlock = CreateBlock(
                            GridDebug.publicGrid.GridOrigin +
                                new Vector2(
                                    (GridDebug.publicGrid.ColumnWidth * j) + GridDebug.publicGrid.ColumnWidth / 2,
                                    (GridDebug.publicGrid.RowHeight * (columnManagement.FirstActiveRow - i) + GridDebug.publicGrid.RowHeight / 2
                                    )));
                        columnManagement.PlaceNewBlock(i, j, thisBlock.GetComponent<BlockIndividual>());
                        blocksCreated++;
                        if (blocksCreated >= blocksToCreate)
                            break;
                     //   print("blocks created is now " + blocksCreated);
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                //        print("but it's not empty");
                        yield return new WaitForEndOfFrame();
                    }
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            timesThroughLoop++;
        }
        yield return null;
    }
   
    GameObject CreateBlock(Vector2 position)
    {
        GameObject createdBlock;
        if (blockPool.TryGetNextObject(position, Quaternion.identity, out createdBlock))
        {
            createdBlock.transform.position = position;
            return createdBlock;
        }
        else
        {
            Debug.LogError("no block found to create");
            return createdBlock;
        }
    }
    
#endregion
}
