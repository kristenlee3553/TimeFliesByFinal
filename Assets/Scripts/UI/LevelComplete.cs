using System.Collections;
using UnityEngine;

/// <summary>
/// Shows level complete screen with respective orbs
/// <br></br>
/// Author: Cynthia Wang
/// </summary>
public class LevelComplete : MonoBehaviour
{
    [Header("Frames")]
    [SerializeField] private GameObject levelComplete1;
    [SerializeField] private GameObject levelComplete2;
    [SerializeField] private GameObject levelComplete3;
    [SerializeField] private GameObject levelCompleteTut;
    [SerializeField] private GameObject levelCompleteLevel;

    [Header("Orbs")]
    [SerializeField] private GameObject orb1Empty;
    [SerializeField] private GameObject orb1Collected;
    [SerializeField] private GameObject orb2Empty;
    [SerializeField] private GameObject orb2Collected;
    [SerializeField] private GameObject orb3Empty;
    [SerializeField] private GameObject orb3Collected;
    [SerializeField] private GameObject nextButtonPink;

    void Awake()
    {
        StartCoroutine(PlayLevelCompleteAnimation());
    }

    IEnumerator PlayLevelCompleteAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            levelComplete1.SetActive(true);
            yield return new WaitForSeconds(.1f);
            levelComplete1.SetActive(false);
            levelComplete2.SetActive(true);
            yield return new WaitForSeconds(.1f);
            levelComplete2.SetActive(false);
            levelComplete3.SetActive(true);
            yield return new WaitForSeconds(.1f);
            levelComplete3.SetActive(false);
        }
        if (GameManager.s_level == "Tut")
        {
            levelCompleteTut.SetActive(true);
        }
        else
        {
            levelCompleteLevel.SetActive(true);
        }
        StartCoroutine(DisplayOrb());
    }

    IEnumerator DisplayOrb()
    {
        if (GameManager.s_curOrbs[0] == false)
        {
            orb1Empty.SetActive(true);
        }
        else
        {
            orb1Collected.SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        if (GameManager.s_curOrbs[1] == false)
        {
            orb2Empty.SetActive(true);
        }
        else
        {
            orb2Collected.SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        if (GameManager.s_curOrbs[2] == false)
        {
            orb3Empty.SetActive(true);
        }
        else
        {
            orb3Collected.SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        nextButtonPink.SetActive(true);
    }

    public void ToLevelSelect()
    {
        GameUIHandler.Instance.LoadUIPage("LevelSelect");
    }
}
