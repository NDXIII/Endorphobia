using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public bool isSprinting = false;


    [Header("Move Parameter")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 9.81f;

    [Header("Camera Parameter")]
    public Camera fpsCamera;
    public float lookSpeed = 0.5f;
    public float lookLimit = 90f;


    public CharacterController characterController;
    private Vector3 moveVelocity;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float currentSpeedX = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * moveInput.y : 0f;
        float currentSpeedY = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * moveInput.x : 0f;

        float movementVelocity = moveVelocity.y;
        moveVelocity = (forward * currentSpeedX) + (right * currentSpeedY);

        // Jump
        moveVelocity.y = movementVelocity;
        if (!characterController.isGrounded) {
            moveVelocity.y -= gravity * Time.deltaTime;
        }

        // Camera
        if (canMove)
        {
            rotation += -lookInput.y * lookSpeed;
            rotation = Mathf.Clamp(rotation, -lookLimit, lookLimit);

            fpsCamera.transform.localRotation = Quaternion.Euler(rotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);
        }

        characterController.Move(moveVelocity * Time.deltaTime);
    }

    public void HandleMoveInput(InputAction.CallbackContext ctx) {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void HandleSprintInput(InputAction.CallbackContext ctx) {
        if (ctx.performed) {
            isSprinting = true;
        } else if (ctx.canceled) {
            isSprinting = false;
        }
    }

    public void HandleJumpInput(InputAction.CallbackContext ctx) {
        if (ctx.performed && canMove && characterController.isGrounded) {
            moveVelocity.y = jumpPower;
        }
    }

    public void HandleLookInput(InputAction.CallbackContext ctx) {
        lookInput = ctx.ReadValue<Vector2>();
    }
}
