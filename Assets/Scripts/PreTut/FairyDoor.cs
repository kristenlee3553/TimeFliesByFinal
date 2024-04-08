using UnityEngine;

/// <summary>
/// Triggers when fairy is faster than the wizard to the door.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class FairyDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fairy"))
        {
            PreTutManager.fairyDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fairy"))
        {
            PreTutManager.fairyDoor = false;
        }
    }
}
