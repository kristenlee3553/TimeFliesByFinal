using UnityEngine;

/// <summary>
/// For dialogues. Create in Assets like any other GameObject.
/// Edit dialogues in the inspector.
/// <br></br>
/// Author: Kristen Lee
/// </summary>
[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    public MyDialogue[] dialogue;
}

[System.Serializable]
public struct MyDialogue
{
    [TextArea]
    public string dialogue;

    /// <summary>
    /// Use Wizard, Fairy, TimeLord
    /// </summary>
    public string name;

    /// <summary>
    /// Where dialogue will run if dialogue is skipped
    /// </summary>
    public bool checkpoint;
}
