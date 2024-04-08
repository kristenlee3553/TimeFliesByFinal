using UnityEngine;

/// <summary>
/// Hides orb display when wizard enters
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Balcony : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameUIHandler.Instance.HideOrbDisplay();
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
            GameUIHandler.Instance.ShowOrbDisplay();
        }
    }
}
