using UnityEngine;

/// <summary>
/// If gate is open, loads part2. Otherwise, shows locked gate dialogue;
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject openPrompt;

    [SerializeField]
    private GameObject closedPrompt;

    [SerializeField] private DialogueAsset gateDia;

    public bool open;

    public void Interact()
    {
        if (open)
        {
            FreezeManager.Instance.ResetManager();
            openPrompt.SetActive(false);
            int curPhase = GameManager.s_curPhase;
            GameManager.s_firstPhase = curPhase;
            GameManager.s_wizardResetX = 3.54f;
            GameManager.s_wizardResetY = -3.1f;
            GameManager.s_fairyResetX = 4.44f;
            GameManager.s_fairyResetY = -1.7f;
            GameUIHandler.Instance.SetPhaseBar(curPhase);
            MedTwoManager.copyOrbs = GameManager.s_curOrbs;
            CharacterManager.Instance.SetFairyPosition(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);
            CharacterManager.Instance.SetWizardPosition(GameManager.s_wizardResetX, GameManager.s_wizardResetY, 0);

            LevelManager.Instance.LoadNewLevel("MedTwo", curPhase, "MedOne", curPhase);
        }
        else
        {
            closedPrompt.SetActive(false);
            GameUIHandler.Instance.StartDialogue(gateDia.dialogue);
        }
    }

    public void RemoveInteractable()
    {
        openPrompt.SetActive(false);
        closedPrompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (open)
        {
            openPrompt.SetActive(true);
        }
        else
        {
            closedPrompt.SetActive(true);
        }
    }
}
