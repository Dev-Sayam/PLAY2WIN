using TMPro;
using UnityEngine;

public class EachResultItem : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI gameIdText, drawTimeText, resultText, XFText;

	public void SetResultItems(string gameId, string drawTime, string result, string XF)
	{
		gameIdText.text = gameId;
		drawTimeText.text = drawTime;
		resultText.text = result;
		XFText.text = XF;
	}
}
