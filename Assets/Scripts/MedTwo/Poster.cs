using UnityEngine;

/// <summary>
/// Plays poster dialogue on interaction
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Poster : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject prompt;

    [SerializeField] private DialogueAsset dialogue;

    public void Interact()
    {
        RemoveInteractable();
        GameUIHandler.Instance.StartDialogue(dialogue.dialogue);
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
