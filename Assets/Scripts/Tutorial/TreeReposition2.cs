using UnityEngine;

/// <summary>
/// Repositions wizard if collides with tree.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class TreeReposition2 : MonoBehaviour
{
    private bool reposition = false;

    public void Reposition()
    {
        if (reposition)
        {
            Vector2 curPos = CharacterManager.Instance.GetWizardPosition();
            float newX = curPos.x < 0? -1.79f : 1.777f;
            float newY = curPos.x < 0? -3.3f : -3.319f;
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
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
