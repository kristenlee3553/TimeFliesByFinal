using System.Collections;
using UnityEngine;

/// <summary>
/// Plays ladder animation on interact and moves wizard up or down the ladder.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Ladder : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject promptTop;
    [SerializeField] GameObject promptBottom;

    [SerializeField] private Transform ladderTop;
    [SerializeField] private Transform ladderBottom;

    [SerializeField] private GameObject animHolder;
    [SerializeField] private SpriteRenderer ladderEmpty;

    private Transform[] animFrames;

    private bool climbing = false;

    private void Start()
    {
        int children = animHolder.transform.childCount;
        animFrames = new Transform[children];

        for (int i = 0; i < children; ++i)
        {
            animFrames[i] = animHolder.transform.GetChild(i);
            animFrames[i].gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (GameManager.s_curPhase <= 3)
        {
            MedTwoManager.s_climbing = true;
            climbing = true;
            RemoveInteractable();
            StartCoroutine(StartLadderClimb());
        }
    }

    private IEnumerator StartLadderClimb()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        CharacterManager.Instance.TurnOffGravity(true);
        CharacterManager.Instance.SetWizardColor(Color.clear);

        bool topClosest = IsTopDistClosest();

        float y = topClosest ? ladderBottom.position.y : ladderTop.position.y;
        float x = topClosest ? ladderBottom.position.x : ladderTop.position.x;
        CharacterManager.Instance.SetWizardPosition(x, y, 0);

        ladderEmpty.enabled = false;
        animHolder.SetActive(true);

        if (topClosest)
        {
            for (int i = 7; i >= 0; i--)
            {
                animFrames[i].gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(.25f);
                animFrames[i].gameObject.SetActive(false);
            }

        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                animFrames[i].gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(.25f);
                animFrames[i].gameObject.SetActive(false);
            }
        }

        animHolder.SetActive(false);
        ladderEmpty.enabled = true;
        
        MedTwoManager.s_showTunnel = true;

        CharacterManager.Instance.SetWizardColor(Color.white);
        CharacterManager.Instance.TurnOffGravity(false);
        CharacterManager.Instance.DisableAll(false);

        MedTwoManager.s_climbing = false;
        climbing = false;
    }
    public void RemoveInteractable()
    {
        promptTop.SetActive(false);
        promptBottom.SetActive(false);
    }

    private bool IsTopDistClosest()
    {
        Vector3 pos = CharacterManager.Instance.GetWizardPosition();

        float distTop = Vector3.Distance(pos, ladderTop.position);
        float distBot = Vector3.Distance(pos, ladderBottom.position);

        return distTop < distBot;
    }

    public void ShowInteractable()
    {
        if (!climbing && GameManager.s_curPhase <= 3)
        {
            if (IsTopDistClosest())
            {
                promptTop.SetActive(true);
                promptBottom.SetActive(false);
            }
            else
            {
                promptTop.SetActive(false);
                promptBottom.SetActive(true);
            }
        }
        else
        {
            RemoveInteractable();
        }
    }
}
