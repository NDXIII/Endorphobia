using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Camera mainMenuCamera;
    public GameObject playerObject;
    

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
        playerObject.SetActive(false);
    }


    public void StartGame() {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Disable menu camera
        mainMenuCamera.enabled = false;

        // Enable player
        playerObject.SetActive(true);
    }

    public void PauseGame() {
        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freeze game
        Time.timeScale = 0f;
        playerObject.GetComponent<PlayerController>().canMove = false;

        // Show pause screen
        UiManager.Instance.ShowPauseScreen();
    }

    public void ResumeGame() {
        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Unfreeze game
        playerObject.GetComponent<PlayerController>().canMove = true;
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
}