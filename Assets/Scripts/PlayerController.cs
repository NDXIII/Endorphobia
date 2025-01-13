using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("General Parameters")]
    public Transform spawnPoint;
    public LayerMask interactableLayer;
    public GameObject staminaResourceObject;

    [Header("Move Parameter")]
    public bool canMove = true;
    public bool isSprinting = false;
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float staminaDepletionRate = 0.25f;
    public float staminaRechargeRate = 0.1f;
    public float jumpPower = 7f;
    public float stepDistance = 3f;
    public float gravity = 9.81f;

    [Header("Camera Parameter")]
    public Camera fpsCamera;
    public float lookSpeed = 0.5f;
    public float lookLimit = 90f;
    
    private CharacterController characterController;
    private Vector3 moveVelocity;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotation;
    private float lastStepDistance = 0f;
    private NightVisionTool nightVisionTool;
    private BaitTool baitTool;
    private UiResource staminaResourceClass;
    

    private void Awake()
    {
        // Get components
        characterController = GetComponent<CharacterController>();
        nightVisionTool = GetComponentInChildren<NightVisionTool>();
        baitTool = GetComponentInChildren<BaitTool>();
        staminaResourceClass = staminaResourceObject.GetComponent<UiResource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Player movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        staminaResourceClass.SetCharge(Mathf.Clamp(staminaResourceClass.charge + (isSprinting ? - (Time.deltaTime * staminaDepletionRate) : (Time.deltaTime * staminaRechargeRate)), 0f, 1f));

        float currentSpeedX = canMove ? (isSprinting && staminaResourceClass.charge > 0f ? sprintSpeed : walkSpeed) * moveInput.y : 0f;
        float currentSpeedY = canMove ? (isSprinting && staminaResourceClass.charge > 0f ? sprintSpeed : walkSpeed) * moveInput.x : 0f;

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
                // Play sound effect
                SoundEffectManager.Instance.Play(SoundEffect.Step);
                lastStepDistance = 0f;
            }
        }

        // Apply movement
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
            Interactable interactable = collider[0].GetComponentInParent<Interactable>();

            // Interact with the object if valid
            if (interactable != null)
            {
                interactable.Pickup();
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
        nightVisionTool.ChargeBattery(1f);
        nightVisionTool.SwitchOn(true);
        baitTool.SetStock(1);

        // Reset resources
        staminaResourceClass.SetCharge(1f);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the player has collided with the boss
        if (hit.gameObject.CompareTag("Boss") && GameManager.Instance.gameState == GameState.Playing)
        {
            // Player is dead now
            SoundEffectManager.Instance.Play(SoundEffect.Catch);
            GameManager.Instance.SetState(GameState.Dead);
        }
    }

    public void OnInteractablePickedUp(Interactable interactable) {
        // Interact correspondingly
        switch (interactable.GetType()) {
            case InteractableType.Battery:
                BatteryInteractable batteryInteractable = (BatteryInteractable)interactable;
                nightVisionTool.GetComponent<NightVisionTool>().ChargeBattery(batteryInteractable.pickupChargeAmount);
                SoundEffectManager.Instance.Play(batteryInteractable.trapped ? SoundEffect.Trap : SoundEffect.Pickup);
                break;
            default:
                baitTool.Refill();
                SoundEffectManager.Instance.Play(SoundEffect.Pickup);
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
            SoundEffectManager.Instance.Play(SoundEffect.Jump);
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
            nightVisionTool.Toggle();
        }
    }
}