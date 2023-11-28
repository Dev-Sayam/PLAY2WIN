
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    [SerializeField] private RectTransform rouletteGames;
    [SerializeField] private RectTransform drawGames;
    [SerializeField] private Image roulette;
    [SerializeField] private Image draw;
    private Color deSelectedColor = new Color(0.9882f, 0.8941f, 0.5215f);


    private void Start()
    {
        roulette.color=Color.white;
        draw.color = deSelectedColor;
    }
    public void ToggleRoulette()
    {
        rouletteGames.gameObject.SetActive(false);
        drawGames.gameObject.SetActive(true);
        draw.color = Color.white;
        roulette.color = deSelectedColor;
    }

    public void ToggleDraw()
    {
        rouletteGames.gameObject.SetActive(true);
        drawGames.gameObject.SetActive(false);
        draw.color = deSelectedColor;
        roulette.color = Color.white;
    }
}
