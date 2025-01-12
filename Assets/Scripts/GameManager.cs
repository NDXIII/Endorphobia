using Unity.Behavior;
using UnityEngine;

public enum GameState {
    MainMenu,
    Paused,
    Dead,
    Playing
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState gameState { get; private set; } = GameState.MainMenu;
    public float playTimeSeconds { get; private set; } = 0f;
    public GameObject menuCamera;

    [Header("Spawners")]
    public ObjectSpawner batterySpawner;
    public ObjectSpawner baitSpawner;

    [Header("Game Objects")]
    public GameObject player;
    public GameObject boss;

    private PlayerController playerController;


    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) { 
            Destroy(this);
        } 
        else { 
            Instance = this; 
        }

        // Get components
        playerController = player.GetComponent<PlayerController>();
    }

    private void Start() {
        // Set initial game state
        SetState(GameState.MainMenu);
    }

    private void Update() {
        // Update play time
        if (gameState == GameState.Playing) {
            playTimeSeconds += Time.deltaTime;
        }
    }


    public void SetState(GameState state) {
        // Switch to corresponding game state
        switch(state)
        {
            case GameState.MainMenu:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                player.SetActive(false);
                menuCamera.SetActive(true);
                playTimeSeconds = 0f;
                playerController.Reset();
                boss.GetComponent<Boss>().ResetBoss();
                Time.timeScale = 1f;
                UiManager.Instance.ShowScreen(UiScreen.MainMenu);
                break;

            case GameState.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                menuCamera.SetActive(false);
                player.SetActive(true);
                playerController.canMove = false;
                Time.timeScale = 0f;
                UiManager.Instance.ShowScreen(UiScreen.Pause);
                break;

            case GameState.Dead:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                menuCamera.SetActive(false);
                player.SetActive(true);
                playerController.canMove = false;
                Time.timeScale = 0f;
                boss.GetComponent<BehaviorGraphAgent>().enabled = false;
                UiManager.Instance.ShowScreen(UiScreen.Death);
                break;

            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                menuCamera.SetActive(false);
                player.SetActive(true);
                playerController.canMove = true;
                Time.timeScale = 1f;
                boss.GetComponent<BehaviorGraphAgent>().enabled = true;
                UiManager.Instance.ShowScreen(UiScreen.Gameplay);
                break;
        }

        // Apply new game state
        gameState = state;
    }

    public void OnInteractablePickedUp(InteractableType type, float amount) {
        // Spawn a new interactables
        switch (type) {
            case InteractableType.BatteryNormal:
            case InteractableType.BatteryTrap:
                batterySpawner.SpawnAdditionalObjects(1);
                break;
            default:
                baitSpawner.SpawnAdditionalObjects(1);
                break;
        }

        // Inform player that he has picked up an interactable
        playerController.OnInteractablePickedUp(type, amount);
    }
}