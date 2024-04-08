using UnityEngine;

/// <summary>
/// Changes cursor to hand
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] private Texture2D cursorHand;
    public void OnCursorEnter()
    {
        Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.Auto);
    }

    public void OnCursorExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
