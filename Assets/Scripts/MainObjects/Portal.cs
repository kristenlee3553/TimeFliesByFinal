using UnityEngine;

/// <summary>
/// WHERE USER COMPLETES THE LEVEL
/// <br></br>
/// Authors:
/// <br></br>
/// Cynthia Wang: saving of orbs
/// Kristen Lee: Everything else
/// </summary>
public class Portal : MonoBehaviour, IInteractable
{
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject prompt;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    public void Interact()
    {
        string level = GameManager.s_level;

        for (int index = 0; index < 3; index++)
        {
            // Update level specific orb data
            if (level == "Dino")
            {
                if (!GameManager.s_dinoOrbs[index])
                {
                    GameManager.s_dinoOrbs[index] = GameManager.s_curOrbs[index];
                }
            }
            else if (level == "MedOne" || level == "MedTwo")
            {
                if (!GameManager.s_medOrbs[index])
                {
                    GameManager.s_medOrbs[index] = GameManager.s_curOrbs[index];
                }
            }
        }
        GameUIHandler.Instance.LoadUIPage("LevelComplete");
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
