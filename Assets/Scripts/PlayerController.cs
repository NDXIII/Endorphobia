using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("General Parameters")]
    public Transform spawnPoint;
    public LayerMask interactableLayer;

    [Header("Move Parameter")]
    public bool canMove = true;
    public bool isSprinting = false;
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 9.81f;

    [Header("Camera Parameter")]
    public Camera fpsCamera;
    public float lookSpeed = 0.5f;
    public float lookLimit = 90f;

    [Header("Audio")]
    public AudioClip[] stepSounds;
    public float stepDistance = 3f;
    public float stepVolume = 0.25f;
    public AudioClip jumpSound;
    public float jumpVolume = 0.25f;
    private AudioSource audioSource;
    private float lastStepDistance = 0f;


    private CharacterController characterController;
    private Vector3 moveVelocity;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotation;
    private NightVisionTool nightVision;
    private BaitTool baitTool;
    

    private void Awake()
    {
        // Get components
        characterController = GetComponent<CharacterController>();
        nightVision = GetComponentInChildren<NightVisionTool>();
        baitTool = GetComponentInChildren<BaitTool>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Player movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float currentSpeedX = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * moveInput.y : 0f;
        float currentSpeedY = canMove ? (isSprinting ? sprintSpeed : walkSpeed) * moveInput.x : 0f;

        float movementVelocity = moveVelocity.y;
        moveVelocity = (forward * currentSpeedX) + (right * currentSpeedY);

        // Gravity
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

        //Debug.Log($"Last step distance: {lastStepDistance} + MoveVelocity: {moveVelocity}");
        // Play random step sound if enough distance has been covered
        float moveVelocityXZ = new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude * Time.deltaTime;
        //Debug.Log($"MoveVelocityXZ: {moveVelocityXZ}");
        if (moveVelocityXZ > 0f && characterController.isGrounded)
        {
            lastStepDistance += moveVelocityXZ;
            if (lastStepDistance > stepDistance)
            {
                // Play random step sound with random pitch
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)], stepVolume);
                
                lastStepDistance = 0f;
            }
        }

        characterController.Move(moveVelocity * Time.deltaTime);
    }


    private void Interact()
    {
        // Get all interactable objects in range
        Collider[] collider = Physics.OverlapSphere(fpsCamera.transform.position, Interactable.pickupRadius, interactableLayer);

        // Check if there are any interactable objects
        if (collider.Length > 0)
        {
            // Get the interactable component
            Interactable interactable = collider[0].GetComponent<Interactable>();

            // Interact with the object if valid
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }


    public void Reset() {
        // Reset player transform
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        // Reset camera rotation
        rotation = 0f;

        // Reset tools
        nightVision.ChargeBattery(1f);
        baitTool.SetStock(1);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the player has collided with the boss
        if (hit.gameObject.CompareTag("Boss"))
        {
            // Player is dead now
            GameManager.Instance.SetState(GameState.Dead);
        }
    }

    public void OnInteractablePickedUp(InteractableType type, float amount) {
        switch (type) {
            case InteractableType.Battery:
                nightVision.GetComponent<NightVisionTool>().ChargeBattery(amount);
                break;
            default:
                baitTool.Refill();
                break;
        }
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
            audioSource.PlayOneShot(jumpSound, jumpVolume);
        }
    }

    public void HandleLookInput(InputAction.CallbackContext ctx) {
        lookInput = ctx.ReadValue<Vector2>();
    }

    public void HandlePauseInput(InputAction.CallbackContext ctx) {
        if (ctx.performed && GameManager.Instance.gameState != GameState.Dead)
        {
            GameManager.Instance.SetState(GameManager.Instance.gameState == GameState.Playing ? GameState.Paused : GameState.Playing);
        }
    }

    public void HandleThrowBaitInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canMove)
        {
            baitTool.Throw();
        }
    }

    public void HandleInteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canMove)
        {
            Interact();
        }
    }

    public void HandleNightVisionInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canMove) {
            nightVision.Toggle();
        }
    }
}