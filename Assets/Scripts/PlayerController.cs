using Unity.Behavior;
using Unity.VisualScripting;
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

    [Header("Bait Throw Parameters")]
    public GameObject baitPrefab;
    public Transform baitSpawnPoint;
    public float throwForce = 25f;
    public float throwCooldown = 3f;
    private GameObject currentBaitObj;
    private float nextThrowTime = 0f;

    [Header("Interact Parameters")]
    public LayerMask interactableLayer;

    private TestEvent event1;


    private CharacterController characterController;
    private Vector3 moveVelocity;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Flashlight flashlight;
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        flashlight = GetComponentInChildren<Flashlight>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (flashlight == null || characterController == null)
        {
            Debug.LogError("Flashlight or CharacterController not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            event1 = ScriptableObject.CreateInstance<TestEvent>();
            GameManager.Instance.GetBoss().GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "TestEvent").ObjectValue = event1;
            event1.SendEventMessage();
        }

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

    private void ThrowBait()
    {
        // Cooldown
        if (Time.time < nextThrowTime)
        {
            return;
        }
        nextThrowTime = Time.time + throwCooldown;


        // Instantiate bait prefab
        if (currentBaitObj != null)
        {
            Destroy(currentBaitObj);
        }
        currentBaitObj = Instantiate(baitPrefab, baitSpawnPoint.position, transform.rotation);

        // Add force to bait
        Rigidbody rb = currentBaitObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(fpsCamera.transform.forward * throwForce, ForceMode.Impulse);
        }
    }

    private void Interact()
    {
        // Raycast to detect interactable objects
        RaycastHit hit;
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 5f, Color.red, 1f);
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, 5f, interactableLayer))
        {
            //Debug.Log(hit.collider.name);
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }


    public void OnBatteryPickedUp(float chargeAmount) {
        flashlight.GetComponent<Flashlight>().ChargeBattery(chargeAmount);
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

    public void HandlePauseInput(InputAction.CallbackContext ctx) {
        // Check if we are currently not in the pause screen
        if (UiManager.Instance.uiScreen != UiScreen.Pause) {
            // Pause the game
            GameManager.Instance.PauseGame();
        }
        else {
            // Trigger resume button from UI Manager
            UiManager.Instance.OnResumeButton();
        }
    }

    public void HandleThrowBaitInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ThrowBait();
        }
    }

    public void HandleInteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Interact();
        }
    }

    public void HandleFlashlightInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) {
            flashlight.Toggle();
        }
    }
}