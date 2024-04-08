using UnityEngine;

/// <summary>
/// Shows hint for the orb
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class LockedStable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        if (!MedOneManager.playLockedStableDia && MedOneManager.noHorsesInStable)
        {
            prompt.SetActive(false);
            MedOneManager.playLockedStableDia = true;
        }
    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (MedOneManager.noHorsesInStable)
        {
            prompt.SetActive(true);
        }
        else
        {
            prompt.SetActive(false);
        }
    }
}
