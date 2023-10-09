using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GenericGrid<TGridObject> // TGridObject defines type to populate grid with.
{

    private int width;
    private int height;
    private TGridObject[,] mapGridArray;        // Two dimensional array of int type.
    private float cellSize;
    private TextMesh[,] debugTextArray;
    private Vector3 originPosition;

    public bool debugMode;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    /// <summary>
    /// Creates a two dimensional grid of given dimensions and cell size, starting creation from a given origin point. 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    /// <param name="originPosition"></param>
    /// <param name="debugMode"></param>
    /// <param name="createGridObject"></param>
    public GenericGrid(int width, int height, float cellSize, Vector3 originPosition, bool debugMode, Func<GenericGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        //Creates a 2D array of type TGridObject, which is defined when the grid is created.
        mapGridArray = new TGridObject[width, height];

        for (int x = 0; x < mapGridArray.GetLength(0); x++)
        {
            for (int z = 0; z < mapGridArray.GetLength(1); z++)
            {
                //Uses the func delegate to create a new object of type TGridObject at each grid cell.
                mapGridArray[x, z] = createGridObject(this, x, z);
            }
        }
        //If debug mode is enabled, draw grid lines and create textmeshes to display grid values.
        if (debugMode)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            // Draw grid lines
            for (int x = 0; x < mapGridArray.GetLength(0); x++)
            {
                for (int z = 0; z < mapGridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = CreateWorldText(mapGridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            // Draw boundary lines of the grid
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => 
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = mapGridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    //Given grid co-ordinate return the world position of that grid cell.
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    //Get XZ coordinates corresponding to grid in world position.
    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }


    //Set value based on Grid Coordinate
    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            mapGridArray[x, z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    //Set value based on world position. 
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetGridObject(x, z, value);
    }

    //Get value from a given grid co-ordinate.
    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return mapGridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    //Get reference to grid object from world position.
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    //Return a list of all grid cell world positions that contain a given value.
    public List<Vector3> GetPositionsFromValue(TGridObject value)
    {
        List<Vector3> vectorList = new List<Vector3>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (mapGridArray[x, z].Equals(value))
                {
                    vectorList.Add(GetWorldPosition(x, z));
                }
            }
        }
        return vectorList;
    }


    //Return a list of all grid cell co-ordinates that contain a given value.
    public List<Vector3> GetGridPositionsFromValue(TGridObject value)
    {
        // Use Vector3 list, but for each value in list use .x and .z to get grid co-ordinates
        List<Vector3> vectorList = new List<Vector3>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (mapGridArray[x, z].Equals(value))
                {
                    vectorList.Add(new Vector3(x, 0f, z));
                }
            }
        }
        return vectorList;
    }

    //Event to update the debug textmesh when a cell value is changed.
    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    //Function to create world text within a grid cell for debug purposes courtesy of CodeMonkey in his UtilsClass package.
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
