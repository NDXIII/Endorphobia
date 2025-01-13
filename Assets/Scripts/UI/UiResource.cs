using System;
using UnityEngine;
using UnityEngine.UI;

public class UiResource : MonoBehaviour
{
    public float charge { get; private set; } = 1f;
    public Image image;


    public void SetCharge(float charge)
    {
        this.charge = Math.Clamp(charge, 0f, 1f);
        image.fillAmount = charge;
    }
}
