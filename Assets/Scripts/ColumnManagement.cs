using System;
using UnityEngine;

[Serializable]
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
public struct Block
{
    public BlockType type;
    public GameObject myGameObject;
}

public class ColumnManagement : MonoBehaviour {
    #region Private Variables
    [SerializeField] int columnCount, rowCount, firstActiveRow;

    [SerializeField] Block[,] blockGrid;

    [SerializeField] GameObject currentBlockGO;
    [SerializeField] BlockType currentBlock;
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
#endregion

#region Custom Functions
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
    public void CategorizeBlock(int row, int column, GameObject blockToCategorize)
    {
        //CHECK IF CELL IS OCCUPIED HERE 
        //
        //
        //ASSIGN GIVEN VALUES TO THIS CELL NOW THAT IT'S EMPTY
        print("attempting to categorize block in column " + column + " and row " + row);
        blockGrid[column, row].myGameObject = blockToCategorize;
        blockGrid[column, row].type = SelectRandomType();
    }
    BlockType SelectRandomType()
    {
        BlockType type = GetRandomEnum<BlockType>();
        return type;
    }
    static T GetRandomEnum <T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
#endregion
}
