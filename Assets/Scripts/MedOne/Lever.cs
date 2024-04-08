using UnityEngine;

/// <summary>
/// Makes lever interactable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{

    [SerializeField]
    private GameObject prompt;
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    public void Interact()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        MedOneManager.leverFlipped = true;
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

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }
}
