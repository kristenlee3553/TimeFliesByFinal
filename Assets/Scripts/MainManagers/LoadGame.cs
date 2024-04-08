using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// First script that runs when the game is loaded
/// <br></br>
/// THIS WILL BE IN THE FIRST SCENE
/// <br></br>
/// Artist Credits: 
/// Savannah Han - Wizard/Fairy design and animation. Tutorial design. 
/// Various objects in level 1 and level 2.
/// <br></br>
/// Alyssa Zhao - Backgrounds of level 1 and level 2. Various objects in level 1 and level 2.
/// <br></br>
/// Amanda Li - UI Design
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class LoadGame : MonoBehaviour
{
    [SerializeField] private Image[] dots;
    [SerializeField] private AudioSource musicPlayer;

    // Dot stuff
    readonly private int[] dir = { 1, 1, -1 };
    readonly private float[] startX = { -50f, 0, 50f };

    void Awake()
    {
        // Set up key bindings
        // ------------ FUTURE LOAD KEY BINDINGS FROM FILE ---------------
        GameManager.s_keyBinds = new();

        GameManager.s_keyBinds.Add(GameManager.KeyBind.WizardJump, KeyCode.UpArrow);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.WizardLeft, KeyCode.LeftArrow);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.WizardRight, KeyCode.RightArrow);

        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyLeft, KeyCode.A);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyUp, KeyCode.W);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyRight, KeyCode.D);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.FairyDown, KeyCode.S);

        GameManager.s_keyBinds.Add(GameManager.KeyBind.MoveTimeBack, KeyCode.Q);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.MoveTimeFor, KeyCode.E);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.Preserve, KeyCode.Space);
        GameManager.s_keyBinds.Add(GameManager.KeyBind.Interact, KeyCode.P);

        StartCoroutine(WaitToLoad());
    }

    IEnumerator WaitToLoad()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);

        dots[0].rectTransform.localPosition = new(startX[0], -75f, 0);
        dots[1].rectTransform.localPosition = new(startX[1], -50f, 0);
        dots[2].rectTransform.localPosition = new(startX[2], -25f, 0);

        float min = -75f;
        float max = -25f;

        float deltaY = 10f;

        musicPlayer.Play();

        while (musicPlayer.isPlaying)
        {
            for (int i = 0; i < 3; i++)
            {
                float curY = dots[i].rectTransform.localPosition.y;
                float newY = curY + deltaY * dir[i];

                // Go down
                if (newY > max)
                {
                    newY = max;
                    dir[i] = -1;
                }
                else if (newY < min)
                {
                    newY = min;
                    dir[i] = 1;
                }
                dots[i].rectTransform.localPosition = new(startX[i], newY, 0);
            }
            yield return new WaitForSeconds(.05f);
        }
        GameUIHandler.Instance.LoadUIPage("HomePage");
    }
}
