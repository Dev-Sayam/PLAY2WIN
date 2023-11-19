
using UnityEngine;
using UnityEngine.UI;

public class ScrollButton : MonoBehaviour
{
    [SerializeField]private ScrollRect scrollRect;
    [SerializeField]private float scrollAmount = 100f;
    [SerializeField] private GameObject rouletteGamesRef;


    /// <summary>
    /// For Scrolling left.
    /// called in the left button click events.
    /// </summary>
    public void OnLeftButtonClick()
    {
        // Scroll the content to the left by a set amount
        if (rouletteGamesRef.activeSelf)
        {
            scrollRect.content.localPosition += new Vector3(scrollAmount, 0f, 0f);
        }
    }

    /// <summary>
    /// For Scrolling Right.
    /// called in the right button click events.
    /// </summary>
    public void OnRightButtonClick()
    {
        // Scroll the content to the right by a set amount
        if (rouletteGamesRef.activeSelf)
        {
            scrollRect.content.localPosition -= new Vector3(scrollAmount, 0f, 0f);
        }
    }
}