using UnityEngine;

/// <summary>
/// Script that shows the interior of the cave when wizard enters collison zone
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class CaveEntrance : MonoBehaviour
{
    /// <summary>
    /// Put nest here
    /// </summary>
    [SerializeField] private GameObject nest;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Show nest if not showing
        if (collision.CompareTag("Wizard") && !nest.activeInHierarchy)
        {
            nest.SetActive(true);
            DinoManager.s_inCave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hide nest
        if (collision.CompareTag("Wizard") && nest.activeInHierarchy)
        {
            nest.SetActive(false);
            DinoManager.s_inCave = false;
        }
    }

}
