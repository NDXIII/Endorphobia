using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float lookSpeed = 2;

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotation;

    private void Start()
    {
        if(cameraTransform == null)
        {
            Debug.LogError("Camera Transform is null in " + this.gameObject);
        }
        if(controller == null)
        {
            Debug.LogError("Character Controller is null in " + this.gameObject);
        }
    }

    private void Update()
    {
        rotation += -lookInput.y * lookSpeed;
        rotation = Mathf.Clamp(rotation, -89, 89);

        cameraTransform.localRotation = Quaternion.Euler(rotation, 0, 0);
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLookInput(InputAction.CallbackContext context) {
        lookInput = context.ReadValue<Vector2>();
    }
}
