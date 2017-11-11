using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridLayout : MonoBehaviour
{

    #region Private Variables
    public static GridLayout publicGrid;

    [Header("Debug Variables")]
    [SerializeField] bool showGridCenterLines;
    [SerializeField] bool showGridOutlines;
    [SerializeField] bool showGridOrigin;
    [SerializeField] bool gridInitialized;

    [Header("Grid Variables")]
    [SerializeField] Vector2 gridOrigin;
    [SerializeField] Vector2[,] cellPositions;
    [SerializeField] int columns;
    [SerializeField] int rows;
    [SerializeField] float rowHeight, columnWidth;
    [SerializeField] float firstColumnXPosition, firstRowYPosition, finalColumnXPosition;
    #endregion
    /*
    #region Public Properties
    public float RowHeight
    {
        get
        {
            return rowHeight;
        }

        set
        {
            rowHeight = value;
        }
    }

    public float ColumnWidth
    {
        get
        {
            return columnWidth;
        }

        set
        {
            columnWidth = value;
        }
    }

    public Vector2 GridBottomLeft
    {
        get
        {
            return (Vector2)transform.position + gridBottomLeft;
        }
    }
    public Vector2 GridTopLeft
    {
        get
        {
            return (Vector2)transform.position + gridTopLeft;
        }
    }

    public float FirstColumnXPosition
    {
        get
        {
            return firstColumnXPosition;
        }

        set
        {
            firstColumnXPosition = value;
        }
    }
    public float FinalColumnXPosition
    {
        get
        {
            return finalColumnXPosition;
        }

        set
        {
            finalColumnXPosition = value;
        }
    }
    public float FirstRowYPosition
    {
        get
        {
            return firstRowYPosition;
        }

        set
        {
            firstRowYPosition = value;
        }
    }

    public float[] ColumnPositions
    {
        get
        {
            return columnPositions;
        }

        set
        {
            columnPositions = value;
        }
    }
    public float[] RowPositions
    {
        get
        {
            return rowPositions;
        }

        set
        {
            rowPositions = value;
        }
    }
    #endregion
    */
    #region Unity Functions
    void OnEnable()
    {
        publicGrid = this;
        if (!gridInitialized)
            InitializeGrid();
    }
    private void Start()
    {
       
    }
    private void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
       
    }
    #endregion
    #region Custom Functions
    void InitializeGrid()
    {
        cellPositions = new Vector2[columns, rows];             //this array holds the   
        gridInitialized = true;
    }
    #endregion
}
