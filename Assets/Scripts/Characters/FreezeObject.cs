using UnityEngine;

/// <summary>
/// Script that allows the fairy to freeze an object
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class FreezeObject : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Can use power as long as power is not disabled
        if (!CharacterManager.Instance.IsPowerDisabled())
        {
            // If fairy is already preserving an object and wants to release object
            if (Input.GetKeyUp(KeyCode.Space) && FreezeManager.Instance.IsPreserving())
            {
                FreezeManager.Instance.ReleaseFreezedObject();
            }

            // If Fairy is near object that can be preserved and user chooses to preserve object
            else if (Input.GetKeyUp(KeyCode.Space) && FreezeManager.Instance.CanPreserve())
            {
                FreezeManager.Instance.FreezeObject();
            }
        }
    }
}