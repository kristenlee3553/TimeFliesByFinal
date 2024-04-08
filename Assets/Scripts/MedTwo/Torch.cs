using UnityEngine;

/// <summary>
/// Makes torch interactable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Torch : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        if (MedTwoManager.s_freezedTorch)
        {
            FreezeManager.Instance.ResetManager();
        }
        MedTwoManager.s_hasTorch = true;
        GameUIHandler.Instance.SetUpInventory("Candle");
        transform.gameObject.SetActive(false);
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
