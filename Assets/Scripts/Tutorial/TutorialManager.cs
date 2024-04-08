using System.Collections;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Stores key variables of tutorial as well as controls the dialogue.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class TutorialManager : MonoBehaviour
{
    public static bool s_hasRipeApple = false;
    public static bool s_hasUnripeApple = false;

    public static bool s_hasSeed = false;
    public static bool s_plantedSeed = false;
    public static bool s_firstPower = false;

    public static bool showGameOver = false;

    /// <summary>
    /// DO NOT TOUCH -> internal variable ish
    /// It is here cuz some bugs are weird
    /// </summary>
    public static bool s_disablePower = false;

    private bool phase5FirstReached = false;
    private bool phase4FirstBack = false;
    private bool phase5SecondReached = false;

    private bool seedPickUpDiaStarted = false;
    private bool seedPlantDiaStarted = false;

    private bool firstPowerDiaStarted = false;
    private bool firstFreeze = false;
    private bool freezePowerTutStarted = false;

    private bool unripeDiaStarted = false;
    private bool ripeDiaStarted = false;

    private bool firstFreezeTimeDiaStarted = false;
    private bool startFreezeDiaTooLong = false;
    private bool startFailingFreeze = false;
    private bool skipFreezeTut = false;

    private bool ripeAppleFreezed = false;

    [SerializeField] private Sprite normalFairy;
    [SerializeField] private Sprite deadFairy;

    [SerializeField] private Camera camera;
    [SerializeField] private GameObject seed;
    [SerializeField] private GameObject gameOverScreen;
    private VideoPlayer vp;

    public DialogueAsset[] dialogueAsset;

    private string path;
    private readonly string videoPath = "/CutScenes/EvilMentor.mp4";

    private void Start()
    {
        vp = camera.GetComponent<VideoPlayer>();
        path = Application.dataPath;
        vp.url = path + videoPath;
        vp.Prepare();
    }

    private void Update()
    {
        int phase = GameManager.s_curPhase;

        // On pick up of seed
        if (!seedPickUpDiaStarted && s_hasSeed)
        {
            GameUIHandler.Instance.SetUpInventory("Seed");
            seedPickUpDiaStarted = true;
            GameUIHandler.Instance.SetHintText("Where shall we plant this seed?");
            GameUIHandler.Instance.StartDialogue(dialogueAsset[1].dialogue);
        }

        // On seed planted
        if (!seedPlantDiaStarted && s_plantedSeed)
        {
            GameUIHandler.Instance.SetUpInventory("Empty");
            seedPlantDiaStarted = true;
            GameUIHandler.Instance.SetHintText("The fairy can press E to move time forwards.");
            GameUIHandler.Instance.StartDialogue(dialogueAsset[2].dialogue);
        }
       
        // When fairy moves time forward for the first time
        if (seedPlantDiaStarted && !CharacterManager.Instance.IsPowerDisabled()
            && !firstPowerDiaStarted)
        {
            if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.MoveTimeFor]))
            {
                s_firstPower = true;
                firstPowerDiaStarted = true;
                GameUIHandler.Instance.SetHintText("How tall can this tree grow?");
                StartCoroutine(StartFirstPower());
            }
           
        }

        // First time reaching the last phase
        if (!phase5FirstReached && phase == 5)
        {
            phase5FirstReached = true;
            StartCoroutine(StartFreezeCountdown());
        }

        // If player reached golden apple phase then went back to phase 4. 
        // Trigger dialogue of mentor teaching player about freezing
        if (phase5FirstReached && phase == 4
            && !ripeAppleFreezed && !phase4FirstBack && !skipFreezeTut)
        {
            GameUIHandler.Instance.SetHintText("I recall the golden apple appears when the tree is fulling grown");
            phase4FirstBack = true;
            StartCoroutine(StartFreezeIntro());
        }

        // Player took too long to figure out -> alternate freeze tutorial
        if (startFreezeDiaTooLong && !phase4FirstBack)
        {
            startFreezeDiaTooLong = false;
            phase4FirstBack = true;
            skipFreezeTut = true;
            freezePowerTutStarted = true;
            GameUIHandler.Instance.SetHintText(
    "The fairy can freeze objects with the space bar. " +
    "Maybe we can freeze the apple...");
            StartCoroutine(StartFreezeIntroTooLong());
        }

        // Freezing tutorial starts
        if (phase5FirstReached && phase4FirstBack && !phase5SecondReached
            && phase == 5 && !skipFreezeTut)
        {
            GameUIHandler.Instance.SetHintText(
                "The fairy can freeze objects with the space bar. " +
                "Maybe we can freeze the apple...");
            phase5SecondReached = true;
            freezePowerTutStarted = true;
            StartCoroutine(StartOnGoingBackToPhase5());
        }

        // Fairy first freezes the apple
        if (freezePowerTutStarted && !firstFreeze && ripeAppleFreezed)
        {
                GameUIHandler.Instance.SetHintText("Uh yes I can move time backwards while freezing the apple!");
                firstFreeze = true;
                StartCoroutine(StartFirstFreeze());
        }

        // Sucessfully freeze and moved time
        if (firstFreeze && phase == 4 && ripeAppleFreezed && !firstFreezeTimeDiaStarted)
        {
            GameUIHandler.Instance.SetHintText("Perhaps the wizard can jump on a smaller tree.");
            firstFreezeTimeDiaStarted = true;
            StartCoroutine(StartFirstFreezeAndTimeTravel());
        }

        // Failed to freeze and move time
        if (firstFreeze && phase == 4 && !ripeAppleFreezed && !firstFreezeTimeDiaStarted
            && !startFailingFreeze)
        {
            startFailingFreeze = true;
            GameUIHandler.Instance.SetHintText("Freeze the golden apple and press Q to move time backwards");
            StartCoroutine(OnFailingFreeze());
        }

        // Pick up of unripe apple
        if (s_hasUnripeApple && !unripeDiaStarted)
        {
            unripeDiaStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[8].dialogue);
        }

        // Level complete
        if (s_hasRipeApple && !ripeDiaStarted)
        {
            ripeDiaStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[7].dialogue);
        }
    }

    /// <summary>
    /// Give players 45 sec on phase 5 before showing dialogue
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFreezeCountdown()
    {
        yield return new WaitForSecondsRealtime(45);

        startFreezeDiaTooLong = true;
    }

    /// <summary>
    /// Showcase fairy using power. Then play dialogue
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFirstPower()
    {
        CharacterManager.Instance.TriggerTimeAnimation();
        AudioManager.Instance.PlayChangeTimeSound();
        yield return StartCoroutine(LevelManager.Instance.LoadPhase(2, true));

        CharacterManager.Instance.DisablePower(true);

        yield return new WaitForSeconds(2.0f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[3].dialogue);
        yield return null;
    }

    private IEnumerator StartFreezeIntro()
    {
        s_disablePower = true;
        yield return new WaitForSeconds(2.0f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[4].dialogue);
        s_disablePower = false;
        yield return null;
    }

    private IEnumerator StartFreezeIntroTooLong()
    {
        s_disablePower = true;
        GameUIHandler.Instance.StartDialogue(dialogueAsset[9].dialogue);
        s_disablePower = false;
        yield return null;
    }

    private IEnumerator StartOnGoingBackToPhase5()
    {
        s_disablePower = true;
        yield return new WaitForSeconds(2.0f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[5].dialogue);
        s_disablePower = false;
        yield return null;
    }

    private IEnumerator StartFirstFreeze()
    {
        s_disablePower = true;
        GameUIHandler.Instance.StartDialogue(dialogueAsset[6].dialogue);
        s_disablePower = false;
        yield return null;
    }

    private IEnumerator StartFirstFreezeAndTimeTravel()
    {
        s_disablePower = true;
        yield return new WaitForSeconds(2.0f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[10].dialogue);
        s_disablePower = false;
        yield return null;
    }

    private IEnumerator OnFailingFreeze()
    {
        s_disablePower = true;
        yield return new WaitForSeconds(2.0f);
        GameUIHandler.Instance.StartDialogue(dialogueAsset[11].dialogue);
        s_disablePower = false;
        yield return null;
    }

    void JoinConversation()
    {
        CharacterManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        CharacterManager.Instance.DisableFairyMovement(false);

        if (s_plantedSeed)
        {
            CharacterManager.Instance.DisablePower(false);
        }
        CharacterManager.Instance.DisableWizardInput(false);

        // Play cutscene
        if (ripeDiaStarted && vp.isPrepared && !GameManager.s_websiteBuild)
        {
            PlayCutScene();
        }
        else if (ripeDiaStarted)
        {
            GameUIHandler.Instance.LoadUIPage("LevelComplete");
        }

        if (showGameOver)
        {
            showGameOver = false;
            StartCoroutine(KillFairyAndWizard());
        }
    }

    private IEnumerator KillFairyAndWizard()
    {
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.ResetWizard();
        CharacterManager.Instance.ResetFairy();

        // Pauses wizard
        CharacterManager.Instance.SetGravityWizard(0);

        CharacterManager.Instance.EnableFairyAnim(false);

        // Death face
        CharacterManager.Instance.SetWizardColor(Color.white);
        CharacterManager.Instance.SetFairySprite(deadFairy);
        CharacterManager.Instance.SetWizardDeadAnimation(true);
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.PlayGameOverSound();

        // Wait
        yield return new WaitForSeconds(0.5f);

        // Allow wizard to fall through objects
        CharacterManager.Instance.SetWizardCollider(false);
        CharacterManager.Instance.SetGravityWizard(1);
        CharacterManager.Instance.SetFairyGravity(1);

        // Launches wizard up
        CharacterManager.Instance.AddForceToWizard(Vector2.up * 5);

        // Wait for wizard to drop out of screen
        yield return new WaitForSeconds(2.5f);

        CharacterManager.Instance.SetFairyGravity(0);
        CharacterManager.Instance.SetFairySprite(normalFairy);
        CharacterManager.Instance.SetWizardCollider(true);
        CharacterManager.Instance.EnableFairyAnim(true);

        GameUIHandler.Instance.TurnOffGameUI();

        // Show GameOver
        gameOverScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);

        // To home screen
        GameUIHandler.Instance.LoadUIPage("HomePage");
    }

    private void PlayCutScene()
    {
        // Get ready for cutscene
        CharacterManager.Instance.DisableAll(true);
        GameUIHandler.Instance.TurnOffGameUI();
        AudioManager.Instance.StopBgMusic();

        // Add method to stop video when done
        vp.loopPointReached += StopVideo;

        // Play cutscene
        vp.Play();
    }

    private void StopVideo(VideoPlayer vp)
    {
        vp.Stop();

        GameUIHandler.Instance.LoadUIPage("LevelComplete");
    }

    public static void ResetStaticVariable()
    {
        s_hasRipeApple = false;
        s_hasUnripeApple = false;

        s_hasSeed = false;
        s_plantedSeed = false;
        s_firstPower = false;
        showGameOver = false;
    }

    /// <summary>
    /// Resets internal variables
    /// </summary>
    private void ResetVariables()
    {
        ResetStaticVariable();

        phase5FirstReached = false;
        phase4FirstBack = false;
        phase5SecondReached = false;

        seedPickUpDiaStarted = false;
        seedPlantDiaStarted = false;

        firstPowerDiaStarted = false;
        firstFreeze = false;
        freezePowerTutStarted = false;

        unripeDiaStarted = false;
        ripeDiaStarted = false;

        firstFreezeTimeDiaStarted = false;
        startFreezeDiaTooLong = false;
        startFailingFreeze = false;
        skipFreezeTut = false;

        ripeAppleFreezed = false;

        seed.SetActive(true);

        SetUpScene();

        GameUIHandler.Instance.HideOrbDisplay();
        GameUIHandler.Instance.HideTimeBar();
        CharacterManager.Instance.DisablePower(true);
        AudioManager.Instance.PlayBgMusic();

        GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
        GameUIHandler.Instance.SetHintText("We should pick up the seed bag.");
    }

    private void SetUpScene()
    {
        if (s_hasSeed)
        {
            seed.SetActive(false);
        }
        // Scene change, so no more apple 
        s_hasRipeApple = false;
        s_hasUnripeApple = false;
    }

    private void FairyFreeze()
    {
        ripeAppleFreezed = FreezeManager.Instance.GetPreservedObject().name.Equals("RipeApple");
    }

    private void FairyRelease()
    {
        ripeAppleFreezed = false;
    }

    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject cameraObject;

    private void HideEventSystem()
    {
        eventSystem.SetActive(false);
        cameraObject.SetActive(false);
    }

    private void ShowEventSystem()
    {
        GameUIHandler.Instance.HideOrbDisplay();
        GameUIHandler.Instance.ShowTopRightMenu();
        if (firstPowerDiaStarted)
        {
            GameUIHandler.Instance.ShowTimeBar();
        }
        eventSystem.SetActive(true);
        cameraObject.SetActive(true);
    }

    private void OnEnable()
    {
        LevelManager.AfterSceneLogic += SetUpScene;
        GameUIHandler.OnDialogueStarted += JoinConversation;
        GameUIHandler.OnDialogueEnded += LeaveConversation;
        LevelManager.OnResetEvent += ResetVariables;
        FreezeManager.OnObjectFreeze += FairyFreeze;
        FreezeManager.OnObjectRelease += FairyRelease;
        GameUIHandler.HideEventSystem += HideEventSystem;
        GameUIHandler.ShowEventSystem += ShowEventSystem;
    }

    private void OnDisable()
    {
        LevelManager.AfterSceneLogic -= SetUpScene;
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
        LevelManager.OnResetEvent -= ResetVariables;
        FreezeManager.OnObjectFreeze -= FairyFreeze;
        FreezeManager.OnObjectRelease -= FairyRelease;
        GameUIHandler.HideEventSystem -= HideEventSystem;
        GameUIHandler.ShowEventSystem -= ShowEventSystem;
    }
}
