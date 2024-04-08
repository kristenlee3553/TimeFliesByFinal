using System.Collections;
using UnityEngine;

/// <summary>
/// Makes the wizard do a flip before landing in the cart.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class DivePoint : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        prompt.SetActive(false);

        if (MedOneManager.ableToDive)
        {
            StartCoroutine(StartDive());
        }  
    }

    private IEnumerator StartDive()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();

        CharacterManager.Instance.TurnOffGravity(true);
        CharacterManager.Instance.FlipWizardSprite(false);

        CharacterManager.Instance.ChangeWizardLayer("Default", 0);

        float newX = -3.02f;
        float newY = 4.44f;
        float startAngle = 0f;

        for (int i = 0; i <= 10; i++)
        {
            newX -= 0.083f;
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
            CharacterManager.Instance.FlipWizardSprite(false);
            yield return new WaitForSeconds(.01f);
        }

        for (int i = 1; i <= 100; i++)
        {
            newX += 0.0028f;
            newY -= 0.0661f;
            startAngle += 6.54f;
            CharacterManager.Instance.RotateWizard(startAngle);
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
            CharacterManager.Instance.FlipWizardSprite(false);
            yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(.75f);

        CharacterManager.Instance.SetWizardPosition(-2.01f, -2.67f, 0);
        CharacterManager.Instance.RotateWizard(0f);
        CharacterManager.Instance.TurnOffGravity(false);
        CharacterManager.Instance.DisableAll(false);
        CharacterManager.Instance.ChangeWizardLayer("Wizard", 0);

    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (MedOneManager.ableToDive)
        {
            prompt.SetActive(true);
        }
        else
        {
            prompt.SetActive(false);
        }
    }
}
