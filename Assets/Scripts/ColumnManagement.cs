using System;
using UnityEngine;



public class ColumnManagement : MonoBehaviour {
    #region Private Variables
    [SerializeField] int columnCount, rowCount, firstActiveRow;
    [SerializeField] BlockMovement blockMovement;

    [SerializeField] Block[,] blockGrid;

    [SerializeField] Vector2 gridQuery;
    [SerializeField] BlockType queryBlockType;
    [SerializeField] GameObject queryGameObject;
    [SerializeField] Vector2 queryLocalGridPosition;
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
    void Start () {
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
        int x = Mathf.RoundToInt(gridQuery.x);
        int y = Mathf.RoundToInt(gridQuery.y);
        queryBlockType = blockGrid[x, y].type;
        queryGameObject = blockGrid[x, y].myGameObject;
        queryLocalGridPosition = blockGrid[x, y].myGameObject.GetComponent<BlockIndividual>().MyGridIndex;
    }
    void InitializeBlockArray()
    {
        blockGrid = new Block[columnCount, rowCount];
    }

    public bool CellEmpty(int testRow, int testColumn)
    {
        if (blockGrid[testColumn, testRow].type == BlockType.none)
            return true;
        else
            return false;
    }
    public void PlaceNewBlock(int row, int column, BlockIndividual blockScript)
    { 
        blockScript.MyGridIndex = new Vector2(column, row);
        blockGrid[column, row].myGameObject = blockScript.gameObject;
        blockGrid[column, row].type = blockScript.MyType;
    }
    public void PlaceNewBlock(int row, int column)
    {
        blockGrid[column, row].myGameObject = null;
        blockGrid[column, row].type = BlockType.none;
    }
    public void CategorizeBlock(int row, int column, BlockIndividual blockScript)                                 //UPDATE THE GRID VARIABLES AT THIS LOCATION WITH THE VARIABLES OF TH ENEW BLOCK
    {
        print("attempting to categorize block in column " + column + " and row " + row);

            print("displacing block of type " + blockGrid[column, row].type);                                     
            int newCol = blockScript.MyGridIndex.x > column ? column + 1 : column - 1;                            //THERE'S SOMETHING HERE ALREADY - WE HAVE TO MOVE IT LEFT OR RIGHT
            blockMovement.DisplaceBlock(blockGrid[newCol, row], blockScript.MyGridIndex);                         //WE DISPLACE THE BLOCK THAT IS ALREADY THERE TO WHERE THE NEW BLOCK WAS  
            if (blockGrid[column, row].myGameObject == null)                                                      //WE CATEGORIZE WHERE THE OLD BLOCK HAS GONE
                PlaceNewBlock(row, newCol);                                                                       //even if it's empty
            else
                PlaceNewBlock(row, newCol, blockGrid[column, row].myGameObject.GetComponent<BlockIndividual>());  //and an overloaded version of the function if it's not
            PlaceNewBlock(row, column, blockScript);                                                              //THEN WE OFFICIALLY CATEGORIZE THE NEW BLOCK
    }

#endregion
}
