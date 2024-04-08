using UnityEngine;

/// <summary>
/// Script that handles buttons of home screen
/// <br></br>
/// Author: Cynthia Wang
/// </summary>
public class MainMenu : MonoBehaviour
{
    // Defines behaviour for PlayButton
    public void PlayGame()
    {
        GameUIHandler.Instance.LoadUIPage("LevelSelect");
    }

    // Defines behaviour for SettingsButton
    public void LoadSettings()
    {
        GameUIHandler.Instance.goToHomePage = true;
        GameUIHandler.Instance.LoadUIPage("Settings");
    }

    // Defines behaviour for CreditsButton
    public void LoadCredits()
    {
        GameUIHandler.Instance.LoadUIPage("Credits");
    }

    // Defines behaviour for ExitButton
    public void QuitGame()
    {
        if (!GameManager.s_websiteBuild)
        {
            Debug.Log("Quit!");
            // Doesn't do anything in Unity, only works in actual games
            Application.Quit();
        }
    }
}