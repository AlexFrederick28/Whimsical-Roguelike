using System.Collections;
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
    [SerializeField] private float fallingMoveSpeed;
    [SerializeField] private float gravity = -9.81f; // default unity gravity
    [SerializeField] private float weight = 2; 
    [SerializeField] private float jumpHeight = 10f;
    [Tooltip("Keep negative number")]
    [SerializeField] private float jumpCurve = -2f; 
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterView;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector2 inputVector;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 fallVelocity;

    [Space]
    [Header("Ground Detection")]
    [SerializeField] private float sphereRadius = 0.15f;
    [SerializeField] private Collider collider;
    [SerializeField] private Vector3 colliderBottom;    
    [SerializeField] private LayerMask colliderLayer;
    private RaycastHit hit;
    private bool isJumping = false;

    private bool moveCancelled = false;

    private void OnEnable()
    {
        // player movement
        GameManager.globalInputActions.Player.Move.performed += OnMove;
        GameManager.globalInputActions.Player.Move.canceled += OnMove;
        GameManager.globalInputActions.Player.Move.started += OnMove;

        GameManager.globalInputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        // player movement
        GameManager.globalInputActions.Player.Move.performed -= OnMove;
        GameManager.globalInputActions.Player.Move.canceled -= OnMove;
        GameManager.globalInputActions.Player.Move.started -= OnMove;

        GameManager.globalInputActions.Player.Jump.performed += OnJump;
    }

    private void Update()
    {
        GetGroundedDetectionPosition();
        IsGrounded();
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

        if (IsGrounded() == false)
        {
            // applies gravity over time (for falling) - character controller needs constant gravity to be touching the ground
            //Debug.Log("Applying gravity");
            fallVelocity.y += gravity * weight * Time.deltaTime;
            speed = fallingMoveSpeed;
        }
        else if (IsGrounded() == true && isJumping == false)
        {
            // resets force of gravity when grounded
            //Debug.Log("reset gravity");
            fallVelocity.y = -2f; // Slight downward force keeps character pinned to slopes
            speed = runSpeed;
        }

        // constantly applying gravity
        characterController.Move(fallVelocity * Time.deltaTime);

        if (moveCancelled || moveDirection == Vector3.zero) { return; } // stops the character from rotating back to the default position
        characterView.rotation = Quaternion.LookRotation(moveDirection);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            //Debug.Log("Jumped");
            StartCoroutine(JumpC());
            fallVelocity.y = Mathf.Sqrt(jumpHeight * jumpCurve * gravity);
        }
    }

    public IEnumerator JumpC()
    {
        // jumping needs a slight delay in gravity so the jump can occur
        isJumping = true;

        yield return new WaitForSeconds(0.1f);

        isJumping = false;
    }

    public bool IsGrounded()
    {
        if (hit.transform != null)
        {
            //Debug.Log(hit.transform.name);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(colliderBottom, sphereRadius);
        Gizmos.DrawWireSphere(new Vector3(colliderBottom.x, colliderBottom.y - 1f, colliderBottom.z), sphereRadius);
    }

    private void GetGroundedDetectionPosition()
    {
        colliderBottom = new Vector3(collider.bounds.center.x, collider.bounds.min.y + 1f, collider.bounds.center.z);

        Physics.SphereCast(colliderBottom, sphereRadius, Vector3.down, out hit, 1f, colliderLayer);
    }
}
