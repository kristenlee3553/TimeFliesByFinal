using UnityEngine;

/// <summary>
/// Repositions wizard if going to be in tower.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class TowerReposition : MonoBehaviour
{
    private bool reposition = false;

    public void Reposition()
    {
        if (reposition && GameManager.s_curPhase == 4)
        {
            CharacterManager.Instance.SetWizardPosition(5.08f, 2.92f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard") && GameManager.s_curPhase == 5)
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
