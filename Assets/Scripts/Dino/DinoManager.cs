using UnityEngine;

/// <summary>
/// Holds key variables in dinosaur stage
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class DinoManager : MonoBehaviour
{

    [SerializeField] private GameObject cave;
    [SerializeField] private GameObject orb1;
    [SerializeField] private GameObject orb2;
    [SerializeField] private GameObject orb3;

    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject camera;

    /// <summary>
    /// If wizard is in cave
    /// </summary>
    public static bool s_inCave = false;

    /// <summary>
    /// Dialogue for the dino level
    /// </summary>
    public DialogueAsset[] dialogueAsset;

    private void SetUpScene()
    {
        cave.SetActive(s_inCave);
    }

    /// <summary>
    /// Resets all variables in the dino level
    /// </summary>
    private void ResetVariables()
    {
        // Start dialogue
        GameUIHandler.Instance.StartDialogue(dialogueAsset[0].dialogue);
        GameUIHandler.Instance.SetHintText("Perhaps we can make use of Fairy's freezing ability...");
        s_inCave = false;
        AudioManager.Instance.PlayBgMusic(0);

        SetUpScene();

        orb1.SetActive(true);
        orb2.SetActive(true);
        orb3.SetActive(true);
        GameManager.s_curOrbs[0] = false;
        GameManager.s_curOrbs[1] = false;
        GameManager.s_curOrbs[2] = false;
        GameUIHandler.Instance.UpdateOrbTracker();
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
        LevelManager.AfterSceneLogic += SetUpScene;
        GameUIHandler.OnDialogueStarted += JoinConversation;
        GameUIHandler.OnDialogueEnded += LeaveConversation;
        LevelManager.OnResetEvent += ResetVariables;
        GameUIHandler.HideEventSystem += HideEventSystem;
        GameUIHandler.ShowEventSystem += ShowEventSystem;
    }

    private void OnDisable()
    {
        LevelManager.AfterSceneLogic -= SetUpScene;
        GameUIHandler.OnDialogueStarted -= JoinConversation;
        GameUIHandler.OnDialogueEnded -= LeaveConversation;
        LevelManager.OnResetEvent -= ResetVariables;
        GameUIHandler.HideEventSystem -= HideEventSystem;
        GameUIHandler.ShowEventSystem -= ShowEventSystem;
    }

    void JoinConversation()
    {
        CharacterManager.Instance.StopAllVelocity();
        CharacterManager.Instance.DisableAll(true);
    }

    void LeaveConversation()
    {
        CharacterManager.Instance.DisableAll(false);
        GameUIHandler.Instance.ShowOrbDisplay();
    }

}
