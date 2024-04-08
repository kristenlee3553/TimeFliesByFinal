using UnityEngine;

/// <summary>
/// Handles the hiding and showing of the secret tunnel.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Tunnel : MonoBehaviour
{
    [SerializeField] private GameObject tunnel;
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Show tunnel if not showing
        if (collision.CompareTag("Wizard") && !tunnel.activeInHierarchy)
        {
            MedTwoManager.s_showTunnel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hide tunnel if not showing
        if (collision.CompareTag("Wizard") && tunnel.activeInHierarchy && !MedTwoManager.s_climbing)
        {
            MedTwoManager.s_hideTunnel = true;
        }
    }
}
