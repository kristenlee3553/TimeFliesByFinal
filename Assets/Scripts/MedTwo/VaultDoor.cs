using UnityEngine;

/// <summary>
/// Reveals treasure room if have key. Otherwise, play locked vault dialogue.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class VaultDoor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject keyPrompt;
    [SerializeField] GameObject noKeyPrompt;

    [SerializeField] private GameObject openDoor;
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject blocker;

    [SerializeField] private DialogueAsset lockedDia;

    public void Interact()
    {
        if (MedTwoManager.s_hasKey)
        {
            RemoveInteractable();
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
            blocker.SetActive(false);
        }
        else
        {
            GameUIHandler.Instance.StartDialogue(lockedDia.dialogue);
        }
    }

    public void RemoveInteractable()
    {
        keyPrompt.SetActive(false);
        noKeyPrompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (MedTwoManager.s_hasKey)
        {
            keyPrompt.SetActive(true);
            noKeyPrompt.SetActive(false);
        }
        else
        {
            keyPrompt.SetActive(false);
            noKeyPrompt.SetActive(true);
        }
    }
}
