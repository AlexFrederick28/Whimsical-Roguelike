using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

/// <summary>
/// Velocity movement for the player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterView;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector2 inputVector;
    [SerializeField] private Vector3 moveDirection;

    private void Update()
    {
        Movement();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    private void Movement()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0.0f;
        camRight.y = 0.0f;

        moveDirection = (camForward.normalized * inputVector.y).normalized + (camRight * inputVector.x).normalized;
        characterController.Move(moveDirection * speed * Time.deltaTime);

        characterView.rotation = Quaternion.LookRotation(moveDirection);
    }
}
