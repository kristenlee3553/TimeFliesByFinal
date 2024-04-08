using UnityEngine;
using System;

/// <summary>
/// This script is placed in a empty object in the GameScene.
/// <br></br>
/// It manages the preservation of objects
/// </summary>
public class FreezeManager : MonoBehaviour
{
    /// <summary>
    ///  Holds the object that is near the fairy that can be preserved
    /// </summary>
    private GameObject preservableObject = null;

    /// <summary>
    /// Object fairy is preserving
    /// </summary>
    private GameObject preservedObject = null;

    /// <summary>
    /// If wizard is frozen
    /// </summary>
    private bool isPreservingWizard = false;

    [SerializeField] private GameObject fairy;
    private Animator animator;

    // SHOULD HAVE USED THIS EARLIER -> Too scared to rewrite all the code
    public static Action OnObjectRelease;
    public static Action OnObjectFreeze;

    public static FreezeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        // Script will last between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        animator = fairy.GetComponent<Animator>();   
    }

    public void SetHoldingAnimation(bool hold)
    {
        animator.SetBool("Holding", hold);
    }

    public void ResetManager()
    {
        SetPreservableObject(null);
        SetPreservedObject(null);
        SetPreservingWizard(false);
        SetHoldingAnimation(false);
        OnObjectRelease?.Invoke();
    }

    /// <summary>
    /// Returns if the fairy can preserve an object
    /// </summary>
    /// <returns></returns>
    public bool CanPreserve()
    {
        return preservableObject != null;
    }

    /// <summary>
    /// Returns if the fairy is preserving an object
    /// </summary>
    /// <returns></returns>
    public bool IsPreserving()
    {
        return preservedObject != null;
    }

    /// <summary>
    /// Returns true if preserved object is wizard
    /// </summary>
    /// <returns></returns>
    public bool IsPreservingWizard()
    {
        return isPreservingWizard;
    }

    /// <summary>
    /// Set if the wizard is being preserved
    /// </summary>
    /// <param name="isFrozen"></param>
    public void SetPreservingWizard(bool isFrozen)
    {
        isPreservingWizard = isFrozen;
    }

    /// <summary>
    /// Set object fairy COULD preserve right now
    /// </summary>
    /// <param name="new_object"></param>
    public void SetPreservableObject(GameObject newObject)
    {
        preservableObject = newObject;
    }

    /// <summary>
    /// Returns object fairy could be preserving
    /// </summary>
    /// <returns></returns>
    public GameObject GetPreservableObject()
    {
        return preservableObject;
    }

    /// <summary>
    /// Sets object fairy is currently preserving
    /// </summary>
    /// <param name="new_object"></param>
    public void SetPreservedObject(GameObject newobject)
    {
        preservedObject = newobject;
    }

    /// <summary>
    /// Returns object fairy is preserving
    /// </summary>
    /// <returns></returns>
    public GameObject GetPreservedObject()
    {
        return preservedObject;
    }

    /// <summary>
    /// Unfreezes object in preserved field and sets the preservable field to the preserved object
    /// </summary>
    public void ReleaseFreezedObject()
    {
        // Stop animation
        SetHoldingAnimation(false);

        GameObject preservedObject = GetPreservedObject();

        // Wizard specific
        if (preservedObject.CompareTag("Wizard"))
        {
            SetPreservingWizard(false);
            CharacterManager.Instance.SetGravityWizard(1);
        }

        // Update objects
        SetPreservableObject(preservedObject);
        SetPreservedObject(null);

        // Show prompt
        preservedObject.GetComponent<PreservableObject>().ShowPrompt(true);

        OnObjectRelease?.Invoke();
    }

    /// <summary>
    /// Freezes object stored in preservable field
    /// </summary>
    public void FreezeObject()
    {
        // Set hold animation
        SetHoldingAnimation(true);

        GameObject preservable = GetPreservableObject();

        // Wizard specific
        if (preservable.CompareTag("Wizard"))
        {
            SetPreservingWizard(true);
            CharacterManager.Instance.SetGravityWizard(0);

        }

        // Save preserved object
        SetPreservedObject(preservable);

        // Object no longer preservable
        FreezeManager.Instance.SetPreservableObject(null);

        // Hide prompt
        preservable.GetComponent<PreservableObject>().ShowPrompt(false);

        OnObjectFreeze?.Invoke();
    }
}
