using UnityEngine;

/// <summary>
/// Handles the showing and hiding of the treasure room.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class TreasureRoom : MonoBehaviour
{
    [SerializeField] private GameObject blocker;
    private bool reposition = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Show treasure room if not showing
        if (collision.CompareTag("Wizard"))
        {
            reposition = true;

            if (blocker.activeInHierarchy)
            {
                blocker.SetActive(false);
                MedTwoManager.s_inTreasureRoom = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hide Blocker
        if (collision.CompareTag("Wizard"))
        {
            MedTwoManager.s_inTreasureRoom = false;
            reposition = false;
        }
    }

    public void Reposition()
    {
        if (reposition)
        {
            CharacterManager.Instance.SetWizardPosition(2.507f, -4.53f, 0);
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
