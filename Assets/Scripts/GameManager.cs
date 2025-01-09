using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Camera mainMenuCamera;

    [Header("Spawners")]
    public ObjectSpawner batterySpawner;
    public ObjectSpawner baitSpawner;

    [Header("Game Objects")]
    public GameObject player;
    public GameObject boss;


    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) { 
            Destroy(this);
        } 
        else { 
            Instance = this; 
        }

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable player
        player.SetActive(false);

        // Freeze Game
        Time.timeScale = 0f;
    }


    public void StartGame() {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Disable menu camera
        mainMenuCamera.enabled = false;

        // Enable player
        player.SetActive(true);

        // Unfreeze Game
        Time.timeScale = 1f;
    }

    public void PauseGame() {
        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freeze game
        Time.timeScale = 0f;
        player.GetComponent<PlayerController>().canMove = false;

        // Show pause screen
        UiManager.Instance.ShowScreen(UiScreen.Pause);
    }

    public void ResumeGame() {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Unfreeze game
        player.GetComponent<PlayerController>().canMove = true;
        Time.timeScale = 1f;
    }


    public void OnDeath() {
        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Enable menu camera
        mainMenuCamera.enabled = true;

        // Show respawn screen
        //UiManager.Instance.ShowRespawnScreen();
    }

    public void OnRespawn() {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Disable menu camera
        mainMenuCamera.enabled = false;
    }

    public void OnInteractablePickedUp(InteractableType type, float amount) {
        // Spawn a new interactables
        switch (type) {
            case InteractableType.Battery:
                batterySpawner.SpawnAdditionalObjects(1);
                break;
            default:
                baitSpawner.SpawnAdditionalObjects(1);
                break;
        }

        // Inform player that he has picked up an interactable
        player.GetComponent<PlayerController>().OnInteractablePickedUp(type, amount);
    }
}