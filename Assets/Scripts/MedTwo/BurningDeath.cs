using UnityEngine;
/// <summary>
/// Kills wizard and changes them to red
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class BurningDeath : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            LevelManager.Instance.StartDeathAnimation(false, Color.red);
        }
    }
}
