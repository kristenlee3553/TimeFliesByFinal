using UnityEngine;

/// <summary>
/// On interact, loads tutorial level
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Door2Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        GameManager.s_firstPhase = 1;
        GameManager.s_wizardResetX = -7.56f;
        GameManager.s_wizardResetY = -3.19f;
        GameManager.s_fairyResetX = -5.86f;
        GameManager.s_fairyResetY = -2.11f;
        GameUIHandler.Instance.SetPhaseBar(1);
        CharacterManager.Instance.SetWizardSize(0.54f, 0.48f, 1.0f);
        CharacterManager.Instance.SetFairySize(0.87f, 0.74f, 1.0f);
        CharacterManager.Instance.SetFairyPosition(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
        CharacterManager.Instance.SetWizardPosition(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);
        AudioManager.Instance.PlayBgMusic(5);

        LevelManager.Instance.LoadNewLevel("Tut", 1, "PreTut", 2);
    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        prompt.SetActive(true);
    }
}
