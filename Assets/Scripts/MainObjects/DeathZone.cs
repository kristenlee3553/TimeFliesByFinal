using UnityEngine;

/// <summary>
/// Attach to Game Objects. If wizard is touching this object after time changes -> death
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class DeathZone : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Wizard collision + scene is in middle of changing
        if (GameManager.s_sceneChange && collision.gameObject.CompareTag("Wizard"))
        {   
            // Fairy is preserving an object that is not the wizard
            if (FreezeManager.Instance.IsPreserving() && !FreezeManager.Instance.IsPreservingWizard())
            {
                // If object fairy is preserving is not the object the wizard is standing on, wizard will die
                if (!ReferenceEquals(transform.gameObject, FreezeManager.Instance.GetPreservedObject()))
                {
                    GameManager.s_onDeathObject = true;
                }
                else
                {
                    // Wizard will not die because fairy is preserving object
                    GameManager.s_onDeathObject = false;
                }
            }
            // Wizard will not die if being preserved by fairy
            else if (FreezeManager.Instance.IsPreservingWizard())
            {
                GameManager.s_onDeathObject = false;
            }

            // If on death object
            else
            {
                GameManager.s_onDeathObject = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard"))
        {
            GameManager.s_onDeathObject = false;
        }
    }
}
