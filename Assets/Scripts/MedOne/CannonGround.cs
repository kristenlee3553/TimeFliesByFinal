using System.Collections;
using UnityEngine;

/// <summary>
/// Fires the wizard from the ground to the top of the tower.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class CannonGround : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject prompt;
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    public void Interact()
    {
        prompt.SetActive(false);

        if (GameManager.s_curPhase == 3)
        {
            StartCoroutine(StartDeathShoot());
        }
        else
        {
            StartCoroutine(StartShoot());
        }
    }

    /// <summary>
    /// Wizard dies to cannon ball
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDeathShoot()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();

        float startAngle = -40.0f;

        CharacterManager.Instance.RotateWizard(startAngle);
        CharacterManager.Instance.TurnOffGravity(true);

        float newX = -1.56f;
        float newY = -2.81f;

        for (int i = 1; i <= 25; i++)
        {
            newX += 0.1044f;
            newY += 0.1188f;
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
            yield return new WaitForSeconds(.01f);
        }

        // Die to cannon ball
        LevelManager.Instance.StartDeathAnimation(false);
    }
    
    /// <summary>
    /// Wizard lives. Ending spot is castle top
    /// </summary>
    /// <returns></returns>
    IEnumerator StartShoot()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();

        float startAngle = -40.0f;

        CharacterManager.Instance.RotateWizard(startAngle);
        CharacterManager.Instance.TurnOffGravity(true);

        float newX = -1.56f;
        float newY = -2.81f;

        for (int i = 1; i <= 100; i ++)
        {
            newX += 0.0568f;
            newY += 0.0635f;
            CharacterManager.Instance.SetWizardPosition(newX, newY, 0);
            yield return new WaitForSeconds(.01f);
        }

        for (int i = 1; i <= 20; i++)
        {
            startAngle += 2f;
            newX += 0.047f;
            newY -= 0.033f;
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
