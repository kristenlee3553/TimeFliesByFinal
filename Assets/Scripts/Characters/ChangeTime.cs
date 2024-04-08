using UnityEngine;

/// <summary>
/// Script that controls when fairy wants to move time back and forth
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class ChangeTime : MonoBehaviour
{
    private FairyMovement fairyMove;
    private Animator animator;

    private void Start()
    {
        fairyMove = GetComponent<FairyMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool powerDisabled = fairyMove.IsPowerDisabled();

        // Wacky code in the second half.
        // There was a bug that left me scratching my head for a few hours
        // This was the solution and I'm too tired to find a more elegant way
        if (!powerDisabled && GameManager.s_level != "Tut" || TutorialManager.s_firstPower && !powerDisabled)
        {
            // Go back in time
            if (Input.GetKeyUp(KeyCode.Q))
            {
                // If not at first phase
                if (!MinPhase())
                {

                    if (!FreezeManager.Instance.IsPreserving())
                    {
                        animator.SetTrigger("Power");
                        AudioManager.Instance.PlayChangeTimeSound();
                    }

                    // Change scene
                    LevelManager.Instance.ChangePhase(GameManager.s_curPhase - 1);
                }

            }

            // Go Forward in Time
            if (Input.GetKeyUp(KeyCode.E))
            {
                // If not at last phase
                if (!MaxPhase())
                {
                    LevelManager.Instance.ChangePhase(GameManager.s_curPhase + 1);

                    if (!FreezeManager.Instance.IsPreserving())
                    {
                        animator.SetTrigger("Power");
                        AudioManager.Instance.PlayChangeTimeSound();
                    }
                }
            }
        }

    }

    /// <summary>
    /// Returns true if on the last phase
    /// </summary>
    /// <returns></returns>
    private bool MaxPhase()
    {
        return GameManager.s_curPhase == 5;
    }

    /// <summary>
    ///  Returns true if on the first phase
    /// </summary>
    /// <returns></returns>
    private bool MinPhase()
    {
        return GameManager.s_curPhase == 1;
    }
}
