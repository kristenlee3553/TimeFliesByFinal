using UnityEngine;
using System.Collections;

/// <summary>
/// Manager for Level 2 part 1.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class MedOneManager : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject castle;
    [SerializeField] private GameObject brokenCastle;
    [SerializeField] private GameObject bridgeOpen;
    [SerializeField] private GameObject bridgeDown;
    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject horsesWalking;
    [SerializeField] private GameObject orb1;

    public DialogueAsset[] dialogueAsset;

    private SpriteRenderer gateSprite;
    private Gate gateScript;

    public static bool leverFlipped = false;
    public static bool noHorsesInFrontOfStable = true;
    public static bool noHorsesInStable = true;
    public static bool playOpenStableDia = false;
    public static bool playLockedStableDia = false;
    public static bool ableToDive = false;

    private bool cartFreezed;
    private bool horsesOutFreezed;
    private bool horsesInFreezed;
    private bool leverFreezed;
    private bool gateIsOpen;
    private bool playHorseWalk;
    private bool horseDiaStarted;
    private bool stableOrbObtained;
    private bool lastPhaseIs3;
    private bool playOrbSound;

    // Start is called before the first frame update
    void Start()
    {
        gateSprite = gate.GetComponent<SpriteRenderer>();
        gateScript = gate.GetComponent<Gate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leverFlipped)
        {
            leverFlipped = false;
            gateIsOpen = !gateIsOpen;
            AudioManager.Instance.PlayLeverSound();
            SetUpGate();
        }
        if (playOpenStableDia)
        {
            if (stableOrbObtained)
            {
                GameUIHandler.Instance.StartDialogue(dialogueAsset[3].dialogue);
            }
            else
            {
                stableOrbObtained = true;
                GameManager.s_curOrbs[1] = true;
                GameUIHandler.Instance.UpdateOrbTracker();
                GameUIHandler.Instance.StartDialogue(dialogueAsset[2].dialogue);
            }
            playOpenStableDia = false;
        }

        if (playLockedStableDia)
        {
            if (stableOrbObtained)
            {
                GameUIHandler.Instance.StartDialogue(dialogueAsset[3].dialogue);
            }
            else
            {
                GameUIHandler.Instance.StartDialogue(dialogueAsset[4].dialogue);
            }
            playLockedStableDia = false;
        }
    }

    private void SetUpScene()
    {
        int phase = GameManager.s_curPhase;
        SetUpLever();
        SetUpGate();
        SetUpCastle(phase);
        SetUpMusic(phase);
        SetUpHorses(phase);
        SetUpBridge(phase);

        ableToDive = (cartFreezed || phase == 1);
    }

    private void SetUpHorses(int phase)
    {
        noHorsesInStable = true;
        noHorsesInFrontOfStable = true;

        if (horsesInFreezed)
        {
            noHorsesInStable = false;
        }
        else
        {
            if (phase == 3)
            {
                noHorsesInStable = false;
            }
        }
        if (horsesOutFreezed)
        {
            noHorsesInFrontOfStable = false;
        }
        else
        {
            if (phase == 2)
            {
                noHorsesInFrontOfStable = false;
            }
        }
    }

    private void SetUpMusic(int phase)
    {
        if (phase == 3)
        {
            lastPhaseIs3 = true;
            AudioManager.Instance.PlayBgMusic(2);
        }
        else if (lastPhaseIs3)
        {
            lastPhaseIs3 = false;
            AudioManager.Instance.PlayBgMusic(3);
        }
    }

    public void SetUpBridge(int phase)
    {
        bridgeDown.SetActive(false);
        bridgeOpen.SetActive(false);

        if (phase == 2)
        {
            bridgeDown.SetActive(true);
        }
        else
        {
            bridgeOpen.SetActive(true);
        }

        CheckHorseFood();
    }

    private void SetUpGate()
    {
        if (gateIsOpen)
        {
            gateSprite.color = Color.clear;
            gateScript.open = true;
        }
        else
        {
            gateSprite.color = Color.white;
            gateScript.open = false;
        }
    }

    private void SetUpCastle(int phase)
    {
        // Crumbled castle
        if (phase == 5)
        {
            castle.SetActive(false);
            brokenCastle.SetActive(true);
        }
        else
        {
            castle.SetActive(true);
            brokenCastle.SetActive(false);
        }
    }

    private void SetUpLever()
    {
        // If not preserving the lever, set to default
        if (!leverFreezed)
        {
            gateIsOpen = false;
        }
    }

    private void CheckHorseFood()
    {
        int phase = GameManager.s_curPhase;

        // Food brought to horses in phase 2
        if (phase == 2 && cartFreezed)
        {
            // Play Dialogue
            if (!horseDiaStarted)
            {
                playHorseWalk = true;
                horseDiaStarted = true;
                GameUIHandler.Instance.StartDialogue(dialogueAsset[1].dialogue);
            }
            else
            {
                StartCoroutine(StartHorseWalk());
            }
        }
    }

    private IEnumerator StartHorseWalk()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.StopAllVelocity();

        // Hide horses
        GameObject horse = GameObject.FindGameObjectWithTag("Horse");
        if (horse != null)
        {
            horse.SetActive(false);
        }
        float startX = 2.38f;
        float startY = -4.14f;
        horsesWalking.transform.position = new(startX, startY, 0);
        horsesWalking.SetActive(true);
        AudioManager.Instance.PlayHorseTrot();

        for (int i = 0; i < 100; i++)
        {
            startX -= 0.0426f;
            horsesWalking.transform.position = new(startX, startY, 0);
            yield return new WaitForSeconds(.01f);
        }

        horsesWalking.SetActive(false);

        noHorsesInFrontOfStable = true;
        FreezeManager.Instance.ResetManager();
        CharacterManager.Instance.DisableAll(false);
    }

    void JoinConversation()
    {
        CharacterManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        CharacterManager.Instance.DisableAll(false);
        GameUIHandler.Instance.ShowOrbDisplay();

        if (playHorseWalk)
        {
            playHorseWalk = false;
            StartCoroutine(StartHorseWalk());
        }

        if (stableOrbObtained && !playOrbSound)
        {
            playOrbSound = true;
            AudioManager.Instance.PlayOrbSound();
        }
    }

    private void ResetVariables()
    {
        noHorsesInFrontOfStable = true;
        playOpenStableDia = false;
        playHorseWalk = false;
        horseDiaStarted = false;
        playLockedStableDia = false;
        stableOrbObtained = false;
        ableToDive = false;
        cartFreezed = false;
        horsesInFreezed = false;
        noHorsesInStable = true;
        horsesOutFreezed = false;
        leverFreezed = false;
        playOrbSound = false;
        gateIsOpen = false;
        SetUpScene();
        GameManager.s_curOrbs[1] = false;
        GameManager.s_curOrbs[0] = false;
        GameManager.s_curOrbs[2] = false;
        orb1.SetActive(true);
        GameUIHandler.Instance.UpdateOrbTracker();
        AudioManager.Instance.PlayBgMusic(3);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
    }

    private void SetUpMusicAfterDeath()
    {
        if (GameManager.s_checkpointPhase == 3)
        {
            lastPhaseIs3 = true;
            AudioManager.Instance.PlayBgMusic(2);
        }
        else
        {
            AudioManager.Instance.PlayBgMusic(3);
        }
    }

    private void OnFairyFreeze()
    {
        GameObject obj = FreezeManager.Instance.GetPreservedObject();
        string name = obj.name;
        string tag = obj.tag;

        if (tag == "Cart")
        {
            cartFreezed = true;
        }
        else if (tag == "Lever")
        {
            leverFreezed = true;
        }
        else if (tag == "Horse" && name == "HorsesOut")
        {
            horsesOutFreezed = true;
        }
        else if (tag == "Horse" && name == "HorseInStable")
        {
            horsesInFreezed = true;
        }
    }

    private void OnFairyUnfreeze()
    {
        cartFreezed = false;
        horsesOutFreezed = false;
        horsesInFreezed = false;
        leverFreezed = false;
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
        LevelManager.MusicAfterDeath -= SetUpMusicAfterDeath;
        LevelManager.AfterSceneLogic -= SetUpScene;
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
        LevelManager.OnResetEvent -= ResetVariables;
        FreezeManager.OnObjectRelease -= OnFairyUnfreeze;
        GameUIHandler.HideEventSystem -= HideEventSystem;
        GameUIHandler.ShowEventSystem -= ShowEventSystem;
    }
}