using UnityEngine;

/// <summary>
/// Kills wizard on tocuh
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Alligator : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            LevelManager.Instance.StartDeathAnimation(false);
        }
    }
}
