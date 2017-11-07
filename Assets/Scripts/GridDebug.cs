using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDebug : MonoBehaviour
{

    #region Private Variables
    public static GridDebug publicGrid;

    [Header("Debug Variables")]
    [SerializeField]
    bool showGridCenterLines;
    [SerializeField] bool showGridOutlines;
    [SerializeField] bool showGridOrigin;

    [Header("Grid Variables")]
    [SerializeField] Vector2 gridOrigin;
    [SerializeField] int columns;
    [SerializeField] int rows;
    [SerializeField] float rowHeight, columnWidth;
    [SerializeField] float firstColumnXPosition, finalColumnXPosition;
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

    public Vector2 GridOrigin
    {
        get
        {
            return gridOrigin;
        }

        set
        {
            gridOrigin = value;
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

    #endregion

    #region Unity Functions
    void OnEnable()
    {
        publicGrid = this;
    }
    private void Start()
    {
        firstColumnXPosition = gridOrigin.x + columnWidth / 2;
        finalColumnXPosition = gridOrigin.x + (columnWidth * (columns-1)) + columnWidth / 2;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3 (firstColumnXPosition,0,0), Vector3.one / 3);
        Gizmos.DrawCube(new Vector3(finalColumnXPosition, 0, 0), Vector3.one / 3);
        Gizmos.color = Color.cyan;
        if (showGridOrigin)
            Gizmos.DrawSphere(gridOrigin, .2f);

        int i = 0;
        Vector2 startingPosition;

        if (showGridCenterLines)
        {
            Gizmos.color = Color.red;

            for (i = 0; i < columns; i++)
            {
                startingPosition = new Vector2((gridOrigin.x + i * columnWidth) + columnWidth / 2, gridOrigin.y);
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x, startingPosition.y + rowHeight * rows));
            }
            Gizmos.color = Color.green;
            for (i = 0; i < rows; i++)
            {
                startingPosition = new Vector2(gridOrigin.x, (gridOrigin.y + i * rowHeight) + rowHeight / 2);
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x + columnWidth * columns, startingPosition.y));
            }
        }
        if (showGridOutlines)
        {
            Gizmos.color = Color.blue;
            for (i = 0; i < columns + 1; i++)
            {
                startingPosition = new Vector2((gridOrigin.x + i * columnWidth), gridOrigin.y);
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x, startingPosition.y + rowHeight * rows));
            }
            Gizmos.color = Color.magenta;
            for (i = 0; i < rows + 1; i++)
            {
                startingPosition = new Vector2(gridOrigin.x, (gridOrigin.y + i * rowHeight));
                Gizmos.DrawLine(startingPosition, new Vector2(startingPosition.x + columnWidth * columns, startingPosition.y));
            }
        }
    }
    #endregion
}
