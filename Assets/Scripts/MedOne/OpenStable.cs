using UnityEngine;

/// <summary>
/// Orb in the stable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class OpenStable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        if (!MedOneManager.playOpenStableDia && MedOneManager.noHorsesInFrontOfStable
            && MedOneManager.noHorsesInStable)
        {
            MedOneManager.playOpenStableDia = true;
        }
    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (MedOneManager.noHorsesInFrontOfStable && MedOneManager.noHorsesInStable)
        {
            prompt.SetActive(true);
            GameManager.s_curOrbs[1] = true;
        }
        else
        {
            prompt.SetActive(false);
        }
    }
}
