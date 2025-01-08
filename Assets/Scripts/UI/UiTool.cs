using TMPro;
using UnityEngine;

public class UITool : MonoBehaviour {
    public bool selected { get; private set; } = true;
    
    public CanvasGroup canvasGroup;
    public TMP_Text detailText;
    public float opacityDeactivated = 0.25f;


    private void Awake() {
        canvasGroup.alpha = 1.0f;
    }


    public void Toggle() {
        selected = !selected;
        canvasGroup.alpha = selected ? 1.0f : opacityDeactivated;
    }

    public string GetDetailText(string text) {
        return detailText.text;
    }

    public void SetDetailText(string text) {
        detailText.SetText(text);
    }
}