using UnityEngine;

/// <summary>
/// Controls the movement of the fairy.
/// <br></br>
/// Authors: 
/// <br></br>
/// Cynthia Wang: WASD and flipping of fairy
/// <br></br>
/// Kristen Lee: Everything else
/// </summary>
public class FairyMovement : MonoBehaviour
{
    // Movement variables
    public float movementSpeed = 5f;
    private Vector2 fairyMovement;

    // Component variables
    [SerializeField] private Rigidbody2D rbFairy;

    /// <summary>
    /// When set to true, will not be able to move.
    /// </summary>
    private bool disableMovement = false;

    /// <summary>
    /// When set to true, will not be able to move and use powers
    /// </summary>
    private bool disablePower = false;

    private SpriteRenderer sprite;
    private Animator animator;

    bool isFacingRight = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Called once per frame
    void Update()
    {
        if (!IsMovementDisabled())
        {
            // Default is no movement
            fairyMovement = Vector2.zero;

            CheckFairyInput();

            if (FreezeManager.Instance.IsPreserving())
            {
                fairyMovement = Vector2.zero;
            }

            // Boundaries
            CheckBoundaries();

            Flip();
        }
    }

    // Called 50 times per second
    private void FixedUpdate()
    {
        if (!IsMovementDisabled())
        {
            // Update fairy movement
            rbFairy.velocity = movementSpeed * fairyMovement;
        }
    }

    private void CheckFairyInput()
    {
        // Handles fairy inputs
        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyLeft]))
        {
            fairyMovement = Vector2.left;
        }
        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyRight]))
        {
            fairyMovement = Vector2.right;
        }
        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyUp]))
        {
            fairyMovement = Vector2.up;
        }
        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyDown]))
        {
            fairyMovement = Vector2.down;
        }
    }

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
        else if (transform.position.y < GameManager.s_boundaryDown)
        {
            Vector3 newPos = new(transform.position.x, GameManager.s_boundaryDown, transform.position.z);
            transform.position = newPos;
        }
    }

    private void Flip()
    {
        if (isFacingRight && fairyMovement.Equals(Vector2.left) || !isFacingRight && fairyMovement.Equals(Vector2.right))
        {
            isFacingRight = !isFacingRight;
            sprite.flipX = !sprite.flipX;
        }
    }

    /// <summary>
    /// Sets velocity of fairy to 0. Resets animation and preservation variables.
    /// </summary>
    public void ResetFairy()
    {
        rbFairy.velocity = Vector2.zero;
        FreezeManager.Instance.ResetManager();
    }

    /// <summary>
    /// When set to true, will not be able to move.
    /// </summary>
    public void DisableMovement(bool disable)
    {
        disableMovement = disable;
    }

    /// <summary>
    /// When set to true, will not be able to use powers
    /// </summary>
    public void DisablePower(bool disable)
    {
        disablePower = disable;
    }

    /// <summary>
    /// If true, will not be able to move.
    /// </summary>
    public bool IsMovementDisabled()
    {
        return disableMovement;
    }

    /// <summary>
    /// if true, will not be able use powers
    /// </summary>
    public bool IsPowerDisabled()
    {
        return disablePower;
    }

    /// <summary>
    /// Sets velocity to 0
    /// </summary>
    public void StopFairyVelocity()
    {
        rbFairy.velocity = Vector2.zero;
    }

    public void RepositionFairy(float x, float y, float z)
    {
        gameObject.transform.position = new Vector3(x, y, z);
    }

    public void ResizeFairy(float x, float y, float z)
    {
        gameObject.transform.localScale = new Vector3(x, y, z);
    }
}