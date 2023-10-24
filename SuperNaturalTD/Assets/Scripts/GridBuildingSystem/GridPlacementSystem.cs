using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [SerializeField]
    MouseInputManager mouseInputManager;

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private Grid grid;

    private void Update()
    {
        Vector3 mousePosition = mouseInputManager.GetSelectedMousePosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        gridPosition.y = 1;
        cellIndicator.transform.position = gridPosition;
    }

}
