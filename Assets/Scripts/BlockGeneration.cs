using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColumnManagement))]
public class BlockGeneration : MonoBehaviour {

    #region Private Variables
    [SerializeField] ColumnManagement columnManagement;
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
        int blocksCreated = 0;
        int timesThroughLoop = 0;
        while (blocksCreated < blocksToCreate)
        {
            if (timesThroughLoop > 100)
            {
                blocksCreated = blocksToCreate;
                print("timed out of loop");
            }
            //loop through available cells and create blocks in them as long as they're not empty
            for (int i = columnManagement.FirstActiveRow; i > minRowToGenerateIn; i--)          //FOR EVERY ROW, STARTING AT THE BOTTOM AND ENDING AT THE STATED MINIMUM
            {
                print("in a row");
                for (int j = 0; j < columnManagement.ColumnCount; j++)
                {
                    print("in a column");
                    bool empty = columnManagement.CellEmpty(i, j);
                    if (Random.value < likelihoodOfGeneratingBlock && empty)
                    {
                        //print("in if");
                        Block thisBlock = CreateBlock(
                            GridManagement.publicGrid.GridOrigin + 
                                new Vector2(
                                    (GridManagement.publicGrid.ColumnWidth * j) + GridManagement.publicGrid.ColumnWidth / 2,
                                    (GridManagement.publicGrid.RowHeight * i) + GridManagement.publicGrid.RowHeight / 2
                                    ));
                        columnManagement.CategorizeBlock(j,i, thisBlock);
                        blocksCreated++;
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            timesThroughLoop++;
        }
        yield return null;
    }
    Block CreateBlock(Vector2 position)
    {
        GameObject createdBlock;
        Block thisBlock;
        if (blockPool.TryGetNextObject(position, Quaternion.identity, out createdBlock))
        {
            createdBlock.transform.position = position;
            thisBlock.myGO = createdBlock;
            thisBlock.myType = BlockType.fire;
            return thisBlock;
    }
        else
        {
            Debug.LogError("no block found to create");
            thisBlock.myGO = gameObject;
            thisBlock.myType = BlockType.none;
            return thisBlock;
        }
    }
    
#endregion
}
