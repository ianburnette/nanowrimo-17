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

public class BlockCell
{
    public Vector2 cellPosition;
    public GridCoordinates myCoordinates;
    public BlockIndividual blockInCell;
    public bool currentlyPartOfAMatch;
}

public class GridManagement : MonoBehaviour {
    #region Private Variables
    [Header("Game Board variables")]
    public static GridManagement publicGrid;                        
    [SerializeField] int columnCount, rowCount;
    [SerializeField] BlockCell[,] blockGrid;
    [SerializeField] float rowHeight, columnWidth;

    [Header ("References")]
    [SerializeField] BlockDragging blockMovement;

    [Header("Dynamic variables")]
    [SerializeField] int rowsFromTopOfGrid;
    [SerializeField] int currentBottonRow, currentTopRow, currentRowCount;
    [SerializeField] bool gridInitialized;

    [Header("Gizmo Variables")]
    [SerializeField] bool showGridCenterLines;
    [SerializeField] bool showGridOutlines;
    [SerializeField] bool showGridOrigin;
    [SerializeField] bool showCells;

    [Header("Debug Variables")]
    [SerializeField] bool debugQueryNow;
    [SerializeField] GridCoordinates debugGridQuery;
    [SerializeField] BlockType queryBlockType;
    [SerializeField] GameObject queryGameObject;
    [SerializeField] GridCoordinates queryLocalGridPosition;
    [SerializeField] GridCoordinates returnedCellGridPosition;
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

    public int RowCount
    {
        get
        {
            return rowCount;
        }

        set
        {
            rowCount = value;
        }
    }

    public int CurrentBottonRow
    {
        get
        {
            return currentBottonRow;
        }

        set
        {
            currentBottonRow = value;
        }
    }

    public int RowsFromTopOfGrid
    {
        get
        {
            return rowsFromTopOfGrid;
        }

        set
        {
            rowsFromTopOfGrid = value;
        }
    }

    public int CurrentTopRow
    {
        get
        {
            return currentTopRow;
        }

        set
        {
            currentTopRow = value;
        }
    }

    public int CurrentRowCount
    {
        get
        {
            return currentRowCount;
        }

        set
        {
            currentRowCount = value;
        }
    }
    #endregion

    #region Unity Functions
    void OnEnable () {
        publicGrid = this;
       
    }
    private void Start()
    {
        if (!gridInitialized)
            InitializeGrid();
    }
    private void Update()
    {
        if (debugQueryNow)
            DebugCell();
        currentTopRow = currentBottonRow - currentRowCount;
    }
    private void OnDrawGizmos()
    {
        if (showCells && gridInitialized)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < columnCount; i++) //for every column
                for (int j = 0; j < rowsFromTopOfGrid+1; j++) //and every inaccessible row at the top
                    Gizmos.DrawCube(blockGrid[i, j].cellPosition, new Vector3(.1f, .1f, .1f));
            Gizmos.color = Color.green;
            for (int i = 0; i < columnCount; i++) //for every column
                for (int j = rowsFromTopOfGrid+1; j < rowCount; j++) //and every accessible row within every column
                    Gizmos.DrawCube(blockGrid[i, j].cellPosition, new Vector3(.1f, .1f, .1f));
        }
    }
    #endregion

    #region Custom Functions
    #region Grid Setup
    void InitializeGrid()
    {
        blockGrid = new BlockCell[columnCount, rowCount];
        for (int i = 0; i<columnCount; i++) //for every column
        {
            for (int j = 0; j < rowCount; j++) //and every row within every column
            {
                blockGrid[i, j] = new BlockCell();
                blockGrid[i, j].cellPosition = (Vector2)transform.position +
                                                new Vector2(columnWidth * i, rowHeight * j);
                blockGrid[i, j].myCoordinates.column = i;
                blockGrid[i, j].myCoordinates.row = j;
            }
        }
        gridInitialized = true;
    }
    #endregion
    #region Block Placement
    public void SpawnBlock(BlockIndividual thisBlock, GridCoordinates gridCoords)
    {
        thisBlock.transform.position = blockGrid[gridCoords.column, gridCoords.row].cellPosition;
        blockGrid[gridCoords.column, gridCoords.row].blockInCell = thisBlock;
        blockGrid[gridCoords.column, gridCoords.row].myCoordinates = thisBlock.MyGridCoords = gridCoords;
    }
    public void SwapBlock(BlockIndividual thisBlock, BlockMovementDirection directionToMove)
    {
        BlockCell targetCell = GridMovementQuery(thisBlock.MyGridCoords, directionToMove);
        GridCoordinates previousCoords = thisBlock.MyGridCoords;
        if (targetCell.blockInCell != null)
        {
            //store the target cell's values in temporary variables
            Vector2 previousPos = thisBlock.transform.position;
            BlockIndividual previousBlock = targetCell.blockInCell;
            //physically move the blocks
            thisBlock.transform.position = targetCell.cellPosition;
            targetCell.blockInCell.transform.position = previousPos;
            //update the moving cell's values
            blockGrid[targetCell.myCoordinates.column, targetCell.myCoordinates.row].blockInCell = thisBlock;
            blockGrid[targetCell.myCoordinates.column, targetCell.myCoordinates.row].myCoordinates = thisBlock.MyGridCoords = targetCell.myCoordinates; ;
            //update the displaced cell's values
            blockGrid[previousCoords.column, previousCoords.row].blockInCell = previousBlock;
            blockGrid[previousCoords.column, previousCoords.row].myCoordinates = previousBlock.MyGridCoords = previousCoords;
        }
        else
        { 
            //physically move the moving block
            thisBlock.transform.position = targetCell.cellPosition;
            //update the new cell's values
            blockGrid[targetCell.myCoordinates.column, targetCell.myCoordinates.row].blockInCell = thisBlock;
            blockGrid[targetCell.myCoordinates.column, targetCell.myCoordinates.row].myCoordinates = thisBlock.MyGridCoords = targetCell.myCoordinates; ;
            //update the old, empty cell's values
            blockGrid[previousCoords.column, previousCoords.row].blockInCell = null;
            blockGrid[previousCoords.column, previousCoords.row].myCoordinates = previousCoords;
        }
    }

    #endregion
    #region Utility Functions
    BlockCell GridMovementQuery(GridCoordinates gridCoords, BlockMovementDirection movementDirection)
    {
        switch (movementDirection)
        {
            case BlockMovementDirection.down:
                return blockGrid[gridCoords.column, gridCoords.row + 1];
            case BlockMovementDirection.left:
                return blockGrid[gridCoords.column - 1, gridCoords.row];
            case BlockMovementDirection.right:
                return blockGrid[gridCoords.column + 1, gridCoords.row];
            case BlockMovementDirection.up:
                return blockGrid[gridCoords.column, gridCoords.row - 1];
        }
        Debug.LogError("didn't find a direction to query so returning this block");
        return blockGrid[gridCoords.column, gridCoords.row];
    }
    public BlockCell GridCellQuery(GridCoordinates coords)
    {
        return blockGrid[coords.column, coords.row];
    }
    public Vector2 GetPositionAtCoordinates(GridCoordinates coords)
    {
        return blockGrid[coords.column, coords.row].cellPosition;
    }
    void DebugCell()
    {
        if (blockGrid[debugGridQuery.column, debugGridQuery.row].blockInCell != null)
        {
            queryBlockType = blockGrid[debugGridQuery.column, debugGridQuery.row].blockInCell.MyType;
            queryGameObject = blockGrid[debugGridQuery.column, debugGridQuery.row].blockInCell.gameObject;
            queryLocalGridPosition = blockGrid[debugGridQuery.column, debugGridQuery.row].blockInCell.MyGridCoords;
        }
        returnedCellGridPosition = blockGrid[debugGridQuery.column, debugGridQuery.row].myCoordinates;
        debugQueryNow = false;
    }

    #endregion
    /*
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
    /*}
void InitializeGrid()                                              
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
    /*
     public void SwapBlocks(BlockIndividual incomingBlock, BlockMovementDirection incomingDirection)
     {
        //the primary location is the place where we're placing the block. the secondary location is that block's old position and the displaced blocks new position  
        
        GridCoordinates primaryLocation = GetRelativeCoordinates(incomingBlock.MyGridCoords, incomingDirection);
        GridCoordinates secondaryLocation = incomingBlock.MyGridCoords;
        print(secondaryLocation.column + " " + secondaryLocation.row);

        if (CellEmpty(primaryLocation)) //move the block to this empty cell
        {
            MoveBlock(incomingBlock.transform, GetRelativePosition(secondaryLocation, incomingDirection));
        }
        else
        {
            Vector2 tempPos = incomingBlock.transform.position;
            MoveBlock(incomingBlock.transform, GetRelativePosition(secondaryLocation, incomingDirection));
            MoveBlock(blockGrid[primaryLocation.column, primaryLocation.row].transform, tempPos);
        }

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
        //update the grid to match the new blocks' positions and 



        //then send the call to physically move them
        
        //blockGrid[outgoingBlock.MyGridCoords.column]
     }
    void MoveBlock(Transform blockToMove, Vector2 destinationToMoveTo)
    {
        blockToMove.position = destinationToMoveTo;
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
    Vector2 GetRelativePosition(GridCoordinates coords, BlockMovementDirection movementDirection)
    {
        Vector2 relativePos = new Vector2(
                gridLayout.GridTopLeft.x + (coords.column   * gridLayout.ColumnWidth) + (gridLayout.ColumnWidth / 2),
                gridLayout.GridTopLeft.y - ((coords.row +1) * gridLayout.RowHeight)   - (gridLayout.RowHeight   / 2)
            );
        return relativePos;
    }
    public Vector2 GetPositionAtCoordinates(GridCoordinates coords)
    {
        Vector2 pos = new Vector2(gridLayout.ColumnPositions[coords.column], gridLayout.RowPositions[coords.row]);
        return pos;
    }
  
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
