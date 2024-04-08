using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Slope stuff: https://www.youtube.com/watch?v=QPiZSTEuZnw
/// Controls the movement of the wizard.
/// <br></br>
/// Authors:
/// Cynthia Wang: Ground checker of wizard
/// Kristen Lee: Everything else
/// </summary>
public class WizardMovement : MonoBehaviour
{
    // Movement variables
    private float wizardMovement;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpingPower;
    private bool isFacingRight = true;

    /// <summary>
    /// True if wizard is on ground layer
    /// </summary>
    private bool isGrounded;

    /// <summary>
    /// True if wizard is currently jumping
    /// </summary>
    private bool isJumping;

    /// <summary>
    /// True if wizard is falling
    /// </summary>
    private bool isFalling;

    // --------------- Physics --------------
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D wizardMaterial;

    // ----------------------- Slope Stuff -----------------------

    /// <summary>
    /// How far to check if on a slope
    /// </summary>
    [SerializeField] private float slopeCheckDistance;

    /// <summary>
    /// Max slope angle wizard can climb
    /// </summary>
    [SerializeField] private float maxSlopeAngle;

    /// <summary>
    /// True if wizard can walk on the slope
    /// </summary>
    private bool canWalkOnSlope;

    /// <summary>
    /// True if wizard is on a slope
    /// </summary>
    private bool isOnSlope;

    /// <summary>
    /// The angle from the bottom of the wizard to the ground
    /// </summary>
    private float slopeDownAngle;

    /// <summary>
    /// The angle from the bottom of the the wizard to the front of the wizard
    /// </summary>
    private float slopeSideAngle;

    /// <summary>
    /// Don't know what this is for
    /// </summary>
    private float lastSlopeAngle;

    /// <summary>
    /// New velocity for wizard
    /// </summary>
    private Vector2 newVelocity;

    /// <summary>
    /// For jumping
    /// </summary>
    private Vector2 newForce;

    private bool isNextToWall;

    // ---------------- Components ---------------------
    private CapsuleCollider2D collider;
    private Rigidbody2D rbWizard;
    private Vector2 colliderSize;
    private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private SpriteRenderer sr;

    /// <summary>
    /// Not sure
    /// </summary>
    private Vector2 slopeNormalPerp;

    // ------------- Fall Damage ------------

    /// <summary>
    /// Position of the object the wizard was last at
    /// </summary>
    private float lastY;

    /// <summary>
    /// Max distance wizard can fall before dying from fall damage
    /// </summary>
    private readonly float maxFallDistance = 3.5f;

    private bool dead = false;

    // ------------- Other ------------
    /// <summary>
    /// True if fairy is freezing wizard
    /// </summary>
    private bool isFrozen;

    /// <summary>
    /// When set to true, wizard won't be able to move or interact
    /// </summary>
    private bool disableInput = false;

    /// <summary>
    /// Holds the object the wizard can interact with
    /// </summary>
    private IInteractable interactObject;

    /// <summary>
    /// When testing relocation logic, adding this caused the bug to go away
    /// </summary>
    private readonly List<Collider2D> collidedObjects = new();

    private void Start()
    {
        // Initiate Variables
        collider = GetComponent<CapsuleCollider2D>();
        rbWizard = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        colliderSize = collider.size;

        lastY = groundCheck.position.y;
    }

    // Called once per frame
    void Update()
    {
        if (!disableInput)
        {
            // Update if wizard is being frozen
            isFrozen = FreezeManager.Instance.IsPreservingWizard();

            if (isFrozen)
            {
                wizardMovement = 0.0f;
                rbWizard.velocity = Vector2.zero;
                collider.enabled = false;
            }
            else if (!collider.enabled)
            {
                collider.enabled = true;
            }
            else
            {
                CheckInput();

                // Boundaries
                CheckBoundaries();

                // Interactable objects
                if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.Interact]) && interactObject != null)
                {
                    interactObject.Interact();
                }
            }
        }
    }

    // Called 50 times per second
    private void FixedUpdate()
    {
        if (!disableInput)
        {
            GameManager.wizardCollisions = collidedObjects.Count;
            collidedObjects.Clear();
            CheckGrounded();
            CheckFallDamage();
            CheckNextToWall();

            if (dead)
            {
                LevelManager.Instance.ResetLevel(false);
            }
            else
            {
                SlopeCheck();
                ApplyMovement();
            }
        }
    }

    /// <summary>
    /// Movement Input
    /// </summary>
    private void CheckInput()
    {
        // --------------------- Movement Input ---------------------------

        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.WizardLeft]))
        {
            wizardMovement = -1.0f;
            Flip();
        }

        else if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.WizardLeft]))
        {
            wizardMovement = 0.0f;
        }

        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.WizardRight]))
        {
            wizardMovement = 1.0f;
            Flip();
        }

        else if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.WizardRight]))
        {
            wizardMovement = 0.0f;
        }

        animator.SetFloat("speed", Mathf.Abs(wizardMovement));
        animator.SetFloat("velocityY", Mathf.Approximately(Mathf.Abs(rbWizard.velocity.y), 0.0f) ? 0.0f : rbWizard.velocity.y);

        // Handles jumping
        if (Input.GetKeyDown(GameManager.s_keyBinds[GameManager.KeyBind.WizardJump]) && !isFrozen)
        {
            Jump();
        }
    }

    /// <summary>
    /// Stops wizard from going past boundary
    /// </summary>
    private void CheckBoundaries()
    {
        if (transform.position.x < GameManager.s_boundaryLeft)
        {
            Vector3 newPos = new(GameManager.s_boundaryLeft, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        else if (transform.position.x > GameManager.s_boundaryRight)
        {
            Vector3 newPos = new(GameManager.s_boundaryRight, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        if (transform.position.y > GameManager.s_boundaryUp)
        {
            Vector3 newPos = new(transform.position.x, GameManager.s_boundaryUp, transform.position.z);
            transform.position = newPos;
        }
    }

    // Checks if player is on the ground
    private void CheckGrounded()
    {
        // Check if on ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (rbWizard.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        isFalling = rbWizard.velocity.y < 0.0f;

        if (isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", false);
        }

    }

    private void CheckNextToWall()
    {
        isNextToWall = Physics2D.OverlapCircle(wallCheck.position, 0.5f, wallLayer);
    }

    private void CheckFallDamage()
    {
        if (isGrounded)
        {
            // Check if dead from fall damage
            if (Mathf.Abs(lastY - groundCheck.position.y) > maxFallDistance)
            {
                dead = true;
            }

            lastY = groundCheck.position.y;
        }
    }

    /// <summary>
    /// Checks if there is a slope in front or wizard is on the slope
    /// </summary>
    private void SlopeCheck()
    {
        //collider.bounds.center
        // Get position at the feet of the wizard
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y / 11.02f));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    /// <summary>
    /// Check if there is a slope to the front of the wizard
    /// </summary>
    /// <param name="checkPos"></param>
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

        Vector2 front = isFacingRight ? transform.right : -transform.right;

        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, front, slopeCheckDistance, groundLayer);

        if (slopeHitFront || isNextToWall)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    /// <summary>
    /// Check if wizard is on a slope
    /// </summary>
    /// <param name="checkPos"></param>
    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle || isNextToWall)
        {
            canWalkOnSlope = false;
            rbWizard.sharedMaterial = noFriction;
        }
        else
        {
            canWalkOnSlope = true;
            rbWizard.sharedMaterial = wizardMaterial;
        }
    }

    /// <summary>
    ///  Actually move the wizard
    /// </summary>
    private void ApplyMovement()
    {
        if (!isFrozen)
        {
            // !isOnSlope && !isJumping
            if (isGrounded && !isOnSlope && !isJumping) //if not on slope
            {
                newVelocity.Set(movementSpeed * wizardMovement, 0.0f);
                rbWizard.velocity = newVelocity;
            }
            else if (isGrounded && isOnSlope && canWalkOnSlope) //If on slope
            {
                newVelocity.Set(movementSpeed * slopeNormalPerp.x * -wizardMovement, movementSpeed * slopeNormalPerp.y * -wizardMovement);
                rbWizard.velocity = newVelocity;
            }
            else if (!isGrounded) //If in air
            {
                newVelocity.Set(movementSpeed * wizardMovement, rbWizard.velocity.y);
                rbWizard.velocity = newVelocity;
            }
        }

    }

    /// <summary>
    /// Make the wizard jump
    /// </summary>
    private void Jump()
    {
        // && slopeDownAngle <= maxSlopeAngle
        if (isGrounded && !isJumping && !isNextToWall)
        {
            AudioManager.Instance.PlayJumpSound();
            animator.SetBool("isJumping", true);
            isFalling = true;
            isGrounded = false;
            isJumping = true;
            newVelocity.Set(0.0f, 0.0f);
            rbWizard.velocity = newVelocity;
            newForce.Set(0.0f, jumpingPower);
            rbWizard.AddForce(newForce, ForceMode2D.Impulse);
        }
    }

    // Handles flipping sprite
    private void Flip()
    {
        if (isFacingRight && wizardMovement < 0f || !isFacingRight && wizardMovement > 0f)
        {
            isFacingRight = !isFacingRight;
            sr.flipX = !sr.flipX;
        }
    }

    // ------------------ Interactable Objects --------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!disableInput)
        {
            if (collision.gameObject.TryGetComponent(out IInteractable interactableObject))
            {
                interactObject = interactableObject;
                interactableObject.ShowInteractable();
            }

            if (!collidedObjects.Contains(collision))
            {
                collidedObjects.Add(collision);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable interactableObject))
        {
            interactObject = null;
            interactableObject.RemoveInteractable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!disableInput)
        {
            if (!collidedObjects.Contains(collision.collider))
            {
                collidedObjects.Add(collision.collider);
            }
        }
    }

    // Also add colliders during collision stay
    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    public void DisableInput(bool disable)
    {
        disableInput = disable;
    }

    public bool InputDisabled()
    {
        return disableInput;
    }

    /// <summary>
    /// Resets wizard everything
    /// </summary>
    public void ResetWizard()
    {
        dead = false;
        isGrounded = true;
        isNextToWall = false;
        isOnSlope = false;
        isJumping = false;
        wizardMovement = 0.0f;
        isFrozen = false;
        rbWizard.velocity = Vector2.zero;
        lastY = transform.position.y; // Probably will cause some bugs
        transform.eulerAngles = new(0, 0, 0);
    }

    public void StopVelocity()
    {
        rbWizard.velocity = Vector2.zero;
        animator.SetFloat("speed", 0);
        wizardMovement = 0.0f;
    }

    public void ResizeWizard(float x, float y, float z)
    {
        gameObject.transform.localScale = new Vector3(x, y, z);
    }

    public void RepositionWizard(float x, float y, float z)
    {
        lastY = y;
        wizardMovement = 0.0f;
        gameObject.transform.position = new Vector3(x, y, z);
        isFacingRight = true;
        sr.flipX = true;
    }

    private void OnWizardFreeze()
    {
        lastY = transform.position.y;
    }

    private void OnEnable()
    {
        FreezeManager.OnObjectFreeze += OnWizardFreeze;
    }

    private void OnDisable()
    {
        FreezeManager.OnObjectFreeze -= OnWizardFreeze;
    }
}