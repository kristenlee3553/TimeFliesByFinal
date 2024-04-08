using UnityEngine;

/// <summary>
/// Makes seed interactable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Seed : MonoBehaviour, IInteractable
{
    private Color startColor;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject prompt;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    public void Interact()
    {
        gameObject.SetActive(false);
        TutorialManager.s_hasSeed = true;
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
