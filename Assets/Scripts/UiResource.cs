using TMPro;
using UnityEngine;

public class UiResource : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public TMP_Text detailText;
    public float opacityDeactivated = 0.25f;

    public bool active { get; private set; } = true;


    private void Awake() {
        canvasGroup.alpha = 1.0f;
    }


    public void Toggle() {
        active = !active;
        canvasGroup.alpha = active ? 1.0f : opacityDeactivated;
    }

    public void SetDetailText(string text) {
        detailText.SetText(text);
    }
}