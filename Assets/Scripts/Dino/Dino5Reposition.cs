using UnityEngine;

/// <summary>
/// Repositions wizard if collides with dino 5
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Dino5Reposition : MonoBehaviour
{
    private bool objFrozen = false;
    private bool reposition = false;

    private void Update()
    {
        // If fairy is preserving
        if (FreezeManager.Instance.IsPreserving() && !FreezeManager.Instance.IsPreservingWizard())
        {
            // Check if object being preserved is the one we are currently on
            if (FreezeManager.Instance.GetPreservedObject().CompareTag("Dino"))
            {
                objFrozen = true;
            }
        }
        else
        {
            objFrozen = false;
        }
    }

    public void Reposition()
    {
        if (reposition)
        {
            Vector2 curPos = CharacterManager.Instance.GetWizardPosition();

            if (curPos.x > 1.7f)
            {
                CharacterManager.Instance.SetWizardPosition(1.7f, 0.89f, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && !objFrozen)
        {
            reposition = true;
        }

        // No need to reposition cuz object is being preserved
        if (collision.gameObject.CompareTag("Wizard") && objFrozen)
        {
            reposition = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
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
