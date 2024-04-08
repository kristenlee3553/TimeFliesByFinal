using UnityEngine;

/// <summary>
/// Manager class for Pre tutorial. Handles changing of background and dialogue.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class PreTutManager : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject camera;

    public DialogueAsset[] dialogueAsset;
    public GameObject background1;
    public GameObject background2;

    public static bool fairyDoor = false;

    private bool fairyDiaStarted = false;

    // Update is called once per frame
    void Update()
    {
        if (fairyDoor && !fairyDiaStarted && GameManager.s_curPhase == 1)
        {
            fairyDiaStarted = true;
            GameUIHandler.Instance.StartDialogue(dialogueAsset[2].dialogue);
        }
    }

    private void SetBackground()
    {
        background1.SetActive(false);
        background2.SetActive(true);
    }

    private void ResetVariables()
    {
        fairyDoor = false;
        fairyDiaStarted = false;
        SetUpScene();
        AudioManager.Instance.PlayBgMusic(4);
    }

    private void SetUpScene()
    {
        int phase = GameManager.s_curPhase;

        if (phase == 1)
        {
            GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
            GameUIHandler.Instance.SetHintText("Wizard uses the arrow keys for movement. Fairy uses WASD");
        }
        else
        {
            SetBackground();
            GameUIHandler.Instance.StartDialogue(dialogueAsset[1].dialogue);
            GameUIHandler.Instance.SetHintText("Wizard can jump using the space bar");
        }
    }

    void JoinConversation()
    {
        CharacterManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        CharacterManager.Instance.DisableFairyMovement(false);
        CharacterManager.Instance.DisablePower(true);
        CharacterManager.Instance.DisableWizardInput(false);
    }

    private void HideEventSystem()
    {
        eventSystem.SetActive(false);
        camera.SetActive(false);
    }

    private void ShowEventSystem()
    {
        GameUIHandler.Instance.HideOrbDisplay();
        GameUIHandler.Instance.HideTimeBar();
        GameUIHandler.Instance.ShowTopRightMenu();
        eventSystem.SetActive(true);
        camera.SetActive(true);
    }

    private void OnEnable()
    {
        GameUIHandler.OnDialogueStarted += JoinConversation;
        GameUIHandler.OnDialogueEnded += LeaveConversation;
        LevelManager.OnResetEvent += ResetVariables;
        LevelManager.AfterSceneLogic += SetUpScene;
        GameUIHandler.HideEventSystem += HideEventSystem;
        GameUIHandler.ShowEventSystem += ShowEventSystem;
    }

    private void OnDisable()
    {
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
        LevelManager.OnResetEvent -= ResetVariables;
        LevelManager.AfterSceneLogic -= SetUpScene;
        GameUIHandler.HideEventSystem -= HideEventSystem;
        GameUIHandler.ShowEventSystem -= ShowEventSystem;
    }
}
