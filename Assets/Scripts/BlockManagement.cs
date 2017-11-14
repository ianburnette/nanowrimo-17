using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManagement))]
public class BlockManagement : MonoBehaviour {

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
                    GameObject thisBlock = CreateBlock(currentCoords);
                    blocksCreated++;
                    if (blocksCreated >= blocksToCreate)
                        break;
                    yield return new WaitForEndOfFrame();
                }
                if (blocksCreated >= blocksToCreate)
                    break;
            }
            yield return new WaitForEndOfFrame();
            timesThroughLoop++;
        }
        yield return null;
    }

    GameObject CreateBlock(GridCoordinates currentCoords)
    {
        GameObject createdBlock;
        //Vector2 position = gridManagement.GetPositionAtCoordinates(currentCoords);
        if (blockPool.TryGetNextObject(transform.position, Quaternion.identity, out createdBlock))
        {
            //createdBlock.transform.position = position;
            BlockIndividual currentIndividual = createdBlock.GetComponent<BlockIndividual>();
            currentIndividual.MyGridCoords = currentCoords;
            gridManagement.PutBlockIntoGrid(currentIndividual, currentCoords);
            return createdBlock;
        }
        else
        {
            Debug.LogError("no block found to create");
            return createdBlock;
        }
    }

    public void DestroyBlock(BlockIndividual blockToDestroy, GridCoordinates blockCoordinates)
    {
        blockToDestroy.gameObject.SetActive(false);
        gridManagement.RemoveBlockFromGrid(blockToDestroy, blockCoordinates);
    }
    
#endregion
}
