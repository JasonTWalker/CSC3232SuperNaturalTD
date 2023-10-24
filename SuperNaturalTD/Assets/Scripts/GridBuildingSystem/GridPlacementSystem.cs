using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [SerializeField]
    MouseInputManager mouseInputManager;

    [SerializeField]
    private GameObject mouseIndicator;

    private void Update()
    {
        Vector3 mousePosition = mouseInputManager.GetSelectedMousePosition();
        mouseIndicator.transform.position = mousePosition;
    }

}
