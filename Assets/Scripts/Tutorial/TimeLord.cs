using UnityEngine;

/// <summary>
/// After 3 attempts to freeze the time lord, a game over screen will play.
/// Author: Kristen Lee
/// </summary>
public class TimeLord : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    private bool ableToFreeze = false;
    private int timesFreezed = 0;

    [SerializeField] private DialogueAsset warning;
    [SerializeField] private DialogueAsset killDia;

    private void Update()
    {
        if (TutorialManager.s_firstPower && ableToFreeze)
        {
            if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.Preserve])
                && !CharacterManager.Instance.IsPowerDisabled())
            {
                timesFreezed++;
                prompt.SetActive(false);

                // Game over -> kill fairy and wizard
                if (timesFreezed == 3)
                {
                    TutorialManager.showGameOver = true;
                    GameUIHandler.Instance.StartDialogue(killDia.dialogue);
                }
                else
                {
                    GameUIHandler.Instance.StartDialogue(warning.dialogue);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fairy") && TutorialManager.s_firstPower
            && !CharacterManager.Instance.IsPowerDisabled())
        {
            prompt.SetActive(true);
            ableToFreeze = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fairy"))
        {
            prompt.SetActive(false);
            ableToFreeze = false;
        }   
    }

    private void ResetTimes()
    {
        timesFreezed = 0;
        ableToFreeze = false;
    }

    private void OnEnable()
    {
        LevelManager.OnResetEvent += ResetTimes;
    }
    private void OnDisable()
    {
        LevelManager.OnResetEvent -= ResetTimes;
    }
}
