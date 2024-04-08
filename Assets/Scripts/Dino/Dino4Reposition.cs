using UnityEngine;

/// <summary>
/// Repositions if colliding with dino 4.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Dino4Reposition : MonoBehaviour
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
            float newX = curPos.x;
            float newY = curPos.y;

            if (newX < 0.77f && newY <= 1.65f)
            {
                newX = -2.83f;
                newY = -3.51f;
            }
            else if (newX >= 0.77f && newY <= 1.65f)
            {
                newX = 4.96f;
                newY = -3.52f;
            }
            else if (newX >= -0.49f)
            {
                newX = -1.79f;
                newY = 0.84f;
            }
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
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
