using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

/// <summary>
/// Velocity movement for the player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private float speed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float gravity = -9.81f; // default unity gravity
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterView;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector2 inputVector;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 fallVelocity;

    private bool moveCancelled = false;

    private void OnEnable()
    {
        // player movement
        GameManager.globalInputActions.Player.Move.performed += OnMove;
        GameManager.globalInputActions.Player.Move.canceled += OnMove;
        GameManager.globalInputActions.Player.Move.started += OnMove;
    }

    private void OnDisable()
    {
        // player movement
        GameManager.globalInputActions.Player.Move.performed -= OnMove;
        GameManager.globalInputActions.Player.Move.canceled -= OnMove;
        GameManager.globalInputActions.Player.Move.started -= OnMove;
    }

    private void Update()
    {
        Movement();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            moveCancelled = true;
        }
        else
        {
            moveCancelled = false;
        }
    }

    private void Movement()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0.0f;
        camRight.y = 0.0f;

        moveDirection = (camForward.normalized * inputVector.y).normalized + (camRight * inputVector.x).normalized;
        characterController.Move(moveDirection.normalized * speed * Time.deltaTime);

        // resets force of gravity when grounded
        if (characterController.isGrounded && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f; // Slight downward force keeps character pinned to slopes
            characterController.Move(fallVelocity * Time.deltaTime);
            speed = runSpeed;
        }

        // applies gravity over time (for falling) - character controller needs constant gravity to be touching the ground
        fallVelocity.y += gravity * Time.deltaTime;
        characterController.Move(fallVelocity * Time.deltaTime);
        speed = fallSpeed;

        if (moveCancelled || moveDirection == Vector3.zero) { return; } // stops the character from rotating back to the default position
        characterView.rotation = Quaternion.LookRotation(moveDirection);
    }
}
