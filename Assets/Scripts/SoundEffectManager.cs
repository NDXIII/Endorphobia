using UnityEngine;

public enum SoundEffect {
    Click,
    Switch,
    Throw,
    Step,
    Jump,
    Catch,
    Pickup,
    Trap
}

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip[] stepSounds;
    public AudioClip clickSound;
    public AudioClip switchSound;
    public AudioClip throwSound;
    public AudioClip jumpSound;
    public AudioClip catchSound;
    public AudioClip pickupSound;
    public AudioClip trapSound;

    [Header("Volume")]
    public float clickVolume = 0.5f;
    public float switchVolume = 0.5f;
    public float throwVolume = 0.5f;
    public float stepVolume = 0.25f;
    public float jumpVolume = 0.25f;
    public float catchVolume = 0.5f;
    public float pickupVolume = 0.5f;
    public float trapVolume = 0.5f;

    private AudioSource audioSource;


    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) { 
            Destroy(this);
        } 
        else { 
            Instance = this; 
        }

        // Get components
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(SoundEffect soundEffect) {
        // Play corresponding sound effect
        switch(soundEffect)
        {
            case SoundEffect.Click:
                audioSource.PlayOneShot(clickSound, clickVolume);
                break;
            case SoundEffect.Switch:
                audioSource.PlayOneShot(switchSound, switchVolume);
                break;
            case SoundEffect.Throw:
                audioSource.PlayOneShot(throwSound, throwVolume);
                break;
            case SoundEffect.Step:
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)], stepVolume);
                break;
            case SoundEffect.Jump:
                audioSource.PlayOneShot(jumpSound, jumpVolume);
                break;
            case SoundEffect.Catch:
                audioSource.PlayOneShot(catchSound, catchVolume);
                break;
            case SoundEffect.Pickup:
                audioSource.PlayOneShot(pickupSound, pickupVolume);
                break;
            case SoundEffect.Trap:
                audioSource.PlayOneShot(trapSound, trapVolume);
                break;
        }
    }
}
