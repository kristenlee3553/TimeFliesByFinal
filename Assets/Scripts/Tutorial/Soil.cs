using UnityEngine;

/// <summary>
/// Allows seed to be plantable
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Soil : MonoBehaviour, IInteractable
{
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject prompt;

    public void Interact()
    {
        // If have the seed and has not planted
        if (TutorialManager.s_hasSeed && !TutorialManager.s_plantedSeed)
        {
            TutorialManager.s_plantedSeed = true;
            spriteRenderer.color = startColor;
            prompt.SetActive(false);
            GameUIHandler.Instance.ShowTimeBar();
        }
    }

    public void RemoveInteractable()
    {
        if (TutorialManager.s_hasSeed && !TutorialManager.s_plantedSeed)
        {
            spriteRenderer.color = startColor;
            prompt.SetActive(false);
        }
    }

    public void ShowInteractable()
    {
        if (TutorialManager.s_hasSeed && !TutorialManager.s_plantedSeed)
        {
            spriteRenderer.color = GameManager.s_interactColor;
            prompt.SetActive(true);
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }
}
