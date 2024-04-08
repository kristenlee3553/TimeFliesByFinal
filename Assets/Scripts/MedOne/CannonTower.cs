using System.Collections;
using UnityEngine;

/// <summary>
/// Shoots the wizard from the tower to the platform.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class CannonTower : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    public void Interact()
    {
        prompt.SetActive(false);
        StartCoroutine(StartShoot());
    }

    private IEnumerator StartShoot()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        float startAngle = 65.0f;

        CharacterManager.Instance.RotateWizard(startAngle);
        CharacterManager.Instance.TurnOffGravity(true);
        CharacterManager.Instance.FlipWizardSprite(false);

        float newX = 5f;
        float newY = 2.8f;

        for (int i = 1; i <= 50; i++)
        {
            newX -= 0.1078f;
            newY += 0.037f;
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
            CharacterManager.Instance.FlipWizardSprite(false);
            yield return new WaitForSeconds(.01f);
        }

        for (int i = 1; i <= 20; i++)
        {
            startAngle -= 3.5f;
            newX -= 0.0855f;
            newY -= 0.0105f;
            CharacterManager.Instance.FlipWizardSprite(false);
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
            CharacterManager.Instance.RotateWizard(startAngle);
            yield return new WaitForSeconds(.01f);
        }

        CharacterManager.Instance.TurnOffGravity(false);
        CharacterManager.Instance.DisableAll(false);

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
