using TMPro;
using UnityEngine;

public class UiTimerStats : MonoBehaviour
{
    public TMP_Text txtTimer;

    // Update is called once per frame
    void Update()
    {
        float playTime = GameManager.Instance.playTimeSeconds;
        txtTimer.text = string.Format("{0:00}:{1:00}", (int)(playTime / 60), (int)(playTime % 60));
    }
}
