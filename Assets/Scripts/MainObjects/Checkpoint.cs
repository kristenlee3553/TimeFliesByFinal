using UnityEngine;

/// <summary>
/// Sets checkpoint when wizard enters. Sets hint if applicable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// Empty Game Object that is the location where wizard will respawn
    /// </summary>
    [SerializeField] private Transform checkpoint;

    /// <summary>
    /// Sets hint when user passes checkpoint. Leave empty if no hint change.
    /// </summary>
    [SerializeField] private string hint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Set checkpoint for wizard
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_wizardRespawnX = checkpoint.position.x;
            GameManager.s_wizardRespawnY = checkpoint.position.y;
            GameManager.s_checkpointPhase = GameManager.s_curPhase;

            if (!hint.Equals(""))
            {
                GameUIHandler.Instance.SetHintText(hint);
            }
        }
    }
}
