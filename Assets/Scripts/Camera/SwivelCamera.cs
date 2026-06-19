using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

/// <summary>
/// Allows the player to zoom in and out as well as swivel around the character
/// </summary>
public class SwivelCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceZ = 10.0f;
    [SerializeField] private float heightY = 2.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private float maxHeight = 1.0f;
    [SerializeField] private float minHeight = 1.0f;
    [SerializeField] private float swivelRadius = 1.0f;
    [SerializeField] private Vector2 mouseDelta;
    [SerializeField] private Vector2 scrollInput;
    private bool holdingRightMouse = false;

    private void OnEnable()
    {
        camera.transform.localPosition = new Vector3(0, heightY, distanceZ);
    }

    private void FixedUpdate()
    {
        SwivelCameraFunction();
    }

    public void SwivelCameraAction(InputAction.CallbackContext context)
    {
        // need to check with state machine if the player camera should be rotating when holding right click
        Debug.Log("Pressed right mouse");
        if (context.started)
        {
            holdingRightMouse = true;
        }
        else if (context.canceled)
        {
            holdingRightMouse = false;
            mouseDelta = Vector2.zero;
        }
    }

    private void SwivelCameraFunction()
    {
        if (holdingRightMouse == true)
        {
            mouseDelta = Pointer.current.delta.ReadValue();

            if (mouseDelta.x > 0)
            {
                Debug.Log("Mouse right");
                transform.RotateAround(target.position, Vector3.down, (Time.deltaTime * rotationSpeed) * mouseDelta.x);
            }
            else if (mouseDelta.x < 0)
            {
                Debug.Log("Mouse left");
                transform.RotateAround(target.position, Vector3.up, (Time.deltaTime * rotationSpeed) * -mouseDelta.x);
            }
        }
    }

    public void ZoomCameraAction(InputAction.CallbackContext context)
    {
        // need to check with state machine if the player camera should be zooming in when scrolling

        scrollInput = context.action.ReadValue<Vector2>();

        if (scrollInput.y > 0)
        {
            if (camera.transform.position.y <= minHeight) { return; }
            camera.transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
        }
        else if (scrollInput.y < 0)
        {
            if (camera.transform.position.y >= maxHeight) { return; }
            camera.transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
        }
    }
}
