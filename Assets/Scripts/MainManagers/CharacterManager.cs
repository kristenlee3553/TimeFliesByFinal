using UnityEngine;

/// <summary>
/// Class that has methods to manipulate fairy and wizard
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject wizard;
    [SerializeField] private GameObject fairy;

    private Rigidbody2D rb;
    private Rigidbody2D fairyRb;
    private WizardMovement wizardMove;
    private FairyMovement fairyMove;
    private SpriteRenderer wizardSpriteRenderer;
    private SpriteRenderer fairySpriteRenderer;
    private CapsuleCollider2D col;
    private Animator fairyAnim;
    private Animator wizardAnim;

    public static CharacterManager Instance { get; private set; }

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

    // Start is called before the first frame update
    void Start()
    {
        rb = wizard.GetComponent<Rigidbody2D>();
        col = wizard.GetComponent<CapsuleCollider2D>();

        fairyRb = fairy.GetComponent<Rigidbody2D>();

        wizardMove = wizard.GetComponent<WizardMovement>();
        fairyMove = fairy.GetComponent<FairyMovement>();

        wizardSpriteRenderer = wizard.GetComponent<SpriteRenderer>();
        fairySpriteRenderer = fairy.GetComponent<SpriteRenderer>();

        fairyAnim = fairy.GetComponent<Animator>();
        wizardAnim = wizard.GetComponent<Animator>();
    }

    public void AddForceToWizard(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void SetGravityWizard(float scale)
    {
        rb.gravityScale = scale;
    }

    public void SetWizardDeadAnimation(bool dead)
    {
        wizardAnim.SetBool("Dead", dead);
    }

    public void SetFairyColor(Color color)
    {
        fairySpriteRenderer.color = color;
    }

    public void SetWizardColor(Color color)
    {
        wizardSpriteRenderer.color = color;
    }

    public void SetWizardCollider(bool enabled)
    {
        col.enabled = enabled;
    }

    /// <summary>
    /// Calls reset method in wizard
    /// </summary>
    public void ResetWizard()
    {
        wizardMove.ResetWizard();
    }

    public void ResetFairy()
    {
        fairyMove.ResetFairy();
    }

    /// <summary>
    /// Disables time traversal and freezing mechanic
    /// </summary>
    /// <param name="disable"></param>
    public void DisablePower(bool disable)
    {
        fairyMove.DisablePower(disable);
    }

    /// <summary>
    /// Disables wizard's ability to jump and move
    /// </summary>
    /// <param name="disable"></param>
    public void DisableWizardInput(bool disable)
    {
        wizardMove.DisableInput(disable);
    }

    /// <summary>
    /// Disables fairy movement
    /// </summary>
    /// <param name="disable"></param>
    public void DisableFairyMovement(bool disable)
    {
        fairyMove.DisableMovement(disable);
    }

    /// <summary>
    /// Disables all player input
    /// </summary>
    /// <param name="disable"></param>
    public void DisableAll(bool disable)
    {
        fairyMove.DisableMovement(disable);
        wizardMove.DisableInput(disable);
        fairyMove.DisablePower(disable);
    }

    /// <summary>
    /// Returns if the fairy's power is disabled
    /// </summary>
    /// <returns></returns>
    public bool IsPowerDisabled()
    {
        return fairyMove.IsPowerDisabled();
    }

    /// <summary>
    /// Stops velocity of wizard and fairy.
    /// </summary>
    public void StopAllVelocity()
    {
        wizardMove.StopVelocity();
        fairyMove.StopFairyVelocity();
    }

    /// <summary>
    /// Triggers Fairy Power animation
    /// </summary>
    public void TriggerTimeAnimation()
    {
        fairyAnim.SetTrigger("Power");
    }

    public void TriggerWizardFlyAnimation()
    {
        wizardAnim.SetBool("isJumping", true);
    }

    public void TriggerWizardFallAnimation()
    {
        wizardAnim.SetFloat("velocityY", -1);
    }

    public void ResetWizardAnimation()
    {
        wizardAnim.SetBool("isJumping", false);
        wizardAnim.SetFloat("speed", 0);
    }

    /// <summary>
    /// Resizes wizard
    /// </summary>
    public void SetWizardSize(float x, float y, float z)
    {
        wizardMove.ResizeWizard(x, y, z);
    }

    public void SetWizardPosition(float x, float y, float z)
    {
        wizardMove.RepositionWizard(x, y, z);
    }

    public Vector3 GetWizardPosition()
    {
        return wizard.transform.position;
    }

    public void SetFairySize(float x, float y, float z)
    {
        fairyMove.ResizeFairy(x, y, z);
    }

    public void SetFairyPosition(float x, float y, float z)
    {
        fairyMove.RepositionFairy(x, y, z);
    }

    public void RotateWizard(float degrees)
    {
        wizard.transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
    }

    public void TurnOffGravity(bool turnOff)
    {
        rb.gravityScale = turnOff ? 0 : 1;
    }

    public void FlipWizardSprite(bool flipX)
    {
        wizardSpriteRenderer.flipX = flipX;
    }

    public void ChangeWizardLayer(string layer, int order)
    {
        wizardSpriteRenderer.sortingLayerName = layer;
        wizardSpriteRenderer.sortingOrder = order;
    }

    /// <summary>
    /// Whether to enable or disable fairy power
    /// </summary>
    public void HandlePowerDisabling()
    {
        // Tutorial code (bad code cuz can make it can be more concise)
        if (GameManager.s_level == "Tut" && TutorialManager.s_disablePower || GameManager.s_level == "PreTut")
        {
            DisablePower(true);
        }
        else
        {
            // Enable fairy powers 
            DisablePower(false);
        }
    }

    public void SetFairyGravity(int gravity)
    {
        fairyRb.gravityScale = gravity;
    }

    public void SetFairySprite(Sprite sprite)
    {
        fairySpriteRenderer.sprite = sprite;
    }

    public void EnableFairyAnim(bool enable)
    {
        fairyAnim.enabled = enable;
    }
}
