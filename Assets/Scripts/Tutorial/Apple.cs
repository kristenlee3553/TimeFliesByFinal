using UnityEngine;

/// <summary>
/// Makes apple interactable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Apple : MonoBehaviour, IInteractable
{
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    private bool ripe = false;
    [SerializeField] private GameObject prompt;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        string name = gameObject.name;
        if (name == "RipeApple")
        {
            ripe = true;
        }
    }

    public void Interact()
    {
        if (ripe)
        {
            TutorialManager.s_hasRipeApple = true;
        }
        else
        {
            TutorialManager.s_hasUnripeApple = true;
        }

        // Reset variables
        FreezeManager.Instance.ResetManager();

        prompt.SetActive(false);
        gameObject.SetActive(false);
    }

    public void RemoveInteractable()
    {
        spriteRenderer.color = startColor;
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        spriteRenderer.color = GameManager.s_interactColor;
        prompt.SetActive(true);
    }
}
