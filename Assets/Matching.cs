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

struct BlockAction
{
    public BlockIndividual block;
    public GridCoordinates destination;
}

public class Matching : MonoBehaviour
{

    #region Private Variables
    [Header("References")]
    [SerializeField]
    GridManagement grid;

    [Header("Iterator variables")]
    [SerializeField] BlockType currentBlockType;
    [SerializeField] BlockCell currentCell;
    [SerializeField] BlockIndividual currentBlock;
    [SerializeField] GridCoordinates currentCoords;

    [Header("Matches")]
    [SerializeField] int maxPossibleMatches;
    [SerializeField] int maxBlocksPerMatch;
    BlockAction[,] matches;


    [Header("Gravity")]
    [SerializeField] int gravityActionsToTrack;
    [SerializeField] int currentlyTrackedGravityActions;
    BlockAction[] gravActions;

    #endregion

    #region Public Properties

    #endregion

    #region Unity Functions
    void Start()
    {
        maxBlocksPerMatch = gravityActionsToTrack = grid.ColumnCount * grid.CurrentBottonRow;           //calculate max gravity actions or blocks per match that can exist at once
        maxPossibleMatches = Mathf.RoundToInt(maxBlocksPerMatch / 3);                                   //the maximum number of matches is the total number of blocks on the board divided by three

        gravActions = new BlockAction[gravityActionsToTrack];                                           //initialize the gravity array to that size
        for (int i = 0; i < gravityActionsToTrack; i++)                                                 
            gravActions[i] = new BlockAction();                                                         //initialize each array element because we're using a custom struct

        matches = new BlockAction[maxPossibleMatches, maxBlocksPerMatch];
        for (int i = 0; i < maxPossibleMatches; i++)
            for (int j = 0; j < maxBlocksPerMatch; j++)
                matches[i,j] = new BlockAction();
    }

    void Update()
    {

    }
    #endregion

    #region Custom Functions
    #region Board Analysis
    void ScanBoard()
    {
        //run a gravity sweep on every column
        for (currentCoords.column = 0; currentCoords.column < grid.ColumnCount; currentCoords.column++)
        {
            CheckForUnsupportedBlocks(currentCoords);
            if (currentlyTrackedGravityActions > 0)
                ImplementGravity();
        }
        //iterate through the grid, working across each row from left to right and then up to the next row
        for (currentCoords.row = grid.CurrentBottonRow; currentCoords.row > grid.CurrentTopRow; currentCoords.row--)
        {
            for (currentCoords.column = 0; currentCoords.column < grid.ColumnCount; currentCoords.column++)
            {
                currentCell = grid.GridCellQuery(currentCoords);
                if (currentCell.blockInCell != null && !currentCell.currentlyPartOfAMatch) //there's a block here and it's not currently part of any other matches, so check for matches
                {
                    BlockType typeToMatch = currentCell.blockInCell.MyType;
                    BlockType currentType = typeToMatch;
                    //check above this cell for matches
                    int currentRow = currentCoords.row;
                    while(currentType == typeToMatch && currentRow > 0)
                    {

                    }
                    //check to the right of this cell for matches

                }
            }
        }
    }
    #endregion
    #region Matches
    
    #endregion
    #region Gravity
    void CheckForUnsupportedBlocks(GridCoordinates startingCoords)
    {
        GridCoordinates currentCoords = startingCoords;
        GridCoordinates destinationCoords = startingCoords;
        int blocksFoundInThisColumn = 0;
        for (currentCoords.row = startingCoords.row + 1; currentCoords.row < grid.CurrentTopRow; currentCoords.row++) //starting at the row above which we found an empty space and working up to the top of the board
        {
            if (grid.GridCellQuery(currentCoords).blockInCell != null) //we found an unsupported cell above an empty space
            {
                gravActions[currentlyTrackedGravityActions].block = grid.GridCellQuery(currentCoords).blockInCell;
                destinationCoords.row = startingCoords.row + blocksFoundInThisColumn;
                gravActions[currentlyTrackedGravityActions].destination = destinationCoords;
                currentlyTrackedGravityActions++;
                blocksFoundInThisColumn++;
            }
        }
    }
    void ImplementGravity()
    {
        //gravity implemention will go here
        currentlyTrackedGravityActions = 0;
    }
    #endregion
    #region Matches

    #endregion
    #region Public Functions
    public void BlockPlaced(BlockIndividual blockThatHasBeenPlaced)
    {
        ScanBoard();
    }
    #endregion
    #endregion
}
