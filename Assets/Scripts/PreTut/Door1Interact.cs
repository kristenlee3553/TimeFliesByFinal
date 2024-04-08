using UnityEngine;

/// <summary>
/// On interact, loads the second part of the pretutorial.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Door1Interact : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        GameManager.s_firstPhase = 2;
        GameManager.s_curPhase = 2;
        GameManager.s_wizardResetX = -7.59f;
        GameManager.s_wizardResetY = -1.91f;
        GameManager.s_fairyResetX = -6.2034f;
        GameManager.s_fairyResetY = -1.0967f;
        CharacterManager.Instance.SetFairyPosition(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
        CharacterManager.Instance.SetWizardPosition(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);

        LevelManager.Instance.ChangePhase(2, true);
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
