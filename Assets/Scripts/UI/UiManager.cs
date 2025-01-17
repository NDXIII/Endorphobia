using UnityEngine;
using UnityEngine.UI;


public enum UiScreen {
    None,
    MainMenu,
    Pause,
    Death,
    Gameplay
}

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    public UiScreen uiScreen { get; private set; } = UiScreen.MainMenu;

    [Header("UI Screens")]
    public GameObject mainMenuScreen;
    public GameObject pauseScreen;
    public GameObject deathScreen;
    public GameObject gameplayScreen;

    [Header("Buttons")]
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

        // Show main menu
        ShowScreen(UiScreen.None);
    }
    

    public void ShowScreen(UiScreen screen) {
        // Show corresponding screen
        switch(screen)
        {
            case UiScreen.MainMenu:
                mainMenuScreen.SetActive(true);
                pauseScreen.SetActive(false);
                deathScreen.SetActive(false);
                gameplayScreen.SetActive(false);
                break;

            case UiScreen.Pause:
                mainMenuScreen.SetActive(false);
                pauseScreen.SetActive(true);
                deathScreen.SetActive(false);
                gameplayScreen.SetActive(false);
                SoundEffectManager.Instance.Play(SoundEffect.Click);
                break;

            case UiScreen.Death:
                mainMenuScreen.SetActive(false);
                pauseScreen.SetActive(false);
                deathScreen.SetActive(true);
                gameplayScreen.SetActive(false);
                break;

            case UiScreen.Gameplay:
                mainMenuScreen.SetActive(false);
                pauseScreen.SetActive(false);
                deathScreen.SetActive(false);
                gameplayScreen.SetActive(true);
                SoundEffectManager.Instance.Play(SoundEffect.Click);
                break;

            default:
                mainMenuScreen.SetActive(false);
                pauseScreen.SetActive(false);
                deathScreen.SetActive(false);
                gameplayScreen.SetActive(false);
                break;
        }

        // Update screen
        uiScreen = screen;
    }


    public void OnStartButton() {
        GameManager.Instance.SetState(GameState.Playing);
    }

    public void OnResumeButton() {
        GameManager.Instance.SetState(GameState.Playing);
    }

    public void OnMainMenuButton() {
        SoundEffectManager.Instance.Play(SoundEffect.Click);
        GameManager.Instance.SetState(GameState.MainMenu);
    }

    public void OnQuitButton() {
        SoundEffectManager.Instance.Play(SoundEffect.Click);
        Application.Quit();
    }
}
