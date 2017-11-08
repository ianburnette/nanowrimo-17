using System;
using UnityEngine;

[Serializable]
public struct GridCoordinates
{
    public int column;
    public int row;
}

public enum BlockMovementDirection
{
    up, 
    down, 
    left, 
    right,
    none
}

public class GridManagement : MonoBehaviour {
    #region Private Variables
    [SerializeField] int columnCount, rowCount, firstActiveRow;
    [SerializeField] BlockDragging blockMovement;

    [SerializeField] BlockIndividual[,] blockGrid;

    [SerializeField] GridCoordinates gridQuery;
    [SerializeField] BlockType queryBlockType;
    [SerializeField] GameObject queryGameObject;
    [SerializeField] GridCoordinates queryLocalGridPosition;
    #endregion

    #region Public Properties
    public int ColumnCount
    {
        get
        {
            return columnCount;
        }

        set
        {
            columnCount = value;
        }
    }
    public int FirstActiveRow
    {
        get
        {
            return firstActiveRow;
        }

        set
        {
            firstActiveRow = value;
        }
    }
    #endregion

    #region Unity Functions
    void Awake () {
        InitializeBlockArray();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            QueryCell();
    }
    #endregion

    #region Custom Functions
    void QueryCell()                            //a debug function to see what is in a particular cell
    {
        BlockIndividual query = blockGrid[gridQuery.column, gridQuery.row];
        if (query != null)
        {
            queryBlockType = query.MyType;
            queryGameObject = query.gameObject;
            queryLocalGridPosition = query.MyGridCoords;
        }
        
      /*  int x = Mathf.RoundToInt(gridQuery.x);
        int y = Mathf.RoundToInt(gridQuery.y);
        queryBlockType = blockGrid[x, y].type;
        queryGameObject = blockGrid[x, y].myGameObject;
        if (queryGameObject != null)
            queryLocalGridPosition = blockGrid[x, y].myGameObject.GetComponent<BlockIndividual>().MyGridIndex;
        else
            queryLocalGridPosition = Vector2.zero;*/
    }
    void InitializeBlockArray()                                                     //SETS UP THE ARRAY TO BE THE CORRECT SIZE
    {
        blockGrid = new BlockIndividual[columnCount, rowCount];
    }
    public bool CellEmpty(GridCoordinates gridCoords)                               //RETURNS TRUE IF CELL AT THESE COORDINATES IS EMPTY
    {
        if (blockGrid[gridCoords.column, gridCoords.row] == null)
            return true;
        else
            return false;
    }                            
    public void PlaceNewBlock(GridCoordinates gridCoords, BlockIndividual blockScript)
    { 
        blockScript.MyGridCoords = gridCoords;
        blockGrid[gridCoords.column, gridCoords.row] = blockScript;
        //.myGameObject = blockScript.gameObject;
        //blockGrid[gridCoords.column, gridCoords.row].type = blockScript.MyType;
    }
    public void PlaceEmptySpace(GridCoordinates gridCoords)
    {
        blockGrid[gridCoords.column, gridCoords.row] = null;
        //blockGrid[column, row].myGameObject = null;
        //blockGrid[column, row].type = BlockType.none;
    }
    //
    /*
     * The purpose of this class is to keep track of what cells are where on the grid.
     * I need the ability to assign data to any grid cell
     * I need the ability to swap two blocks
     * I need the ability to tell if any given grid cell is empty
     * 
     *  
     * *.
    /*
     */
     public void SwapBlocks(BlockIndividual incomingBlock, BlockMovementDirection incomingDirection)
     {
        //the primary location is the place where we're placing the block. the secondary location is that block's old position and the displaced blocks new position  
        
        GridCoordinates primaryLocation = GetRelativeCoordinates(incomingBlock.MyGridCoords, incomingDirection);
        GridCoordinates secondaryLocation = incomingBlock.MyGridCoords;
        print(secondaryLocation.column + " " + secondaryLocation.row);
        if (CellEmpty(primaryLocation))
        {
            incomingBlock.MyGridCoords = primaryLocation;
            blockGrid[primaryLocation.column, primaryLocation.row] = incomingBlock;
            blockGrid[secondaryLocation.column, secondaryLocation.row] = null;
        }
            
        else
        {
            BlockIndividual outgoingBlock = blockGrid[primaryLocation.column, primaryLocation.row];
            BlockIndividual outgoing = blockGrid[outgoingBlock.MyGridCoords.column, outgoingBlock.MyGridCoords.row];
            BlockIndividual incoming = blockGrid[incomingBlock.MyGridCoords.column, incomingBlock.MyGridCoords.row];
            blockGrid[incomingBlock.MyGridCoords.column, incomingBlock.MyGridCoords.row] = blockGrid[outgoingBlock.MyGridCoords.column, outgoingBlock.MyGridCoords.row];
        }
        //update the grid to match the new blocks' positions and then send the call to physically move them
        
        //blockGrid[outgoingBlock.MyGridCoords.column]
     }
    GridCoordinates GetRelativeCoordinates(GridCoordinates coords, BlockMovementDirection directionToQuery)
    {
        GridCoordinates coordsToReture = coords;
        print(coords.column + " " + coords.row);
        switch (directionToQuery)
        {
            case BlockMovementDirection.down:
                //this should only ever be empty, right? 
                break;
            case BlockMovementDirection.left:
                coords.column = coords.column - 1;
                return coords;
            case BlockMovementDirection.right:
                coords.column = coords.column + 1;
                return coords;
        }
        return coords;
    }
        /*
    public void CategorizeBlock(int row, int column, BlockIndividual blockScript)                                 //UPDATE THE GRID VARIABLES AT THIS LOCATION WITH THE VARIABLES OF TH ENEW BLOCK
    {
        print("attempting to categorize block in column " + column + " and row " + row);

            print("displacing block of type " + blockGrid[column, row].type);                                     
            int newCol = blockScript.MyGridCoords.column > column ? column + 1 : column - 1;                            //THERE'S SOMETHING HERE ALREADY - WE HAVE TO MOVE IT LEFT OR RIGHT
            blockMovement.DisplaceBlock(blockGrid[newCol, row], blockScript.MyGridCoords);                         //WE DISPLACE THE BLOCK THAT IS ALREADY THERE TO WHERE THE NEW BLOCK WAS  
            if (blockGrid[column, row].myGameObject == null)                                                      //WE CATEGORIZE WHERE THE OLD BLOCK HAS GONE
                PlaceEmptySpace(row, newCol);                                                                       //even if it's empty
            else
                PlaceNewBlock(row, newCol, blockGrid[column, row].myGameObject.GetComponent<BlockIndividual>());  //and a different function if it's not
            PlaceNewBlock(row, column, blockScript);                                                              //THEN WE OFFICIALLY CATEGORIZE THE NEW BLOCK
    }
    */
#endregion
}
