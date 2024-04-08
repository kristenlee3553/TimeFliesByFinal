using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Manager for the settings screen.
/// <br></br>
/// Authors:
/// <br></br>
/// Cynthia Wang: Sliders, audio, display
/// <br></br>
/// Kristen Lee: Added dialogue slider and code for the page to remember its settings
/// </summary>
public class Settings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider dialogueSlider;
    public AudioMixer musicMixer;
    public AudioMixer soundMixer;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }
    }

    private void Start()
    {
        musicSlider.value = GameManager.s_musicVolume;
        soundSlider.value = GameManager.s_soundFxVolume;
        dialogueSlider.value = GameManager.s_dialogueSpeed;
    }

    // Defines behaviour for BackButton
    public void BackButton()
    {
        if (GameUIHandler.Instance.goToHomePage)
        {
            GameUIHandler.Instance.LoadUIPage("HomePage");
        }
        else
        {
            GameUIHandler.Instance.HideInGameSettings();
        }
        
    }

    // Controls the music volume level
    public void ChangeMusicVolume(float musicSliderValue)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10 (musicSliderValue) * 20);
        GameManager.s_musicVolume = musicSliderValue;
    }

    // Controls the sound volume level
    public void ChangeSoundVolume(float soundSliderValue)
    {
        soundMixer.SetFloat("SoundVolume", Mathf.Log10 (soundSliderValue) * 20);
        GameManager.s_soundFxVolume = soundSliderValue;
    }

    public void ChangeDialogueSpeed(float speed)
    {
        GameManager.s_dialogueSpeed = speed;
        GameUIHandler.Instance.UpdateDialogueSpeed();
    }
}