using UnityEngine;

/// <summary>
/// Gets the location of the players cursor in 3D space or screen space
/// </summary>
public static class PlayerMouseCursor 
{
    public static Vector3 GetMousePosition3D()
    {
        Vector3 mousePos = GameManager.globalInputActions.Player.Mouse.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

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

    public static Vector3 GetMousePosition3D(LayerMask layerMask)
    {
        Vector3 mousePos = GameManager.globalInputActions.Player.Mouse.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, layerMask))
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
