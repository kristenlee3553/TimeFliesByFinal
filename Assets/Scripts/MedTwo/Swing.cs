using System.Collections;
using UnityEngine;

/// <summary>
/// Shows wizard swinging on the chandelier. Probably could've written more reusable code.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Swing : MonoBehaviour,  IInteractable
{
    [SerializeField] private Sprite wizardChand;
    [SerializeField] private GameObject chandelier;

    [SerializeField]
    private GameObject prompt;

    private SpriteRenderer sprite;
    private Sprite startingSprite;
    private bool startLeft = true;

    private void Start()
    {
        sprite = chandelier.GetComponent<SpriteRenderer>();
        startingSprite = sprite.sprite;
    }

    public void Interact()
    {
        // Unfreeze fairy if freezing chandelier
        if (MedTwoManager.s_chandelierFreezed)
        {
            FreezeManager.Instance.ResetManager();
        }

        RemoveInteractable();
        int phase = GameManager.s_curPhase;
        GameManager.s_checkpointPhase = phase;
        if (phase != 5)
        {
            StartCoroutine(SwingWizardDeath());
        }
        else if (startLeft)
        {
            StartCoroutine(SwingWizardRight());
        }
        else
        {
            StartCoroutine(SwingWizardLeft());
        }
        
    }

    private IEnumerator SwingWizardDeath()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        CharacterManager.Instance.TurnOffGravity(true);
        CharacterManager.Instance.SetWizardColor(Color.clear);
        sprite.sprite = wizardChand;

        float leftDeg = -20f;
        float rightDeg = 26f;
        float startDeg = leftDeg;

        while (startDeg <= rightDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg += 1;
        }

        // Go back
        while (startDeg >= leftDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg -= 1;
        }

        while (startDeg <= rightDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg += 1;
        }

        CharacterManager.Instance.TriggerWizardFlyAnimation();
        float x = 0.61f;
        float y = 1.92f;

        sprite.sprite = startingSprite;
        CharacterManager.Instance.SetWizardColor(Color.white);
        CharacterManager.Instance.SetWizardPosition(x, y, 0);

        for (int i = 0; i < 50; i++)
        {
            x += 0.0598f;
            y += 0.0498f;
            yield return new WaitForSeconds(.01f);
            CharacterManager.Instance.SetWizardPosition(x, y, 0);
        }

        CharacterManager.Instance.TriggerWizardFallAnimation();

        for (int i = 0; i < 7; i++)
        {
            x += 0.058f;
            y -= 0.0196f;
            yield return new WaitForSeconds(.01f);
            CharacterManager.Instance.SetWizardPosition(x, y, 0);
        }

        LevelManager.Instance.StartDeathAnimation(false);
    }

    private IEnumerator SwingWizardRight()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        CharacterManager.Instance.TurnOffGravity(true);
        CharacterManager.Instance.SetWizardColor(Color.clear);
        sprite.sprite = wizardChand;

        float leftDeg = -20f;
        float rightDeg = 26f;
        float startDeg = leftDeg;

        while (startDeg <= rightDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg += 1;
        }

        // Go back
        while (startDeg >= leftDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg -= 1;
        }

        while (startDeg <= rightDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg += 1;
        }

        CharacterManager.Instance.TriggerWizardFlyAnimation();
        float x = 0.61f;
        float y = 1.92f;

        sprite.sprite = startingSprite;
        CharacterManager.Instance.SetWizardColor(Color.white);
        CharacterManager.Instance.SetWizardPosition(x, y, 0);

        for (int i = 0; i < 50; i++)
        {
            x += 0.0598f;
            y += 0.0498f;
            yield return new WaitForSeconds(.01f);
            CharacterManager.Instance.SetWizardPosition(x, y, 0);
        }

        CharacterManager.Instance.TriggerWizardFallAnimation();

        for (int i = 0; i < 25; i++)
        {
            x += 0.058f;
            y -= 0.0196f;
            yield return new WaitForSeconds(.01f);
            CharacterManager.Instance.SetWizardPosition(x, y, 0);
        }

        CharacterManager.Instance.ResetWizardAnimation();
        CharacterManager.Instance.DisableAll(false);
        CharacterManager.Instance.TurnOffGravity(false);

        startLeft = false;
    }

    private IEnumerator SwingWizardLeft()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        CharacterManager.Instance.TurnOffGravity(true);
        CharacterManager.Instance.SetWizardColor(Color.clear);
        sprite.sprite = wizardChand;

        float leftDeg = -20f;
        float rightDeg = 26f;
        float startDeg = rightDeg;

        // Swing right to left
        while (startDeg >= leftDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg -= 1;
        }

        while (startDeg <= rightDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg += 1;
        }

        // Go back
        while (startDeg >= leftDeg)
        {
            chandelier.transform.rotation = Quaternion.Euler(Vector3.forward * startDeg);
            yield return new WaitForSeconds(.01f);
            startDeg -= 1;
        }

        CharacterManager.Instance.TriggerWizardFlyAnimation();
        float x = -0.89f;
        float y = 1.86f;

        sprite.sprite = startingSprite;
        CharacterManager.Instance.SetWizardColor(Color.white);
        CharacterManager.Instance.SetWizardPosition(x, y, 0);

        for (int i = 0; i < 50; i++)
        {
            CharacterManager.Instance.FlipWizardSprite(false);
            x -= 0.0462f;
            y += 0.0594f;
            yield return new WaitForSeconds(.01f);
            CharacterManager.Instance.SetWizardPosition(x, y, 0);
        }

        CharacterManager.Instance.TriggerWizardFallAnimation();

        for (int i = 0; i < 25; i++)
        {
            CharacterManager.Instance.FlipWizardSprite(false);
            x -= 0.0572f;
            y -= 0.0332f;
            yield return new WaitForSeconds(.01f);
            CharacterManager.Instance.SetWizardPosition(x, y, 0);
        }

        CharacterManager.Instance.ResetWizardAnimation();
        CharacterManager.Instance.DisableAll(false);
        CharacterManager.Instance.TurnOffGravity(false);

        startLeft = true;
    }

    public void RemoveInteractable()
    {
        prompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        prompt.SetActive(true);
    }
}
