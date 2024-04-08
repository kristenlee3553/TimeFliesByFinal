using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// HANDLES RESETTING OF LEVEL, CHANGING PHASES, DEATH ANIMATION.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject wizard;

    /// <summary>
    /// Level setup after generic scene change
    /// If I knew about this I would have done this earlier
    /// </summary>
    public static event Action AfterSceneLogic;

    /// <summary>
    /// Will call to restart music
    /// </summary>
    public static event Action MusicAfterDeath;

    /// <summary>
    /// Will call to relocate wizard after scene change
    /// </summary>
    public static event Action RelocateWizard;

    /// <summary>
    /// Called after level is loaded after reset
    /// </summary>
    public static event Action OnResetEvent;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    /// After unloading the last phase. Includes checking for death
    /// </summary>
    /// <param name="scene"></param>
    private void OnSceneUnloaded(Scene scene)
    {
        // If not on UI screen
        if (GameManager.s_onGameLevel)
        {
            CheckOnSceneCollision();
        }
    }

    /// <summary>
    /// Here lies logic for the preservation of objects
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.s_onGameLevel && GameManager.s_level != "PreTut")
        {
            PreserveObject();
        }
    }

    /// <summary>
    /// Brings frozen object to current scene if needed. Hides objects with same tag.
    /// </summary>
    private void PreserveObject()
    {
        // If fairy is preserving an object that is not the wizard
        if (FreezeManager.Instance.GetPreservedObject() != null && !FreezeManager.Instance.IsPreservingWizard())
        {
            // Object to hide
            string tagToFind = FreezeManager.Instance.GetPreservedObject().tag;

            if (tagToFind != "NoTag")
            {
                // Find objects to hide
                GameObject[] dups = GameObject.FindGameObjectsWithTag(tagToFind);
                foreach (GameObject dup in dups)
                {
                    // If object is not the same as preserved object
                    if (GameObject.ReferenceEquals(dup, FreezeManager.Instance.GetPreservedObject()) == false)
                    {
                        // Hide object
                        dup.SetActive(false);
                    }
                }
            }
            // Move preserved object to scene
            SceneManager.MoveGameObjectToScene(FreezeManager.Instance.GetPreservedObject(),
                SceneManager.GetSceneByName(GameManager.s_curScene));
        }
    }

    /// <summary>
    /// Loads Level background of parameter passed in. Calls RespwanWizard to load first phase
    /// If third param set to a level, will unload background of level that is already loaded.
    /// Will also attempt to unload phase of that level
    /// Useful for transionting between parts I.E level 2 part1 to level2 part2
    /// Updates level of GameManager to level passed in
    /// </summary>
    /// <returns></returns>
    public void LoadNewLevel(string level, int firstPhase, string preLevel = "", int prePhase = -1)
    {
        StartCoroutine(LoadLevel(level, firstPhase, preLevel, prePhase));
    }

    private IEnumerator LoadLevel(string level, int firstPhase, string preLevel, int prePhase)
    {
        // Disable fairy powers 
        CharacterManager.Instance.DisablePower(true);
        GameManager.s_onGameLevel = false; // Disable scene logic

        // Unload previous level
        if (preLevel != "" && prePhase != -1)
        {
            // Phase
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(preLevel + prePhase);

            // Wait until scene is unloaded
            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            // Background
            AsyncOperation asyncUnload2 = SceneManager.UnloadSceneAsync(preLevel + "Back");

            // Wait until scene is unloaded
            while (!asyncUnload2.isDone)
            {
                yield return null;
            }
        }

        GameManager.s_level = level;

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level + "Back", LoadSceneMode.Additive);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameManager.s_onGameLevel = true; // Enable scene logic

        yield return RespawnWizard(true, false);
    }

    public void ChangePhase(int nextPhase, bool unloadLastPhase = true)
    {
        StartCoroutine(LoadPhase(nextPhase, unloadLastPhase));
    }

    /// <summary>
    /// Changes scene to current level store in GameManager + phase passed to function. 
    /// If second param set to true, will unload last phase. Default is set to true
    /// Updates UI bar and variables in GameManager
    /// <br></br>
    /// Calls AfterSceneLogic at the end
    /// </summary>
    /// <param name="nextPhase"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public IEnumerator LoadPhase(int nextPhase, bool unloadLastPhase)
    {
        // Disable fairy powers 
        CharacterManager.Instance.DisablePower(true);
        GameManager.s_onGameLevel = true; // Enable scene logic

        // Name of next scene
        string next_scene = GameManager.s_level + nextPhase;
        string lastPhase = GameManager.s_curScene;
        GameManager.s_curScene = next_scene;

        // Update variable
        GameManager.s_curPhase = nextPhase;

        // Reposition wizard if needed
        RelocateWizard?.Invoke();

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next_scene, LoadSceneMode.Additive);

        // Wait until scene is loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (unloadLastPhase)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(lastPhase);

            // Wait until scene is unloaded
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }

        // UI Bar
        GameUIHandler.Instance.SetPhaseBar(nextPhase);

        CharacterManager.Instance.HandlePowerDisabling();

        AfterSceneLogic?.Invoke();

        yield break;

    }

    /// <summary>
    /// Checks if wizard is smacked in the face when the scene changes. 
    /// Plays Death animation if smacked.
    /// </summary>
    private void CheckOnSceneCollision()
    {
        StartCoroutine(CheckDeathCollision());

    }

    IEnumerator CheckDeathCollision()
    {
        // Check wizard collision
        GameManager.s_sceneChange = true;

        // Delay -> allow objects to check their colliders
        yield return new WaitForSeconds(0.15f);

        // Game Over
        if (GameManager.s_onDeathObject)
        {
            StartDeathAnimation(false);
        }

        // Turn off wizard collision
        GameManager.s_sceneChange = false;

        yield return null;
    }

    /// <summary>
    /// Send true if fully reseting the level
    /// False resets the level to the last checkpoint and plays death animation
    /// </summary>
    /// <param name="resetLevel"></param>
    public void ResetLevel(bool resetLevel)
    {
        if (resetLevel)
        {
            StartCoroutine(RespawnWizard(resetLevel));
        }
        else
        {
            StartDeathAnimation(resetLevel);
        }
    }

    public void StartDeathAnimation(bool resetLevel, Color deathColor)
    {
        StartCoroutine(DeathAnimation(resetLevel, deathColor));
    }

    public void StartDeathAnimation(bool resetLevel)
    {
        StartCoroutine(DeathAnimation(resetLevel, Color.white));
    }

    /// <summary>
    ///  Show death animation. After calls a function to respawn Wizard to the checkpoint. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathAnimation(bool resetLevel, Color deathColor)
    {
        // Pause input and reset character's variables and velocity
        CharacterManager.Instance.DisableAll(true);
        CharacterManager.Instance.ResetWizard();
        CharacterManager.Instance.ResetFairy();

        // Pauses wizard
        CharacterManager.Instance.SetGravityWizard(0);

        // Death face
        CharacterManager.Instance.SetWizardColor(deathColor);
        CharacterManager.Instance.SetWizardDeadAnimation(true);
        AudioManager.Instance.StopBgMusic();
        AudioManager.Instance.PlayGameOverSound();

        // Wait
        yield return new WaitForSeconds(0.25f);

        // Allow wizard to fall through objects
        CharacterManager.Instance.SetWizardCollider(false);
        CharacterManager.Instance.SetGravityWizard(1);

        // Launches wizard up
        CharacterManager.Instance.AddForceToWizard(Vector2.up * 5);

        // Wait for wizard to drop out of screen
        yield return new WaitForSeconds(2.5f);

        if (MusicAfterDeath == null)
        {
            AudioManager.Instance.PlayBgMusic();
        }
        else
        {
            MusicAfterDeath?.Invoke();
        }

        yield return StartCoroutine(RespawnWizard(resetLevel));
    }


    /// <summary>
    /// Resets level fully or to the nearest checkpoint depending on parameter. 
    /// True if fully reseting a level
    /// </summary>
    /// <returns></returns>
    private IEnumerator RespawnWizard(bool resetLevel, bool unloadLastScene = true)
    {
        CharacterManager.Instance.ResetWizard();
        CharacterManager.Instance.ResetFairy();

        // Scene change
        if (resetLevel)
        {
            // Back to phase 1
            yield return StartCoroutine(LoadPhase(GameManager.s_firstPhase, unloadLastScene));
        }
        else
        {
            // Load chckpoint scene
            yield return StartCoroutine(LoadPhase(GameManager.s_checkpointPhase, true));
        }

        float wizardX = resetLevel ? GameManager.s_wizardResetX : GameManager.s_wizardRespawnX;
        float wizardY = resetLevel ? GameManager.s_wizardResetY : GameManager.s_wizardRespawnY;

        // Move wizard to respawn point
        CharacterManager.Instance.SetWizardDeadAnimation(false);
        CharacterManager.Instance.SetWizardColor(Color.white);
        CharacterManager.Instance.SetWizardPosition(wizardX, wizardY, 0);

        // Move fairy to respawn point
        CharacterManager.Instance.SetFairyColor(Color.white);
        CharacterManager.Instance.SetFairyPosition(GameManager.s_fairyResetX, GameManager.s_fairyResetY, 0);

        // Enable Input and Reset back to normal
        CharacterManager.Instance.HandlePowerDisabling();
        CharacterManager.Instance.DisableFairyMovement(false);
        CharacterManager.Instance.DisableWizardInput(false);
        CharacterManager.Instance.SetWizardCollider(true);
        GameUIHandler.Instance.SetUpInventory("Empty");

        if (resetLevel)
        {
            OnResetEvent?.Invoke();
        }
    }


    /// <summary>
    /// Resets wizard, fairy, orbs, everything
    /// </summary>
    public void ResetGame()
    {
        for (int i = 0; i < GameManager.s_curOrbs.Length; i++)
        {
            GameManager.s_curOrbs[i] = false;
        }
        GameUIHandler.Instance.UpdateOrbTracker();
        CharacterManager.Instance.HandlePowerDisabling();
        CharacterManager.Instance.DisableFairyMovement(false);
        CharacterManager.Instance.DisableWizardInput(false);
        GameUIHandler.Instance.SetPhaseBar(GameManager.s_firstPhase);
        CharacterManager.Instance.ResetWizard();
        CharacterManager.Instance.ResetFairy();
    }
}