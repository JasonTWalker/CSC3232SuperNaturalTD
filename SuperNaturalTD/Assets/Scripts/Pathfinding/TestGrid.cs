using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    //Public variables to define grid size and cell size, can be defined in unity inspector.
    public int width;
    public int height;
    public int cellSize;
    public Vector3 originPosition;
    public bool debugMode;

    //GenericGrid<int> shows utilization of generic grid to create a 2D grid of int type.
    private GenericGrid<int> genericGrid;

    void Start()
    {
        //Paramters are defined within the unity game engine inspector allowing for easy customization.and testing of various grid sizes.
        genericGrid = new GenericGrid<int>(width, height, cellSize, originPosition, debugMode, (GenericGrid<int> g, int x, int y) => new int());
    }
}