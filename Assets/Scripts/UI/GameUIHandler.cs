using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles functionality of GameUI
/// <br></br>
/// Authors:
/// Cynthia Wang: InGameUI
/// Kristen Lee: Everything else
/// </summary>
public class GameUIHandler : MonoBehaviour
{
    /// <summary>
    /// UI that tells player what phase.
    /// Not sure why there is an m.
    /// Unity tutorial had that there.
    /// </summary>
    private VisualElement m_Timebar;

    /// <summary>
    /// The whole time contanier
    /// </summary>
    private VisualElement m_TimeContainer;

    /// <summary>
    /// Holds all 3 orbs
    /// </summary>
    private VisualElement m_OrbContainer;
    private VisualElement m_orb1;
    private VisualElement m_orb2;
    private VisualElement m_orb3;

    private VisualElement m_resetButton;
    private VisualElement m_menuButton;
    private VisualElement m_hintButton;

    /// <summary>
    /// Contains 3 buttons
    /// </summary>
    private VisualElement m_menuButtonContainer;
    
    private TextElement m_hintText;
    private VisualElement m_hintCloseButton;

    private VisualElement m_backToGameButton;
    private VisualElement m_settingsButton;
    private VisualElement m_levelsButton;
    private VisualElement m_quitButton;
    private VisualElement m_inGameMenuCloseButton;

    private VisualElement m_dialogue;
    private VisualElement m_wizardProfile;
    private VisualElement m_timeLordProfile;
    private VisualElement m_fairyProfile;
    private VisualElement m_wizardTag;
    private VisualElement m_timeLordTag;
    private VisualElement m_fairyTag;

    private TextElement m_dialogueText;
    private VisualElement m_nextDialogueButton;
    private VisualElement m_skipDialogueButton;

    private UIDocument uiDocument;
    private VisualElement hintUI;
    private VisualElement inGameMenuUI;

    // ------------ Inventory --------------
    private VisualElement inventoryButton;
    private VisualElement inventoryUI;
    private VisualElement inventoryCloseBtn;
    private VisualElement emptyInventory;
    private VisualElement imageContainer;
    private VisualElement textContainer;
    private VisualElement keyImage;
    private VisualElement torchImage;
    private VisualElement seedImage;
    private VisualElement seedText;
    private VisualElement keyText;
    private VisualElement torchText;

    /// <summary>
    /// Is called before the dialogue is started
    /// </summary>
    public static event Action OnDialogueStarted;

    /// <summary>
    /// Called after dialogue ends
    /// </summary>
    public static event Action OnDialogueEnded;

    /// <summary>
    /// Put in level managers. Hide the event system and camera in the method.
    /// </summary>
    public static Action HideEventSystem;

    /// <summary>
    /// Put in level managers. Show the event system and camera in the method.
    /// </summary>
    public static Action ShowEventSystem;

    private bool skipLineTriggered;
    private bool skipDialogueTriggered;

    /// <summary>
    /// Typewriter speed
    /// </summary>
    private float charactersPerSecond = 30;

    public bool goToHomePage = true;
    public int lastBgMusic = 0;

    /// <summary>
    /// So that other classes can call methods here using the class name
    /// </summary>
    public static GameUIHandler Instance { get; private set; }

    // Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        hintUI = uiDocument.rootVisualElement.Q<VisualElement>("HintUI");
        inGameMenuUI = uiDocument.rootVisualElement.Q<VisualElement>("InGameMenuUI");
        m_Timebar = uiDocument.rootVisualElement.Q<VisualElement>("TimeBar");
        m_TimeContainer = uiDocument.rootVisualElement.Q<VisualElement>("TimeBarBackground");
        m_OrbContainer = uiDocument.rootVisualElement.Q<VisualElement>("OrbContainer");
        m_orb1 = uiDocument.rootVisualElement.Q<VisualElement>("Orb1");
        m_orb2 = uiDocument.rootVisualElement.Q<VisualElement>("Orb2");
        m_orb3 = uiDocument.rootVisualElement.Q<VisualElement>("Orb3");
        m_menuButton = uiDocument.rootVisualElement.Q<VisualElement>("MenuButton");
        m_resetButton = uiDocument.rootVisualElement.Q<VisualElement>("ResetButton");
        m_hintButton = uiDocument.rootVisualElement.Q<VisualElement>("HintButton");
        m_menuButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("MenuButtonContainer");

        m_dialogue = uiDocument.rootVisualElement.Q<VisualElement>("DialogueContainer");
        m_dialogueText = uiDocument.rootVisualElement.Q<TextElement>("DialogueText");
        m_wizardProfile = uiDocument.rootVisualElement.Q<VisualElement>("WizardProfile");
        m_wizardTag = uiDocument.rootVisualElement.Q<VisualElement>("WizardTag");
        m_fairyProfile = uiDocument.rootVisualElement.Q<VisualElement>("FairyProfile");
        m_fairyTag = uiDocument.rootVisualElement.Q<VisualElement>("FairyTag");
        m_timeLordProfile = uiDocument.rootVisualElement.Q<VisualElement>("TimeLordProfile");
        m_timeLordTag = uiDocument.rootVisualElement.Q<VisualElement>("TimeLordTag");
        m_nextDialogueButton = uiDocument.rootVisualElement.Q<VisualElement>("NextDiaButton");
        m_skipDialogueButton = uiDocument.rootVisualElement.Q<VisualElement>("SkipDiaButton");

        m_hintCloseButton = uiDocument.rootVisualElement.Q<VisualElement>("HintCloseButton");
        m_hintText = uiDocument.rootVisualElement.Q<TextElement>("HintText");

        m_backToGameButton = uiDocument.rootVisualElement.Q<VisualElement>("BackToGameButton");
        m_settingsButton = uiDocument.rootVisualElement.Q<VisualElement>("SettingsButton");
        m_levelsButton = uiDocument.rootVisualElement.Q<VisualElement>("LevelButton");
        m_quitButton = uiDocument.rootVisualElement.Q<VisualElement>("ExitButton");
        m_inGameMenuCloseButton = uiDocument.rootVisualElement.Q<VisualElement>("InGameMenuCloseButton");

        inventoryButton = uiDocument.rootVisualElement.Q<VisualElement>("InventoryButton");
        inventoryUI =  uiDocument.rootVisualElement.Q<VisualElement>("InventoryUI");
        inventoryCloseBtn = uiDocument.rootVisualElement.Q<VisualElement>("InventoryCloseBtn");
        emptyInventory = uiDocument.rootVisualElement.Q<TextElement>("EmptyInventory");
        imageContainer = uiDocument.rootVisualElement.Q<VisualElement>("ImageContainer");
        textContainer = uiDocument.rootVisualElement.Q<VisualElement>("TextContainer");
        keyImage = uiDocument.rootVisualElement.Q<VisualElement>("Key");
        torchImage = uiDocument.rootVisualElement.Q<VisualElement>("Torch");
        seedImage = uiDocument.rootVisualElement.Q<VisualElement>("Seed");
        seedText = uiDocument.rootVisualElement.Q<VisualElement>("SeedText");
        keyText = uiDocument.rootVisualElement.Q<VisualElement>("KeyText");
        torchText = uiDocument.rootVisualElement.Q<VisualElement>("TorchText");

        inventoryButton.RegisterCallback<ClickEvent>(ShowInventoryMenu);
        inventoryCloseBtn.RegisterCallback<ClickEvent>(CloseInvetoryMenu);
        m_hintCloseButton.RegisterCallback<ClickEvent>(CloseHintMenu);
        m_hintButton.RegisterCallback<ClickEvent>(ShowHintMenu);
        m_skipDialogueButton.RegisterCallback<ClickEvent>(SkipDialogue);
        m_nextDialogueButton.RegisterCallback<ClickEvent>(SkipLine);
        m_resetButton.RegisterCallback<ClickEvent>(ResetEvent);
        m_menuButton.RegisterCallback<ClickEvent>(ShowInGameMenu);
        m_inGameMenuCloseButton.RegisterCallback<ClickEvent>(CloseInGameMenu);
        m_backToGameButton.RegisterCallback<ClickEvent>(CloseInGameMenu);
        m_levelsButton.RegisterCallback<ClickEvent>(GoToLevelScreen);
        m_quitButton.RegisterCallback<ClickEvent>(Quit);
        m_settingsButton.RegisterCallback<ClickEvent>(ShowSettings);

        SetPhaseBar(GameManager.s_firstPhase);
        UpdateOrbTracker();
    }

    private void ShowInventoryMenu(ClickEvent evt)
    {
        CharacterManager.Instance.StopAllVelocity();
        inventoryUI.style.display = DisplayStyle.Flex;
        StopAllCoroutines();
        CharacterManager.Instance.DisableAll(true);
    }

    private void CloseInvetoryMenu(ClickEvent evt)
    {
        inventoryUI.style.display = DisplayStyle.None;
        CharacterManager.Instance.HandlePowerDisabling();
        CharacterManager.Instance.DisableWizardInput(false);
        CharacterManager.Instance.DisableFairyMovement(false);
    }

    /// <summary>
    /// Seed, Key, Candle, Empty
    /// </summary>
    /// <param name="item"></param>
    public void SetUpInventory(string item)
    {
        if (item == "Empty")
        {
            emptyInventory.style.display = DisplayStyle.Flex;
            imageContainer.style.display = DisplayStyle.None;
            textContainer.style.display = DisplayStyle.None;
        }
        else
        {
            emptyInventory.style.display = DisplayStyle.None;
            keyImage.style.display = DisplayStyle.None;
            torchImage.style.display = DisplayStyle.None;
            seedImage.style.display = DisplayStyle.None;
            seedText.style.display = DisplayStyle.None;
            torchText.style.display = DisplayStyle.None;
            keyText.style.display = DisplayStyle.None;
            imageContainer.style.display = DisplayStyle.Flex;
            textContainer.style.display = DisplayStyle.Flex;

            if (item == "Candle")
            {
                torchText.style.display = DisplayStyle.Flex;
                torchImage.style.display = DisplayStyle.Flex;
            }
            else if (item == "Key")
            {
                keyText.style.display = DisplayStyle.Flex;
                keyImage.style.display = DisplayStyle.Flex;
            }
            else
            {
                seedText.style.display = DisplayStyle.Flex;
                seedImage.style.display = DisplayStyle.Flex;
            }
        }
    }

    /// <summary>
    /// Shows hint menu and disables input
    /// </summary>
    /// <param name="evt"></param>
    private void ShowHintMenu(ClickEvent evt)
    {
        CharacterManager.Instance.StopAllVelocity();
        hintUI.style.display = DisplayStyle.Flex;
        StopAllCoroutines();
        CharacterManager.Instance.DisableAll(true);
    }

    private void CloseHintMenu(ClickEvent evt)
    {
        hintUI.style.display = DisplayStyle.None;
        CharacterManager.Instance.HandlePowerDisabling();
        CharacterManager.Instance.DisableWizardInput(false);
        CharacterManager.Instance.DisableFairyMovement(false);
    }

    /// <summary>
    /// Sets the hint text
    /// </summary>
    /// <param name="hint"></param>
    public void SetHintText(string hint)
    {
        m_hintText.text = hint;
    }

    /// <summary>
    /// Resets level. Respawns wizard and fairy
    /// </summary>
    /// <param name="evt"></param>
    private void ResetEvent(ClickEvent evt)
    {
        LevelManager.Instance.ResetLevel(true);
    }

    // Shows in game menu and disables input
    private void ShowInGameMenu(ClickEvent evt)
    {
        CharacterManager.Instance.StopAllVelocity();
        inGameMenuUI.style.display = DisplayStyle.Flex;
        StopAllCoroutines();
        CharacterManager.Instance.DisableAll(true);
    }

    private void CloseInGameMenu(ClickEvent evt)
    {
        inGameMenuUI.style.display = DisplayStyle.None;
        CharacterManager.Instance.HandlePowerDisabling();
        CharacterManager.Instance.DisableWizardInput(false);
        CharacterManager.Instance.DisableFairyMovement(false);
    }

    private void ShowSettings(ClickEvent evt)
    {
        HideEventSystem?.Invoke();
        lastBgMusic = AudioManager.Instance.GetCurrentMusicIndex();
        goToHomePage = false;
        LoadUIPage("Settings", true);
    }

    public void HideInGameSettings()
    {
        StartCoroutine(UnloadIngameSettings());
    }

    private IEnumerator UnloadIngameSettings()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("Settings");

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        GameManager.s_onGameLevel = true;
        AudioManager.Instance.PlayBgMusic(lastBgMusic);
        ShowEventSystem?.Invoke();
        ShowInGameMenu(null);
    }

    private void GoToLevelScreen(ClickEvent evt)
    {
        LoadUIPage("LevelSelect");
    }

    private void Quit(ClickEvent evt)
    {
        LoadUIPage("HomePage");
    }

    /// <summary>
    /// USE THIS METHOD TO LOAD UI PAGES
    /// Pass 2nd parameter as true if from in game settings to skip unloading scenes
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadUIPage(string sceneName, bool fromInGame = false)
    {
        if (sceneName == "LevelComplete")
        {
            AudioManager.Instance.StopBgMusic();
            AudioManager.Instance.PlayLevelCompleteSound();
        }
        else if (GameManager.s_onGameLevel)
        {
            AudioManager.Instance.PlayBgMusic(9);
            GameManager.s_onGameLevel = false;
        }
        
        if (!fromInGame)
        {
            int numScenes = SceneManager.loadedSceneCount;
            for (int i = 0; i < numScenes; i++)
            {
                string curSceneName = SceneManager.GetSceneAt(i).name;

                if (curSceneName != "GameScene")
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                }
            }
        }
        TurnOffGameUI();
        CharacterManager.Instance.StopAllVelocity();
        CharacterManager.Instance.DisableAll(true);

        // Load UI Page
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Updates Time Tracker Bar
    /// </summary>
    /// <param name="phase"></param>
    public void SetPhaseBar(int phase)
    {
        float percent = (phase - 1) / 4.0f;
        m_Timebar.style.width = Length.Percent(100 * percent);
    }

    /// <summary>
    /// Updates orb UI
    /// </summary>
    public void UpdateOrbTracker()
    {
        m_orb1.SetEnabled(GameManager.s_curOrbs[0]);
        m_orb2.SetEnabled(GameManager.s_curOrbs[1]);
        m_orb3.SetEnabled(GameManager.s_curOrbs[2]);
    }

    /// <summary>
    /// Updates speed based on value in game manager
    /// </summary>
    public void UpdateDialogueSpeed()
    {
        if (GameManager.s_dialogueSpeed == 1f)
        {
            charactersPerSecond = 15f;
        }
        else if (GameManager.s_dialogueSpeed == 2f)
        {
            charactersPerSecond = 30f;
        }
        else
        {
            charactersPerSecond = 60f;
        }
    }

    /// <summary>
    /// Starts dialogue. Send dialogue to play
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="startPosition"></param>
    /// <param name="name">Wizard, Fairy, TimeLord </param>
    public void StartDialogue(MyDialogue[] dialogue)
    {
        // Stop movement
        CharacterManager.Instance.StopAllVelocity();

        // Hide other UI
        HideOrbDisplay();
        HideTopRightMenu();
        
        // Show Dialogue
        m_dialogue.style.display = DisplayStyle.Flex;

        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue));
    }

    private IEnumerator RunDialogue(MyDialogue[] dialogue)
    {
        skipLineTriggered = false;
        skipDialogueTriggered = false;
        OnDialogueStarted?.Invoke();

        for (int i = 0; i < dialogue.Length; i++)
        {
            // Skip dialogue
            if (skipDialogueTriggered && !dialogue[i].checkpoint)
            {
                continue;
            }
            // Skip to checkpoint
            else if (skipDialogueTriggered && dialogue[i].checkpoint)
            {
                skipDialogueTriggered = false;
                skipLineTriggered = false;
            }
            if (!skipLineTriggered)
            {
                // Textwriter effect
                StartCoroutine(TypeTextUncapped(dialogue[i].dialogue));
            }

            // Show appropiate person
            DisplayTagAndProfile(dialogue[i].name);
            while (skipLineTriggered == false && skipDialogueTriggered == false)
            {
                yield return null;
            }
            skipLineTriggered = false;
            HideTagAndProfile(dialogue[i].name);
        }
        // UI
        m_dialogue.style.display = DisplayStyle.None;
        ShowTopRightMenu();

        OnDialogueEnded?.Invoke();
    }

    /// <summary>
    /// Typewriter effect
    /// Code from here: https://gamedevbeginner.com/dialogue-systems-in-unity/
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private IEnumerator TypeTextUncapped(string line)
    {
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (skipLineTriggered || skipDialogueTriggered)
            {
                yield break;
            }
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                m_dialogueText.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Skips one line of dialogue
    /// </summary>
    /// <param name="evt"></param>
    public void SkipLine(ClickEvent evt)
    {
        skipLineTriggered = true;
    }

    /// <summary>
    /// Skip to nearest breakpoint
    /// </summary>
    /// <param name="evt"></param>
    public void SkipDialogue(ClickEvent evt)
    {
        skipDialogueTriggered = true;
    }

    /// <summary>
    /// Displays who is speaking
    /// </summary>
    /// <param name="name"></param>
    private void DisplayTagAndProfile(string name)
    {
        // Show appropiate person
        if (name.Equals("Wizard"))
        {
            m_wizardTag.style.display = DisplayStyle.Flex;
            m_wizardProfile.style.display = DisplayStyle.Flex;
        }
        else if (name.Equals("Fairy"))
        {
            m_fairyProfile.style.display = DisplayStyle.Flex;
            m_fairyTag.style.display = DisplayStyle.Flex;
        }
        else
        {
            m_timeLordProfile.style.display = DisplayStyle.Flex;
            m_timeLordTag.style.display = DisplayStyle.Flex;
        }
    }

    /// <summary>
    /// Hides who is speaking
    /// </summary>
    /// <param name="name"></param>
    private void HideTagAndProfile(string name)
    {
        if (name.Equals("Wizard"))
        {
            m_wizardTag.style.display = DisplayStyle.None;
            m_wizardProfile.style.display = DisplayStyle.None;
        }
        else if (name.Equals("Fairy"))
        {
            m_fairyProfile.style.display = DisplayStyle.None;
            m_fairyTag.style.display = DisplayStyle.None;
        }
        else
        {
            m_timeLordProfile.style.display = DisplayStyle.None;
            m_timeLordTag.style.display = DisplayStyle.None;
        }
    }

    /// <summary>
    /// Shows whole game UI
    /// </summary>
    public void TurnOnGameUI()
    {
        ShowTopRightMenu();
        ShowOrbDisplay();
        ShowTimeBar();
    }

    /// <summary>
    /// Hides whole game UI
    /// </summary>
    public void TurnOffGameUI()
    {
        HideTopRightMenu();
        HideOrbDisplay();
        HideTimeBar();
        inGameMenuUI.style.display = DisplayStyle.None;
    }

    public void ShowTopRightMenu()
    {
        m_menuButtonContainer.style.display = DisplayStyle.Flex;
    }

    public void HideTopRightMenu()
    {
        m_menuButtonContainer.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Hides all orbs
    /// </summary>
    public void HideOrbDisplay()
    {
        m_OrbContainer.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Shows all orbs
    /// </summary>
    public void ShowOrbDisplay()
    {
        m_OrbContainer.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Hides time bar
    /// </summary>
    public void HideTimeBar()
    {
        m_TimeContainer.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Shows time bar
    /// </summary>
    public void ShowTimeBar()
    {
        m_TimeContainer.style.display = DisplayStyle.Flex;
    }
}
