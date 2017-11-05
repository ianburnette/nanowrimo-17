using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BlockType
{
    none,
    fire,
    ice,
    ghost,
    crate,
    spirit,
    water, 
    wood
}
[Serializable]
public struct Block
{
    public GameObject myGO;
    public BlockType myType;
}
[Serializable]
public class BlockCell
{
    public bool empty = true;
    public Block thisBlock;
}

public class ColumnManagement : MonoBehaviour {
    #region Private Variables
    [SerializeField] int columnCount, maxRows, firstActiveRow;
    [SerializeField] BlockCell[][] columns;
    [SerializeField] BlockCell[] column0, column1, column2, column3, column4, column5, column6;

    [SerializeField] BlockCell currentBlockCell;
    [SerializeField] bool currentBlockEmpty;
    [SerializeField] GameObject currentBlockGO;
    [SerializeField] BlockType currentBlockType;
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

    public BlockCell[][] Columns
    {
        get
        {
            return columns;
        }

        set
        {
            columns = value;
        }
    }
    #endregion

    #region Unity Functions
    void Start () {
        column0 = column1 = column2 = column3 = column4 = column5 = column6 = new BlockCell[maxRows];
        for (int i = 0; i < column0.Length; i++)
            column0[i] = new BlockCell();
        for (int i = 0; i < column1.Length; i++)
            column1[i] = new BlockCell();
        for (int i = 0; i < column2.Length; i++)
            column2[i] = new BlockCell();
        for (int i = 0; i < column3.Length; i++)
            column3[i] = new BlockCell();
        for (int i = 0; i < column4.Length; i++)
            column4[i] = new BlockCell();
        for (int i = 0; i < column5.Length; i++)
            column5[i] = new BlockCell();
        for (int i = 0; i < column6.Length; i++)
            column6[i] = new BlockCell();


        columns = new BlockCell[7][];
        columns[0] = column0;
        columns[1] = column1;
        columns[2] = column2;
        columns[3] = column3;
        columns[4] = column4;
        columns[5] = column5;
        columns[6] = column6;
    }
	
	void Update () {
		
	}
#endregion

#region Custom Functions
    public bool CellEmpty(int testRow, int testColumn)
    {
        //print("checking empty: row " + testRow + " and column " + testColumn);
        // currentBlockCell = GetCellData(testRow, testColumn);


        if (GetCellEmptyState(testRow, testColumn))
            return true;
        //if (GetCellData(testRow, testColumn).empty)
          //  return true;
        else
            return false;
    }
    public void CategorizeBlock(int row, int column, Block thisBlock)
    {
        //check for current occupant of cell here
       // columns[column][row].empty = false;
      //  columns[column][row].thisBlock = thisBlock;
    }
    bool GetCellEmptyState(int row, int column)
    {
        BlockCell currentCell = GetCellData(row, column);
        //print("current cell is " + currentBlockCell.containsBlock);
        return currentCell.empty;// ? true : false;
    }
    BlockCell GetCellData(int currentRow, int currentColumn)
    {
       // print("getting cell data for [" + currentColumn + "][" + currentRow + "]");

        // print("columns length is " + columns[currentColumn].Length);
       // currentBlockEmpty = column0[currentRow].containsBlock;//columns[currentColumn][currentRow].empty;
      // if (column0[currentRow].thisBlock.myGO != null)
       //     currentBlockGO = column0[currentRow].thisBlock.myGO;
        //currentBlockType = columns[currentColumn][currentRow].thisBlock.myType;

        return columns[currentColumn][currentRow];
        /*
        if (currentColumn == 0)
        {
            print("in column if statement");
            return column0[currentRow];
        }

        switch (currentColumn)
        {
            case 0:
                print("in 0");
                return column0[currentRow];
            case 1:
                print("in 1");
                return column1[currentRow];
            case 2:
                print("in 2");
                return column2[currentRow];
            case 3:
                print("in 3");
                return column3[currentRow];
            case 4:
                return column4[currentRow];
            case 5:
                return column5[currentRow];
            case 6:
                return column6[currentRow];
        }
        Debug.LogError("didn't find the right cell");
        return column0[currentRow];*/
    }
#endregion
}
