using UnityEngine;

/// <summary>
/// Kills wizard cuz wizard can't swim.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            LevelManager.Instance.StartDeathAnimation(false);
        }
    }
}
