using UnityEngine;
using System.Collections;

/// <summary>
/// Shows dialogue and burning animation when burnt
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Portrait : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject portrait;
    [SerializeField] private GameObject burningPortrait;

    [SerializeField]
    private GameObject hasTorchPrompt;

    [SerializeField] private GameObject noTorchPrompt;

    [SerializeField] private DialogueAsset keyDia;
    [SerializeField] private DialogueAsset noKeyDia;
    [SerializeField] private DialogueAsset torchHintDia;
 
    private SpriteRenderer sprite;
    private Color burnColor;

    private bool burned = false;

    private void Start()
    {
        sprite = burningPortrait.GetComponent<SpriteRenderer>();
        burnColor = sprite.color;
    }

    public void Interact()
    {
        RemoveInteractable();
        if (MedTwoManager.s_hasTorch && !burned)
        {
            burned = true;
            StartCoroutine(StartBurnAnimation());
        }
        else
        {
            GameUIHandler.Instance.StartDialogue(torchHintDia.dialogue);
        }
    }

    private IEnumerator StartBurnAnimation()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        portrait.SetActive(false);
        burningPortrait.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            burnColor.a = alpha;
            sprite.color = burnColor;
            yield return new WaitForSeconds(.1f);
        }

        burningPortrait.SetActive(false);

        if (GameManager.s_curPhase == 2)
        {
            GameUIHandler.Instance.SetUpInventory("Key");
            MedTwoManager.s_hasKey = true;
            GameUIHandler.Instance.StartDialogue(keyDia.dialogue);
        }
        else
        {
            GameUIHandler.Instance.StartDialogue(noKeyDia.dialogue);
        }
    }

    public void RemoveInteractable()
    {
        hasTorchPrompt.SetActive(false);
        noTorchPrompt.SetActive(false);
    }

    public void ShowInteractable()
    {
        if (!burned)
        {
            if (MedTwoManager.s_hasTorch)
            {
                hasTorchPrompt.SetActive(true);
                noTorchPrompt.SetActive(false);
            }
            else
            {
                hasTorchPrompt.SetActive(false);
                noTorchPrompt.SetActive(true);
            }
        }
    }

    private void ShowPortrait()
    {
        portrait.SetActive(true);
    }

    private void HidePortrait()
    {
        portrait.SetActive(false);
    }

    private void OnEnable()
    {
        MedTwoManager.ShowPortrait += ShowPortrait;
        MedTwoManager.HidePortrait += HidePortrait;
    }

    private void OnDisable()
    {
        MedTwoManager.ShowPortrait -= ShowPortrait;
        MedTwoManager.HidePortrait -= HidePortrait;
    }
}
