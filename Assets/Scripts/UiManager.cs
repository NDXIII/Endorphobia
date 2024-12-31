using UnityEngine;
using UnityEngine.UI;


public enum UiScreen {
    None,
    MainMenu,
    Pause,
    Gameplay
}

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }
    public UiScreen uiScreen { get; private set; } = UiScreen.MainMenu;
    public GameObject mainMenuScreen;
    public GameObject pauseScreen;
    public GameObject gameplayScreen;
    public Button btnStartGame;
    public Button btnQuit;


    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) { 
            Destroy(this);
        }
        else { 
            Instance = this; 
        }

        // Configure screens
        mainMenuScreen.SetActive(true);
        pauseScreen.SetActive(false);
        gameplayScreen.SetActive(false);
    }


    public void ShowPauseScreen() {
        // Return if main menu already opened
        if (uiScreen == UiScreen.Pause) {
            return;
        }

        // Hide gameplay screen
        gameplayScreen.SetActive(false);

        // Show pause screen
        pauseScreen.SetActive(true);
        uiScreen = UiScreen.Pause;
    }


    public void OnStartButton() {
        mainMenuScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        uiScreen = UiScreen.Gameplay;
        GameManager.Instance.StartGame();
    }

    public void OnResumeButton() {
        pauseScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        uiScreen = UiScreen.Gameplay;
        GameManager.Instance.ResumeGame();
    }

    public void OnQuitButton() {
        uiScreen = UiScreen.None;
        Application.Quit();
    }
}
