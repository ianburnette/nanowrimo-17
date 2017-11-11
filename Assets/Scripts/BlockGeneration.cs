﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManagement))]
public class BlockGeneration : MonoBehaviour {

    #region Private Variables
    [SerializeField] GridManagement gridManagement;
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
        yield return new WaitForSeconds(1f);
        int blocksCreated = 0;
        int timesThroughLoop = 0;
        GridCoordinates currentCoords;
        while (blocksCreated < blocksToCreate)
        {
            if (timesThroughLoop > 100)
                blocksCreated = blocksToCreate;
        
            for (int j = gridManagement.CurrentBottonRow; j > gridManagement.RowsFromTopOfGrid; j--) //for every row, starting at the current bottom of the screen and moving up until we reach the inactive rows for this round
            {
                for (int i = 0; i < gridManagement.ColumnCount; i++) //for every column
                {
                    currentCoords.column = i;
                    currentCoords.row = j;
                    GameObject thisBlock = CreateBlock(gridManagement.GetPositionAtCoordinates(currentCoords));
                    blocksCreated++;
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
            timesThroughLoop++;
        }
        yield return null;
    }


    //loop through available cells and create blocks in them as long as they're not empty
    /* for (int i = gridManagement.FirstActiveRow; i > minRowToGenerateIn; i--)          //FOR EVERY ROW, STARTING AT THE BOTTOM AND ENDING AT THE STATED MINIMUM
     {
         currentCoords.row = i;
         for (int j = 0; j < gridManagement.ColumnCount; j++)
         {
             currentCoords.column = j;
             if (Random.value < likelihoodOfGeneratingBlock && gridManagement.CellEmpty(currentCoords))
             {
               //  print("placing block at " + GridManagement.publicGrid.GridOrigin);
                 GameObject thisBlock = CreateBlock(gridManagement.GetPositionAtCoordinates(currentCoords));
                 gridManagement.PlaceNewBlock(currentCoords, thisBlock.GetComponent<BlockIndividual>());
                 blocksCreated++;
                 if (blocksCreated >= blocksToCreate)
                     break;
                 yield return new WaitForEndOfFrame();
             }
             else
                 yield return new WaitForEndOfFrame();
             yield return new WaitForEndOfFrame();
         }
         yield return new WaitForEndOfFrame();
     }*/


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
