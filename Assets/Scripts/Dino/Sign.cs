using UnityEngine;

/// <summary>
/// Shows fall damage warning
/// <br></br>
/// Author: Kristen Lee
/// </summary>
public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject text;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        text.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        text.SetActive(false);
    }
}
