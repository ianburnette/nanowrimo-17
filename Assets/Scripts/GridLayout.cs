using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayout : MonoBehaviour
{

    #region Private Variables
    public static GridLayout publicGrid;

    [Header("Debug Variables")]
    [SerializeField] bool showGridCenterLines;
    [SerializeField] bool showGridOutlines;
    [SerializeField] bool showGridOrigin;

    [Header("Grid Variables")]
    [SerializeField] Vector2 gridBottomLeft;
    [SerializeField] Vector2 gridTopLeft, bottomLeftOffset, topLeftOffset;
    [SerializeField] int columns;
    [SerializeField] int rows;
    [SerializeField] float rowHeight, columnWidth;
    [SerializeField] float firstColumnXPosition, firstRowYPosition, finalColumnXPosition;
    #endregion

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
    #endregion

    #region Unity Functions
    void OnEnable()
    {
        publicGrid = this;
    }
    private void Start()
    {
        gridBottomLeft = (Vector2)transform.position + bottomLeftOffset;
        gridTopLeft = (Vector2)transform.position + topLeftOffset;
        firstColumnXPosition = gridBottomLeft.x + columnWidth / 2;
        finalColumnXPosition = gridBottomLeft.x + (columnWidth * (columns-1)) + columnWidth / 2;
        firstRowYPosition = gridTopLeft.x + rowHeight / 2;
    }
    private void Update()
    {
        gridTopLeft = new Vector2(gridBottomLeft.x, gridBottomLeft.y + (rows * rowHeight));
    }
    private void OnDrawGizmos()
    {
        gridBottomLeft = (Vector2)transform.position + bottomLeftOffset;
        gridTopLeft =    (Vector2)transform.position + topLeftOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3 (firstColumnXPosition,0,0), Vector3.one / 3);
        Gizmos.DrawCube(new Vector3(finalColumnXPosition, 0, 0), Vector3.one / 3);
        Gizmos.color = Color.cyan;
        if (showGridOrigin)
        {
            Gizmos.DrawSphere((Vector2)transform.position, .2f);
            Gizmos.DrawSphere(gridBottomLeft, .2f);
            Gizmos.DrawSphere(gridTopLeft,    .2f);
        }
            

        int i = 0;
        Vector2 startingPosition;

        if (showGridCenterLines)
        {
            Gizmos.color = Color.red;

            for (i = 0; i < columns; i++)
            {
                startingPosition = new Vector2((gridBottomLeft.x + i * columnWidth) + columnWidth / 2, gridBottomLeft.y);
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x, startingPosition.y + rowHeight * rows));
            }
            Gizmos.color = Color.green;
            for (i = 0; i < rows; i++)
            {
                startingPosition = new Vector2(gridBottomLeft.x, (gridBottomLeft.y + i * rowHeight) + rowHeight / 2);
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x + columnWidth * columns, startingPosition.y));
            }
        }
        if (showGridOutlines)
        {
            Gizmos.color = Color.blue;
            for (i = 0; i < columns + 1; i++)
            {
                startingPosition = new Vector2((gridBottomLeft.x + i * columnWidth), gridBottomLeft.y);
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x, startingPosition.y + rowHeight * rows));
            }
            Gizmos.color = Color.magenta;
            for (i = 0; i < rows + 1; i++)
            {
                startingPosition = new Vector2(gridBottomLeft.x, (gridBottomLeft.y + i * rowHeight));
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x + columnWidth * columns, startingPosition.y));
            }
        }
    }
    #endregion
}
