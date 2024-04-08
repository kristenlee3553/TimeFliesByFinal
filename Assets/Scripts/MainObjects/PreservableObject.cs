using UnityEngine;

/// <summary>
/// Place this script on objects that can be preserved
/// <br></br>
/// Objects must have collider
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class PreservableObject : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    public Color highlightColor;

    private SpriteRenderer sprite;

    private Color startColor;

    private bool isWizard;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fairy") && !FreezeManager.Instance.CanPreserve()
            && !FreezeManager.Instance.IsPreserving() && !CharacterManager.Instance.IsPowerDisabled())
        {
            // Set object fairy can preserve
            FreezeManager.Instance.SetPreservableObject(this.gameObject);

            ShowPrompt(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Sometimes the fairy is knocked away from the wizard if wizard
        // collides on scene change which results in staying yellow
        if (collision.CompareTag("Fairy")
            && !FreezeManager.Instance.IsPreserving() && !CharacterManager.Instance.IsPowerDisabled())
        {
            // No longer able to preserve an object
            FreezeManager.Instance.SetPreservableObject(null);

            ShowPrompt(false);
        }
    }

    public void ShowPrompt(bool show)
    {
        if (!isWizard)
        {
            prompt.SetActive(show);
        }
        sprite.color = show ? highlightColor : startColor;
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
        isWizard = CompareTag("Wizard");
    }
}