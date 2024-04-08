using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager for Level Selection Screen
/// Authors: 
/// Cynthia Wang (Display + orbs)
/// Kristen Lee (Loading level code)
/// </summary>
public class LevelSelect : MonoBehaviour
{
    // For hardcoded orbs
    [Header("Orbs")]
    [SerializeField] private GameObject l1EmptyOrb1;
    [SerializeField] private GameObject l1EmptyOrb2;
    [SerializeField] private GameObject l1EmptyOrb3;
    [SerializeField] private GameObject l1Orb1;
    [SerializeField] private GameObject l1Orb2;
    [SerializeField] private GameObject l1Orb3;
    [SerializeField] private GameObject l2EmptyOrb1;
    [SerializeField] private GameObject l2EmptyOrb2;
    [SerializeField] private GameObject l2EmptyOrb3;
    [SerializeField] private GameObject l2Orb1;
    [SerializeField] private GameObject l2Orb2;
    [SerializeField] private GameObject l2Orb3;

    void Start()
    {
        DisplayCollectedOrbs();
    }

    public void DisplayCollectedOrbs()
    {
        // Hardcoded for no save data skull
        if (GameManager.s_dinoOrbs[0] == false)
        {
            l1EmptyOrb1.SetActive(true);
        }
        else
        {
            l1Orb1.SetActive(true);
        }
        if (GameManager.s_dinoOrbs[1] == false)
        {
            l1EmptyOrb2.SetActive(true);
        }
        else
        {
            l1Orb2.SetActive(true);
        }
        if (GameManager.s_dinoOrbs[2] == false)
        {
            l1EmptyOrb3.SetActive(true);
        }
        else
        {
            l1Orb3.SetActive(true);
        }
        if (GameManager.s_medOrbs[0] == false)
        {
            l2EmptyOrb1.SetActive(true);
        }
        else
        {
            l2Orb1.SetActive(true);
        }
        if (GameManager.s_medOrbs[1] == false)
        {
            l2EmptyOrb2.SetActive(true);
        }
        else
        {
            l2Orb2.SetActive(true);
        }
        if (GameManager.s_medOrbs[2] == false)
        {
            l2EmptyOrb3.SetActive(true);
        }
        else
        {
            l2Orb3.SetActive(true);
        }
    }

    // Defines behaviour for tutorial button
    public void PlayTutorial()
    {
        StartCoroutine(SetUpLevel("PreTut"));
    }

    // Defines behaviour for level 1 button
    public void PlayLevel1()
    {
        StartCoroutine(SetUpLevel("Dino"));
    }

    // Defines behaviour for level 2 button
    public void PlayLevel2()
    {
        StartCoroutine(SetUpLevel("MedOne"));
    }

    // Defines behaviour for back button
    public void BackButton()
    {
        GameUIHandler.Instance.LoadUIPage("HomePage");
    }

    IEnumerator SetUpLevel(string level)
    {
        GameManager.s_level = level;

        // Configure scene according to which level it is
        if (level == "PreTut")
        {
            GameManager.s_curScene = "PreTut1";
            GameManager.s_firstPhase = 1;
            GameManager.s_curPhase = 1;
            GameManager.s_wizardResetX = -7.7f;
            GameManager.s_wizardResetY = -3.1f;
            GameManager.s_fairyResetX = -6.08f;
            GameManager.s_fairyResetY = -2.63f;
            CharacterManager.Instance.SetWizardSize(1.15f, 1.08f, 1);
            CharacterManager.Instance.SetFairySize(1.35f, 1.35f, 1);

            TutorialManager.ResetStaticVariable();

            GameUIHandler.Instance.HideOrbDisplay();
            GameUIHandler.Instance.HideTimeBar();
            GameUIHandler.Instance.ShowTopRightMenu();
            AudioManager.Instance.PlayBgMusic(4);
        }
        else if (level == "Dino")
        {
            GameManager.s_curScene = "Dino1";
            GameManager.s_firstPhase = 1;
            GameManager.s_curPhase = 1;
            GameManager.s_wizardResetX = -7.53f;
            GameManager.s_wizardResetY = 1.07f;
            GameManager.s_wizardRespawnX = -7.53f;
            GameManager.s_wizardRespawnY = 1.07f;
            GameManager.s_fairyResetX = -4.29f;
            GameManager.s_fairyResetY = -2.56f;

            CharacterManager.Instance.SetWizardSize(0.54f, 0.48f, 1.0f);
            CharacterManager.Instance.SetFairySize(0.87f, 0.74f, 1.0f);
            AudioManager.Instance.PlayBgMusic(0);
            GameUIHandler.Instance.TurnOnGameUI();
        }
        else if (level == "MedOne")
        {
            GameManager.s_curScene = "MedOne1";
            GameManager.s_firstPhase = 1;
            GameManager.s_curPhase = 1;
            GameManager.s_wizardResetX = -3.78f;
            GameManager.s_wizardResetY = -2.6f;
            GameManager.s_wizardRespawnX = -3.78f;
            GameManager.s_wizardRespawnY = -2.6f;
            GameManager.s_fairyResetX = -2.83f;
            GameManager.s_fairyResetY = -1.22f;

            CharacterManager.Instance.SetWizardSize(0.54f, 0.48f, 1.0f);
            CharacterManager.Instance.SetFairySize(0.87f, 0.74f, 1.0f);
            AudioManager.Instance.PlayBgMusic(3);
            GameUIHandler.Instance.TurnOnGameUI();
        }

        CharacterManager.Instance.SetFairyPosition(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
        CharacterManager.Instance.SetWizardPosition(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);

        LevelManager.Instance.ResetGame();

        SceneManager.UnloadSceneAsync("LevelSelect");

        LevelManager.Instance.LoadNewLevel(level, GameManager.s_firstPhase);

        yield return null;
    }

}
