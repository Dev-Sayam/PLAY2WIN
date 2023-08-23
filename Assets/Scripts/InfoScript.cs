using System.Collections;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InfoScript : MonoBehaviour
{
	[SerializeField] private GameObject fromCalenderPrefab, toCalenderPrefab;
	[SerializeField] private Transform calenderParentT;
	[SerializeField] private DatePickerEvent fromdatePickerEvent;
	[SerializeField] private DatePickerEvent todatePickerEvent;
	[SerializeField] private TextMeshProUGUI fromDateText;
	[SerializeField] private TextMeshProUGUI toDateText;
	[SerializeField] private MainData mainData;
	[SerializeField] private GameObject userTicketDetailsButtonPrefab;
	[SerializeField] private Transform historyContent;

	[Header("API Details")]
	[SerializeField] private string ticketDetailsLink;
	[SerializeField] private string userPlayDetailsLink;
	[SerializeField] private string reportDetailsLink;
	[SerializeField] private string resultDetailsLink;

	[Header("UserPlayDetailsForTicket Refs")]
	[SerializeField] GameObject userPlayDetailsBG;
	[SerializeField]
	TextMeshProUGUI ticketIdText, entryDateText, entryTimeText, drawDateText, drawTimeText,
		betText, wonText, resultText;

	[Header("Report Refs")]
	[SerializeField] TextMeshProUGUI userIdText;
	[SerializeField] TextMeshProUGUI saleText;
	[SerializeField] TextMeshProUGUI winText;
	[SerializeField] TextMeshProUGUI commText;
	[SerializeField] TextMeshProUGUI ntpText;
	[SerializeField] TextMeshProUGUI optrText;

	[Header("Result Refs")]
	[SerializeField] GameObject eachResultItemPrefab;
	[SerializeField] Transform resultContent;

	GameObject fromCalenderUI, toCalenderUI;
	string fromDate, toDate;

	// Start is called before the first frame update
	void Start()
	{
		fromDate = fromDateText.text = toDate = toDateText.text = System.DateTime.Now.ToString("yyyy/MM/dd");
	}

	private void OnEnable()
	{
		fromdatePickerEvent.AddObserver(DisableFromCalenderParentAndGetDate);
		todatePickerEvent.AddObserver(DisableToCalenderParentAndGetDate);

		GetTicketDetails();
		GetDateWiseResults();
	}

	public void OpenFromCalender()
	{
		calenderParentT.gameObject.SetActive(true);
		fromCalenderUI = GameObject.Instantiate(fromCalenderPrefab, calenderParentT);
		fromCalenderUI.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
	}

	public void OpenToCalender()
	{
		calenderParentT.gameObject.SetActive(true);
		toCalenderUI = GameObject.Instantiate(toCalenderPrefab, calenderParentT);
		toCalenderUI.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
	}

	private void OnDisable()
	{
		fromdatePickerEvent.RemoveObserver(DisableFromCalenderParentAndGetDate);
		RemoveHistoryItems();
		ResetUserPlayTicketDetails();
	}

	void RemoveHistoryItems()
	{
		for (int i = 0; i < historyContent.childCount; i++)
		{
			Destroy(historyContent.GetChild(i).gameObject);
		}
	}

	public void ResetUserPlayTicketDetails()
	{
		ticketIdText.text = entryDateText.text = entryTimeText.text = drawDateText.text =
			drawTimeText.text = betText.text = wonText.text = resultText.text = "";
	}			

	void DisableFromCalenderParentAndGetDate(string date)
	{
		Destroy(fromCalenderUI);
		calenderParentT.gameObject.SetActive(false);
		fromDate = date;
		fromDateText.SetText(fromDate);
	}

	void DisableToCalenderParentAndGetDate(string date)
	{
		Destroy(toCalenderUI);
		calenderParentT.gameObject.SetActive(false);
		toDate = date;
		toDateText.SetText(toDate);
	}

	public void GetTicketDetails()
	{
		StartCoroutine(GetTicketDetailsCoroutine());
	}

	IEnumerator GetTicketDetailsCoroutine()
	{
		var sendToViewTicketDetails = new SendToViewTicketDetails(mainData.receivedData.UserID, "01", "A");
		var sendToViewTicketDetailsJson = JsonUtility.ToJson(sendToViewTicketDetails);
		print(sendToViewTicketDetailsJson);
		UnityWebRequest www = UnityWebRequest.Post(ticketDetailsLink, sendToViewTicketDetailsJson);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(sendToViewTicketDetailsJson);
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		www.uploadHandler.Dispose();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			print(www.downloadHandler.text);
			var userTicketDetails = new UserTicketDetails();
			userTicketDetails = JsonUtility.FromJson<UserTicketDetails>(www.downloadHandler.text);

			if (userTicketDetails.Tickets != null && userTicketDetails.Tickets.Length != 0)
			{
				foreach (var item in userTicketDetails.Tickets)
				{
					GameObject userTicketDetailsButtonGameObject = Instantiate(userTicketDetailsButtonPrefab, historyContent);
					userTicketDetailsButtonGameObject.GetComponent<UserTicketDetailsButton>().SetTicketDetails(
						item.TicketID,
						item.DrawTime,
						item.Play,
						item.Win,
						item.Result
						);
					var button = userTicketDetailsButtonGameObject.GetComponent<Button>();
					button.onClick.AddListener(() =>
					{
						GetUserPlayDetailsForTicket(item.TicketID);
					});
				}
			}
			www.downloadHandler.Dispose();
		}
	}

	public void GetUserPlayDetailsForTicket(string ticketID)
	{
		userPlayDetailsBG.SetActive(true);
		StartCoroutine(GetUserPlayDetailsForTicketCoroutine(ticketID));
	}

	IEnumerator GetUserPlayDetailsForTicketCoroutine(string ticketID)
	{
		var sendTicketDetails = new SendTicketDetails(mainData.receivedData.UserID, ticketID);
		var sendTicketDetailsJson = JsonUtility.ToJson(sendTicketDetails);
		print(sendTicketDetailsJson);
		UnityWebRequest www = UnityWebRequest.Post(userPlayDetailsLink, sendTicketDetailsJson);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(sendTicketDetailsJson);
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		www.uploadHandler.Dispose();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			print(www.downloadHandler.text);
			var userPlayDetailsForTicket = new UserPlayDetailsForTicket();
			userPlayDetailsForTicket = JsonUtility.FromJson<UserPlayDetailsForTicket>(www.downloadHandler.text);

			ticketIdText.text = userPlayDetailsForTicket.TicketID;
			entryDateText.text = userPlayDetailsForTicket.EntryDate;
			entryTimeText.text = userPlayDetailsForTicket.EntryTime;
			drawDateText.text = userPlayDetailsForTicket.DrawDate;
			drawTimeText.text = userPlayDetailsForTicket.DrawTime;
			betText.text = userPlayDetailsForTicket.Bet;
			wonText.text = userPlayDetailsForTicket.Won;
			resultText.text = userPlayDetailsForTicket.Result;
		}
	}

	public void GetDateWiseResults()
	{
		StartCoroutine(GetDateWiseResultsCoroutine());
	}

	IEnumerator GetDateWiseResultsCoroutine()
	{
		var sendDateWiseResultDetails = new SendDateWiseResultDetails("01", System.DateTime.Now.ToString("yyyy/MM/dd"));
		var sendDateWiseResultJson = JsonUtility.ToJson(sendDateWiseResultDetails);
		print(sendDateWiseResultJson);
		UnityWebRequest www = UnityWebRequest.Post(resultDetailsLink, sendDateWiseResultJson);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(sendDateWiseResultJson);
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		www.uploadHandler.Dispose();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			//print(www.downloadHandler.text);
			var receivedDateWiseResultsDetails = new ReceivedDateWiseResultDetails();
			receivedDateWiseResultsDetails = JsonUtility.FromJson<ReceivedDateWiseResultDetails>(www.downloadHandler.text);

			print(receivedDateWiseResultsDetails.Draws.Length);

			foreach (var item in receivedDateWiseResultsDetails.Draws)
			{
				var eachResultItemObject = GameObject.Instantiate(eachResultItemPrefab, resultContent);
				var eachResultItem = eachResultItemObject.GetComponent<EachResultItem>();
				eachResultItem.SetResultItems(item.GameID, item.DrawTime, item.Result, item.XF);
			}
		}

	}


	public void GetDateWiseReport()
	{
		StartCoroutine(GetDateWiseReportCoroutine());
	}

	IEnumerator GetDateWiseReportCoroutine()
	{
		var sendReportDetails = new SendReportDetails(mainData.receivedData.UserID,
			fromDate, toDate);
		var sendReportDetailsJson = JsonUtility.ToJson(sendReportDetails);
		print(sendReportDetailsJson);
		UnityWebRequest www = UnityWebRequest.Post(reportDetailsLink, sendReportDetailsJson);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(sendReportDetailsJson);
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		www.uploadHandler.Dispose();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			print(www.downloadHandler.text);
			var receivedReportDetails = new ReceivedReportDetails();
			receivedReportDetails = JsonUtility.FromJson<ReceivedReportDetails>(www.downloadHandler.text);

			userIdText.text = receivedReportDetails.UserID;
			saleText.text = receivedReportDetails.Sale;
			winText.text = receivedReportDetails.Win;
			commText.text = receivedReportDetails.Comm;
			ntpText.text = receivedReportDetails.NTP;
			optrText.text = receivedReportDetails.Optr;
		}

	}
}

[System.Serializable]
public class SendDateWiseResultDetails
{
	public string GameID;
	public string Date;

	public SendDateWiseResultDetails(string gameID, string date)
	{
		GameID = gameID;
		Date = date;
	}
}

public class ReceivedDateWiseResultDetails
{
	public string Module;
	public string GCode;
	public string Date;
	public string Now;
	public ResultDraw[] Draws;

	[System.Serializable]
	public class ResultDraw
	{
		public string GameID;
		public string DrawTime;
		public string Result;
		public string XF;
	}
}

[System.Serializable]
public class SendTicketDetails
{
	public string UserID;
	public string TicketID;

	public SendTicketDetails(string userID, string ticketID)
	{
		UserID = userID;
		TicketID = ticketID;
	}
}

[System.Serializable]
public class SendToViewTicketDetails
{
	public string UserID;
	public string GameID;
	public string Filter;

	public SendToViewTicketDetails(string userID, string gameID, string filter)
	{
		UserID = userID;
		GameID = gameID;
		Filter = filter;
	}
}

[System.Serializable]
public class UserTicketDetails
{
	public string Module;
	public string UserID;
	public string GameID;
	public string Filter;
	public string Date;
	public Ticket[] Tickets;

	[System.Serializable]
	public class Ticket
	{
		public string TicketID;
		public string GID;
		public string Play;
		public string Win;
		public string Claim;
		public string Result;
		public string DrawTime;
		public string TicketTime;
	}
}

[System.Serializable]
public class UserPlayDetailsForTicket
{
	public string Module;
	public string TicketID;
	public string Player;
	public string Game;
	public string EntryDate;
	public string EntryTime;
	public string DrawDate;
	public string DrawTime;
	public string Bet;
	public string Won;
	public string Result;
}

[System.Serializable]
public class SendReportDetails
{
	public string UserID;
	public string FromDate;
	public string ToDate;

	public SendReportDetails(string userID, string fromDate, string toDate)
	{
		UserID = userID;
		FromDate = fromDate;
		ToDate = toDate;
	}
}

[System.Serializable]
public class ReceivedReportDetails
{
	public string Module;
	public string UserID;
	public string FromDate;
	public string ToDate;
	public string Sale;
	public string Win;
	public string Comm;
	public string NTP;
	public string Optr;
}


