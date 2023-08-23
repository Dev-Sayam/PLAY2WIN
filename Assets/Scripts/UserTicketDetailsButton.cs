using TMPro;
using UnityEngine;

public class UserTicketDetailsButton : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI ticketIdText, drawTimeText, playText, winText, resultText;

	public void SetTicketDetails(string ticketId, string drawTime, string play, string win, string result)
	{
		ticketIdText.text = ticketId;
		drawTimeText.text = drawTime;
		playText.text = play;
		winText.text = win;
		resultText.text = result;
	}
}
