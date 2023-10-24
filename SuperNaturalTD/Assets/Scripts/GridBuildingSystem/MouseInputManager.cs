using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    // Holds scena camera value so we can raycast from mouse position
    [SerializeField]
    private Camera sceneCamera;

    // Holds last valid position to return if an invalid position is selected
    [SerializeField]
    private Vector3 lastValidPosition;
    // Holds layermask for grid placement
    [SerializeField]
    private LayerMask gridPlacementLayerMask;

    public Vector3 GetSelectedMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = sceneCamera.nearClipPlane;
        Ray rayCast = sceneCamera.ScreenPointToRay(mousePosition);

        RaycastHit raycastHit;
        if (Physics.Raycast(rayCast, out raycastHit, Mathf.Infinity, gridPlacementLayerMask))
        {
            lastValidPosition = raycastHit.point;
            return raycastHit.point;
        }
        else
        {
            return lastValidPosition;
        }   
    }


}
