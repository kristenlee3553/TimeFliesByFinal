using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Manager for level 2 part 2
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class MedTwoManager : MonoBehaviour
{
    [SerializeField] private GameObject[] curtains;
    [SerializeField] private GameObject[] leftBalcony;
    [SerializeField] private GameObject[] rightBalcony;
    [SerializeField] private GameObject[] leftBanners;
    [SerializeField] private GameObject[] rightBanners;
    [SerializeField] private GameObject[] portalBlocker;
    [SerializeField] private GameObject tunnel;
    [SerializeField] private GameObject treasureHider;
    [SerializeField] private GameObject lockedDoor;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private GameObject portraitAnim;
    [SerializeField] private GameObject orb3;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject camera;

    [SerializeField] private DialogueAsset[] dialogueAsset;

    // Inefficient code but time limit and whatever works, works
    public static Action ShowPortrait;
    public static Action HidePortrait;

    public static bool s_inTreasureRoom = false;
    public static bool s_showTunnel = false;
    public static bool s_hideTunnel = false;
    public static bool s_climbing = false;
    public static bool s_hasTorch = false;
    public static bool s_hasKey = false;
    public static bool s_freezedTorch = false;
    public static bool s_chandelierFreezed = false;

    private bool startedPortraitDia = false;
    private bool showPortraitAnim = false;
    private int lastPhase = 5;

    public static bool[] copyOrbs;

    // Update is called once per frame
    void Update()
    {
        if (s_showTunnel)
        {
            s_showTunnel = false;
            SetUpTunnel(true);
        }
        if (s_hideTunnel)
        {
            s_hideTunnel = false;
            SetUpTunnel(false);
        }
    }

    void JoinConversation()
    {
        CharacterManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        CharacterManager.Instance.DisableAll(false);
        GameUIHandler.Instance.ShowOrbDisplay();

        if (showPortraitAnim)
        {
            showPortraitAnim = false;
            startedPortraitDia = true;
            StartCoroutine(StartPortraitAnim());
        }
    }

    private void ResetVariables()
    {
        lastPhase = GameManager.s_firstPhase;
        startedPortraitDia = false;
        showPortraitAnim = false;

        HidePortrait?.Invoke();

        ResetStaticVariables();

        SetUpScene();

        if (lastPhase == 5)
        {
            AudioManager.Instance.PlayBgMusic(7);
        }
        else if (lastPhase == 4 || lastPhase == 3)
        {
            AudioManager.Instance.PlayBgMusic(6);
        }
        else
        {
            AudioManager.Instance.PlayBgMusic(8);
        }

        SetUpTunnel(false);

        GameManager.s_curOrbs = copyOrbs;
        orb3.SetActive(true);
        GameManager.s_curOrbs[2] = false;
        GameUIHandler.Instance.UpdateOrbTracker();
    }   

    private void ResetStaticVariables()
    {
        s_inTreasureRoom = false;
        s_showTunnel = false;
        s_hideTunnel = false;
        s_climbing = false;
        s_hasTorch = false;
        s_hasKey = false;
        s_freezedTorch = false;
        s_chandelierFreezed = false;
    }

    private void SetUpMusic(int phase)
    {
        if (phase == 5)
        {
            AudioManager.Instance.PlayBgMusic(7);
        }
        else if (lastPhase == 5) // Phase 4
        {
            AudioManager.Instance.PlayBgMusic(6);
        }
        else if (lastPhase == 2 && phase == 3) // Came from phase 2
        {
            AudioManager.Instance.PlayBgMusic(6);
        }
        else if (lastPhase == 3 && phase == 2)
        {
            AudioManager.Instance.PlayBgMusic(8);
        }
        lastPhase = phase;
    }

    private void SetUpMusicAfterDeath()
    {
        int phase = GameManager.s_checkpointPhase;
        if (phase == 5)
        {
            AudioManager.Instance.PlayBgMusic(7);
        }
        else if (phase == 4 || phase == 3)
        {
            AudioManager.Instance.PlayBgMusic(6);
        }
        else if (phase == 2 || phase == 1)
        {
            AudioManager.Instance.PlayBgMusic(8);
        }
        lastPhase = phase;
    }

    private void OnFairyFreeze()
    {
        GameObject obj = FreezeManager.Instance.GetPreservedObject();
        
        if (obj.CompareTag("Chandelier"))
        {
            s_chandelierFreezed = true;
        }
        else if (obj.CompareTag("Candle"))
        {
            string name = obj.name;
            if (name ==  "CandleLit")
            {
                s_freezedTorch = true;
            }
        }
    }

    private void SetUpScene()
    {
        int phase = GameManager.s_curPhase;
        SetUpCurtains(phase);
        SetUpBalconies(phase);
        SetUpBanners(phase);
        SetUpTreasureRoom();
        ShowPortraitAnim(phase);
        SetUpInventory();
        SetUpMusic(phase);
    }

    private void SetUpInventory()
    {
        s_hasTorch = false;
        s_hasKey = false;
        GameUIHandler.Instance.SetUpInventory("Empty");
    }

    private void ShowPortraitAnim(int phase)
    {
        if (phase == 2)
        {
            if (startedPortraitDia)
            {
                StartCoroutine(StartPortraitAnim());
            }
            else
            {
                showPortraitAnim = true;
                GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
            }
        }
    }

    private IEnumerator StartPortraitAnim()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();
        portraitAnim.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        portraitAnim.SetActive(false);
        ShowPortrait?.Invoke();
        CharacterManager.Instance.DisableAll(false);
    }


    private void SetUpTreasureRoom()
    {
        if (!s_inTreasureRoom)
        {
            treasureHider.SetActive(true);
            lockedDoor.SetActive(true);
            openDoor.SetActive(false);
        }
    }

    private void SetUpTunnel(bool showTunnel)
    {
        tunnel.SetActive(showTunnel);
        if (showTunnel)
        {
            GameUIHandler.Instance.HideTimeBar();
        }
        else
        {
            GameUIHandler.Instance.ShowTimeBar();
        }
    }

    private void SetUpBanners(int phase)
    {
        for (int i = 0; i < 3; i++)
        {
            leftBanners[i].SetActive(false);
            rightBanners[i].SetActive(false);
            portalBlocker[i].SetActive(false);
        }
        if (phase == 5)
        {
            leftBanners[2].SetActive(true);
            rightBanners[2].SetActive(true);
            portalBlocker[2].SetActive(true);
        }
        else if (phase == 4)
        {
            leftBanners[1].SetActive(true);
            rightBanners[1].SetActive(true);
            portalBlocker[1].SetActive(true);
        }
        else
        {
            leftBanners[0].SetActive(true);
            rightBanners[0].SetActive(true);
            portalBlocker[0].SetActive(true);
        }
    }

    private void SetUpCurtains(int phase)
    {
        for (int i = 0; i < 5; i ++)
        {
            curtains[i].SetActive(false);
        }
        curtains[phase - 1].SetActive(true);
    }

    private void SetUpBalconies(int phase)
    {
        if (phase == 4 || phase == 5)
        {
            leftBalcony[1].SetActive(true);
            rightBalcony[1].SetActive(true);
            leftBalcony[0].SetActive(false);
            rightBalcony[0].SetActive(false);
        }
        else
        {
            leftBalcony[0].SetActive(true);
            rightBalcony[0].SetActive(true);
            leftBalcony[1].SetActive(false);
            rightBalcony[1].SetActive(false);
        }
    }

    private void OnFairyUnfreeze()
    {
        s_freezedTorch = false;
        s_chandelierFreezed = false;
    }

    private void HideEventSystem()
    {
        eventSystem.SetActive(false);
        camera.SetActive(false);
    }

    private void ShowEventSystem()
    {
        GameUIHandler.Instance.TurnOnGameUI();
        eventSystem.SetActive(true);
        camera.SetActive(true);
    }

    private void OnEnable()
    {
        LevelManager.MusicAfterDeath += SetUpMusicAfterDeath;
        LevelManager.AfterSceneLogic += SetUpScene;
        GameUIHandler.OnDialogueStarted += JoinConversation;
        GameUIHandler.OnDialogueEnded += LeaveConversation;
        LevelManager.OnResetEvent += ResetVariables;
        FreezeManager.OnObjectFreeze += OnFairyFreeze;
        FreezeManager.OnObjectRelease += OnFairyUnfreeze;
        GameUIHandler.HideEventSystem += HideEventSystem;
        GameUIHandler.ShowEventSystem += ShowEventSystem;
    }

    private void OnDisable()
    {
        FreezeManager.OnObjectFreeze -= OnFairyFreeze;
        FreezeManager.OnObjectRelease -= OnFairyUnfreeze;
        LevelManager.MusicAfterDeath -= SetUpMusicAfterDeath;
        LevelManager.AfterSceneLogic -= SetUpScene;
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
        LevelManager.OnResetEvent -= ResetVariables;
        GameUIHandler.HideEventSystem -= HideEventSystem;
        GameUIHandler.ShowEventSystem -= ShowEventSystem;
    }
}
