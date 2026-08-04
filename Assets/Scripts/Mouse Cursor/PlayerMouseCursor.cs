using UnityEngine;

/// <summary>
/// Gets the location of the players cursor in 3D space or screen space
/// </summary>
public static class PlayerMouseCursor 
{
    public static Vector3 GetMousePosition3D()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            
            Debug.Log("3D World Position: " + hitInfo.point);
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static Vector3 GetMousePosition2D()
    {
        return Input.mousePosition;
    }
}
