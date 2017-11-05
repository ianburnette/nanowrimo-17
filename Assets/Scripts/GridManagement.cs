using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagement : MonoBehaviour
{

    #region Private Variables
    public static GridManagement publicGrid;

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

    #endregion

    #region Unity Functions
    void OnEnable()
    {
        publicGrid = this;
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {
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

    #region Custom Functions

    #endregion
}
