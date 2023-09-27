using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using static BookTicketDetails;

public class Spin2WinGameManager : BaseGameManager
{
	public static Spin2WinGameManager Instance;

	[HideInInspector] public int selectedCoinAmt;

	[SerializeField] private TextMeshProUGUI balanceText;

	[SerializeField] private string bookTicketUrl;
	[SerializeField] private TextMeshProUGUI userId, pointsText;
	[SerializeField] private TextMeshProUGUI gameIdText;
	[SerializeField] private NumberButton[] numberButtons;
	[SerializeField] private Button oddButton, evenButton, repeatButton, clearButton, doubleButton;
	[SerializeField] private TextMeshProUGUI[] historyResultsText;
	[SerializeField] private TextMeshProUGUI playText;
	[SerializeField] private TextMeshProUGUI winPopupText;
	[SerializeField] private TextMeshProUGUI winText;

	public Transform BetGridT;
	private int balance;
	private int originalBalance;

	[SerializeField] Timer timer;
	public UnityEvent OnWin;
	private int totalPointsSpent;

	private string gameId = "01";

	public int TotalPointsSpent
	{
		get { return totalPointsSpent; }
		set
		{
			totalPointsSpent = value;
			playText.SetText(totalPointsSpent.ToString());
		}
	}


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		Application.runInBackground = true;
	}

	public void DisableNumberButtons()
	{
		foreach (var item in numberButtons)
		{
			item.SwitchButtonInteraction(false);
		}

		oddButton.interactable = false;
		evenButton.interactable = false;
		clearButton.interactable = false;
		repeatButton.interactable = false;
		doubleButton.interactable = false;
	}

	public void EnableNumberButtons()
	{
		foreach (var item in numberButtons)
		{
			item.SwitchButtonInteraction(true);
		}

		oddButton.interactable = true;
		evenButton.interactable = true;
		clearButton.interactable = true;
		repeatButton.interactable = true;
		doubleButton.interactable = true;
	}

	public void RestartGame()
	{
		Invoke("SendPendingDrawDetailsSpin2Win", 4f);
		Invoke("GetLastFewDrawDetailsSpin2Win", 4f);
		Invoke("OnPressedClear", 4f);
	}

	// Start is called before the first frame update
	void Start()
	{
		userId.SetText(mainData.receivedData.UserID);
		balance = int.Parse(mainData.receivedData.Balance);
		selectedCoinAmt = 2;
		originalBalance = balance;
		ShowBalance();
		GetLastFewDrawDetailsSpin2Win();
		SendPendingDrawDetailsSpin2Win();
	}

	private void GetLastFewDrawDetailsSpin2Win()
	{
		base.GetLastFewDrawDetails(gameId, 10, (result) =>
		{
			mainData.lastFewDrawDetails = JsonUtility.FromJson<CurrentDrawDetails>(result);

			for (int i = 0; i < mainData.lastFewDrawDetails.Draws.Length; i++)
			{
				if (i == 0)
				{
					timer.SetLastDrawTime(mainData.lastFewDrawDetails.Draws[i].DrawTime);
				}

				historyResultsText[i].text = mainData.lastFewDrawDetails.Draws[i].Result[1].ToString();
				string multiplier = mainData.lastFewDrawDetails.Draws[i].XF;

				if (multiplier.Equals("1X") == false)
				{
					historyResultsText[i].text += "\n" + multiplier;
				}
			}
		});
	}

	private void SendPendingDrawDetailsSpin2Win()
	{
		base.SendPendingDrawDetails(gameId, (result) =>
		{
			mainData.pendingDrawDetails = JsonUtility.FromJson<PendingDrawDetails>(result);
			gameIdText.text = mainData.pendingDrawDetails.Draws[0].GID;
			ProcessTimer();
		});
	}

	void ProcessTimer()
	{
		var timeOfDay = Convert.ToDateTime(mainData.pendingDrawDetails.Now).TimeOfDay;
		var drawTime = Convert.ToDateTime(mainData.pendingDrawDetails.Draws[0].DrawTime).TimeOfDay;

		timer.SetCurrentDrawTime(drawTime);
		var remainingTime = drawTime.Subtract(timeOfDay);
		print(remainingTime);
		timer.RunTimer((float)remainingTime.TotalSeconds);
	}

	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			SendPendingDrawDetailsSpin2Win();
		}
	}

	public void GetCurrentDrawDetailsResultSpin2Win()
	{
		base.GetCurrentDrawDetailsResult(gameId, (result) =>
		{
			mainData.currentDrawDetails = JsonUtility.FromJson<CurrentDrawDetails>(result);

			var gameResult = int.Parse(mainData.currentDrawDetails.Draws[0].Result);
			var multiplier = int.Parse(mainData.currentDrawDetails.Draws[0].XF[0].ToString());

			bool win = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin) > 0;

			SpinWheel.Instance.SetupResults(gameResult, multiplier, win, () =>
			{
				OnWin?.Invoke();
				balance = originalBalance = int.Parse(mainData.currentDrawDetails.Balance);
				winText.text = winPopupText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
			});
		});
	}

	public bool DebitBalance(int amount)
	{
		if (balance - amount >= 0)
		{
			balance -= amount;
			ShowBalance();
			return true;
		}
		else
		{
			return false;
		}
	}

	public void ShowBalance()
	{
		balanceText.SetText(balance.ToString());
		pointsText.SetText(balance.ToString());
	}

	public void SelectCoinType(int coinAmt)
	{
		selectedCoinAmt = coinAmt;
	}

	public void OnPressedDouble()
	{
		for (int i = 0; i < BetGridT.childCount; i++)
		{
			BetGridT.GetChild(i).GetComponent<NumberButton>().DoubleAmount();
		}
	}

	public void OnPressedClear()
	{
		for (int i = 0; i < BetGridT.childCount; i++)
		{
			BetGridT.GetChild(i).GetComponent<NumberButton>().Clear();
		}

		balance = originalBalance;
		ShowBalance();
		TotalPointsSpent = 0;
	}

	public void OnPressedOdds()
	{
		for (int i = 0; i < BetGridT.childCount; i += 2)
		{
			BetGridT.GetChild(i).GetComponent<NumberButton>().OnPressedButton(selectedCoinAmt);
		}
	}

	public void OnPressedEven()
	{
		for (int i = 1; i < BetGridT.childCount; i += 2)
		{
			BetGridT.GetChild(i).GetComponent<NumberButton>().OnPressedButton(selectedCoinAmt);
		}

	}

	public void OnPressedRepeat()
	{
		for (int i = 0; i < BetGridT.childCount; i++)
		{
			var numberButton = BetGridT.GetChild(i).GetComponent<NumberButton>();
			numberButton.OnPressedButton(numberButton.LastStoredAmount);
		}
	}

	[ContextMenu("Book Ticket")]
	public void BookTicket()
	{
		StartCoroutine(BookTicketCoroutine());
	}

	IEnumerator BookTicketCoroutine()
	{
		List<BetsDetails> betsDetails = new List<BetsDetails>();

		foreach (var item in numberButtons)
		{
			if (item.Amount != 0)
			{
				betsDetails.Add(new BetsDetails(item.Number, item.Amount.ToString(), ""));
			}
		}

		var bookTicketDetails = new BookTicketDetails(
			mainData.receivedData.UserID,
			mainData.pendingDrawDetails.GameID,
			mainData.pendingDrawDetails.Draws[0].DrawTime,
			betsDetails.ToArray()
			);

		var bookTicketDetailsJson = JsonUtility.ToJson(bookTicketDetails);
		print(bookTicketDetailsJson);
		UnityWebRequest www = UnityWebRequest.Post(bookTicketUrl, bookTicketDetailsJson);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(bookTicketDetailsJson);
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
			Debug.Log(www.downloadHandler.text);
			JObject bookTicketJson = JObject.Parse(www.downloadHandler.text);
			int currentBalance = (int)bookTicketJson["Balance"];
			mainData.receivedData.Balance = currentBalance.ToString();
			balance = originalBalance = currentBalance;
			www.downloadHandler.Dispose();
		}
	}
}
