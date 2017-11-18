using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The goals of this class:
 * 1- every time a block is moved, cycle through the active game board and check to see if there are any matches
 * 2- if there are any matches, activate the matches and disable those blocks
 * 3- while checking for matches, also check for gravity to implement (this will work like a match but will simply be if an empty cell has any blocks above it
 * 4- it should also be used by BlockGeneration.cs to prevent matches from being present on the board when blocks are first generated
 * */

struct GravityAction
{
    public BlockIndividual block;
    public GridCoordinates destination;
}

public class Matching : MonoBehaviour
{

    #region Private Variables
    [Header("References")]
    [SerializeField] GridManagement gridManagement;
    [SerializeField] BlockManagement blockManagement;

    [Header("Iterator variables")]
    [SerializeField] BlockType currentBlockType;
    [SerializeField] BlockCell currentCell;
    [SerializeField] BlockIndividual currentBlock;
    [SerializeField] GridCoordinates currentCoords;

    [Header("Matches")]
    [SerializeField] int maxPossibleMatches;
    [SerializeField] int maxBlocksPerMatch;
    BlockCell[,] matches;
    [SerializeField] int currentNumberOfMatches;
    [SerializeField] int numberOfBlocksInCurrentMatch;


    [Header("Gravity")]
    [SerializeField] int gravityActionsToTrack;
    [SerializeField] int currentlyTrackedGravityActions;
    GravityAction[] gravActions;

    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start()
    {
        maxBlocksPerMatch = gravityActionsToTrack = gridManagement.ColumnCount * gridManagement.CurrentBottonRow;           //calculate max gravity actions or blocks per match that can exist at once
        maxPossibleMatches = Mathf.RoundToInt(maxBlocksPerMatch / 3);                                   //the maximum number of matches is the total number of blocks on the board divided by three

        gravActions = new GravityAction[gravityActionsToTrack];                                           //initialize the gravity array to that size
        for (int i = 0; i < gravityActionsToTrack; i++)                                                 
            gravActions[i] = new GravityAction();                                                         //initialize each array element because we're using a custom struct

        matches = new BlockCell[maxPossibleMatches, maxBlocksPerMatch];
        for (int i = 0; i < maxPossibleMatches; i++)
            for (int j = 0; j < maxBlocksPerMatch; j++)
                matches[i,j] = new BlockCell();
    }

    void Update()
    {

    }
    #endregion

    #region Custom Functions
    #region Board Analysis
    public void ScanBoard()
    {
        //run a gravity sweep on every column
        currentCoords.row = gridManagement.CurrentBottonRow + 1;
        for (currentCoords.column = 0; currentCoords.column < gridManagement.ColumnCount; currentCoords.column++)
        {
            print("in loop");
            CheckForUnsupportedBlocks(currentCoords);
            
        }
        //iterate through the grid, working across each row from left to right and then up to the next row
        for (currentCoords.row = gridManagement.CurrentBottonRow; currentCoords.row >= gridManagement.CurrentTopRow; currentCoords.row--)
        {
            for (currentCoords.column = 0; currentCoords.column < gridManagement.ColumnCount ; currentCoords.column++)
            {
                currentCell = gridManagement.GridCellQuery(currentCoords);
               if (currentCell.blockInCell != null /*&& !currentCell.currentlyPartOfAMatch*/) //there's a block here and it's not currently part of any other matches, so check for matches
                {
                    BlockType typeToMatch = currentCell.blockInCell.MyType;
                //    print("checking " + currentCoords.column + ", " + currentCoords.row);
                    CheckCell(currentCell, currentCoords, typeToMatch);
                }
            }
        }
        //clear any matches
        for (int i = 0; i<currentNumberOfMatches; i++)
        {
            for (int j = 0; j<maxBlocksPerMatch; j++)
            {
                if (matches[i, j].currentlyPartOfAMatch)
                {
                    blockManagement.DestroyBlock(matches[i, j].blockInCell, matches[i, j].myCoordinates);
                }
                else
                    j = maxBlocksPerMatch;
            }
        }
        //clear 
        currentNumberOfMatches = 0;
    }
    #endregion
    #region Matches
    void CheckCell(BlockCell currentCell, GridCoordinates currentCoords, BlockType typeToMatch)
    {
        //check above this cell for matches
      
        int currentRow = currentCoords.row;
        CheckInDirection(currentCoords, typeToMatch, GridDirection.up);
        if (numberOfBlocksInCurrentMatch >= 2)
        {
            matches[currentNumberOfMatches, numberOfBlocksInCurrentMatch] = currentCell;
            currentCell.currentlyPartOfAMatch = true;
            currentNumberOfMatches++;
        }
        numberOfBlocksInCurrentMatch = 0;
     
        //check to the right of this cell for matches
        int currentColumn = currentCoords.column;
        CheckInDirection(currentCoords, typeToMatch, GridDirection.right);
        if (numberOfBlocksInCurrentMatch >= 2)
        {
            matches[currentNumberOfMatches, numberOfBlocksInCurrentMatch] = currentCell;
            currentCell.currentlyPartOfAMatch = true;
            currentNumberOfMatches++;
        }
        numberOfBlocksInCurrentMatch = 0;
    }
    void CheckInDirection(GridCoordinates coords, BlockType typeToMatch, GridDirection dir)
    {

        if (dir == GridDirection.up) {
            if (coords.row > 0)
                coords.row -= 1;
            else
                return;
        }
        else if (dir == GridDirection.right) {
            if (coords.column < gridManagement.ColumnCount - 1)
                coords.column += 1;
            else
                return;
        }
        else
        {
         //   print("checking for matches at " + coords.column + ", " + coords.row + ", which is impossible");
            Debug.LogError("we're not supposed to be searching for matches in that direction, are we? dir = " + dir);
            return;
        }
            

        //if (coords.column > gridManagement.ColumnCount - 1 || coords.row < 0)
        
        BlockCell cellToCheck = gridManagement.GridCellQuery(coords);
        if (cellToCheck.blockInCell != null)
        {
            if (cellToCheck.blockInCell.MyType == typeToMatch)
            {
                cellToCheck.currentlyPartOfAMatch = true;
                matches[currentNumberOfMatches, numberOfBlocksInCurrentMatch] = cellToCheck;//gridManagement.GridCellQuery(coords);
                numberOfBlocksInCurrentMatch++;
                CheckInDirection(coords, typeToMatch, dir);
            }
        }
    }
    #endregion
    #region Gravity
    void CheckForUnsupportedBlocks(GridCoordinates startingCoords)
    {
        GridCoordinates destinationCoords = startingCoords;
        int blocksFoundInThisColumn = 0;
        GridCoordinates currentCoords = startingCoords;
        for (int i = startingCoords.row; i > gridManagement.CurrentTopRow; i--) //starting at the row above which we found an empty space and working up to the top of the board
        {
            currentCoords.column = currentCoords.column;
            currentCoords.row = i;
            if (gridManagement.GridCellQuery(currentCoords).blockInCell != null) //we found an unsupported cell above an empty space
            {
                gravActions[currentlyTrackedGravityActions].block = gridManagement.GridCellQuery(currentCoords).blockInCell;
                destinationCoords.row = startingCoords.row + blocksFoundInThisColumn;
                gravActions[currentlyTrackedGravityActions].destination = destinationCoords;
                currentlyTrackedGravityActions++;
                blocksFoundInThisColumn++;
            }
        }
        if (currentlyTrackedGravityActions > 0)
            ImplementGravity();
    }
    void ImplementGravity()
    {
        //gravity implemention will go here
        //currentlyTrackedGravityActions = 0;
    }
    #endregion
    #region Public Functions
    public void BlockPlaced(BlockIndividual blockThatHasBeenPlaced)
    {
        ScanBoard();
    }
    #endregion
    #endregion
}
