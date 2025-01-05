using TMPro;
using UnityEngine;

public class TimerStats : MonoBehaviour
{
    public TMP_Text txtTimer;

    // Update is called once per frame
    void Update()
    {
        float timeSinceStartup = Time.realtimeSinceStartup;
        txtTimer.text = string.Format("{0:00}:{1:00}", (int)(timeSinceStartup / 60), (int)timeSinceStartup);
    }
}
