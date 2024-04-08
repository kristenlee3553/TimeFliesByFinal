using UnityEngine;

/// <summary>
/// Relocates the wizard for phase 4 throne. Probably could combine with throne1 relocate
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class ThroneRelocate : MonoBehaviour
{
    private bool reposition = false;

    public void Reposition()
    {
        if (reposition)
        {
            CharacterManager.Instance.SetWizardPosition(3.84f, -3.1f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            reposition = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && GameManager.wizardCollisions != 1)
        {
            reposition = false;
        }
    }

    private void OnEnable()
    {
        LevelManager.RelocateWizard += Reposition;
    }

    private void OnDisable()
    {
        LevelManager.RelocateWizard -= Reposition;
    }
}
