using UnityEngine;

/// <summary>
/// Script that relocates wizard if changing scene causes wizard to collide into dino 2
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Dino2Reposition : MonoBehaviour
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

    private void Reposition()
    {
        if (reposition)
        {
            Vector2 curPos = CharacterManager.Instance.GetWizardPosition();
            float newX = curPos.x;
            float newY = curPos.y;

            if (curPos.x < 0.77f)
            {
                newX = -2.84f;
                newY = -3.54f;
            }
            else if (curPos.x >= 0.77f)
            {
                newX = 5.31f;
                newY = -3.48f;
            }
            else if (curPos.y >= -1.23)
            {
                newX = 0.52f;
                newY = -0.75f;
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
