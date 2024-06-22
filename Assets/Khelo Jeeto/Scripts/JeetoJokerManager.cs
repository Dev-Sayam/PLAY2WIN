using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static BookTicketDetails;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace KheloJeeto
{
	public class JeetoJokerManager : BaseGameManager
	{
		public static JeetoJokerManager Instance { get; private set; }
		public int SelectedCoinAmt { get => selectedCoinAmt; private set => selectedCoinAmt = value; }
		public int TotalPointsSpent
		{
			get => totalPointsSpent;
			set
			{
				totalPointsSpent = value;
				playText.text = totalPointsSpent.ToString();
				spinPanelPlayText.text = totalPointsSpent.ToString();
			}
		}

		[SerializeField] private Text balanceText;
		[SerializeField] private Text spinPanelBalanceText;
		[SerializeField] private string bookTicketUrl;
		[SerializeField] private Sprite jokerSprite, queenSprite, kingSprite;
		[SerializeField] private Sprite heartSprite, spadeSprite, diamondSprite, clubSprite;
		[SerializeField] private Image[] betHistoryCardRanks;
		[SerializeField] private Image[] betHistoryCardSuits;
		[SerializeField] private Text[] betHistoryMultiplier;
		[SerializeField] private EachCard[] eachCards;
		[SerializeField] private Button clearButton, doubleButton, repeatButton;
		[SerializeField] private Button allHeartsButton, allSpadesButton, allDiamondsButton, allClubsButton;
		[SerializeField] private Button allJokersButton, allQueensButton, allKingsButton;
		[SerializeField] private Timer timer;
		[SerializeField] private Text gameIdText;
		[SerializeField] private Text drawTimeText;
		[SerializeField] private Text spinPanelGameIdText;
		[SerializeField] private Text playText;
		[SerializeField] private Text spinPanelPlayText;
		public Text winText;
		public Text spinPanelWinText;
		public Text spinPanelPopupWinText;
		[SerializeField] private GameObject boardWheelPanel;
		[SerializeField] private GameObject boardObject;
		[SerializeField] private Transform boardHolder;

		private bool isAllButtonDiasabled;

		[SerializeField] private EachCard[] allJokerCards;
		[SerializeField] private EachCard[] allQueenCards;
		[SerializeField] private EachCard[] allKingCards;

		[SerializeField] private EachCard[] allHeartCards;
		[SerializeField] private EachCard[] allSpadeCards;
		[SerializeField] private EachCard[] allDiamondCards;
		[SerializeField] private EachCard[] allClubCards;

		[SerializeField] private EachCard[] allCards;
		[SerializeField] private int[] randomCardCount;
		[SerializeField] private MessageView messageView;
		[Header("Sound Handler")]
		[SerializeField] private Image soundImg;
		[SerializeField] private Sprite[] soundSprite;
		public bool isMusicSoundEnabled;
		public readonly string AudioPlayerPrefs = "AudioEnabled";//0= off 1= on

		private int selectedCoinAmt = 2;
		private string gameId = "16";
		private int balance;
		private int originalBalance;
		private int totalPointsSpent;
		public UnityEvent OnWin;
		[SerializeField] private GameObject backPopUp;

        [SerializeField] private WheelController wheelController;
        public 
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
			if (!PlayerPrefs.HasKey(AudioPlayerPrefs))
			{
				PlayerPrefs.SetInt(AudioPlayerPrefs, 1);
			}
			isMusicSoundEnabled = PlayerPrefs.GetInt(AudioPlayerPrefs) == 1;
			/*if (isMusicSoundEnabled)
				soundImg.sprite = soundSprite[1];
			else soundImg.sprite = soundSprite[0];*/
			if (isMusicSoundEnabled)
				soundImg.color = Color.white;
			else soundImg.color = Color.gray;
			Application.runInBackground = true;
		}

		// Start is called before the first frame update
		void Start()
		{
			//ShowResults();
			
			balance = int.Parse(mainData.receivedLoginData.Balance);
			originalBalance = balance;
			ShowBalance();
			GetLastFewDrawDetailsJeetoJoker();
			SendPendingDrawDetailsJeetoJoker();

        }

		public void SelectCoin(int amt)
		{
			SoundManager.instance.PlayButtonSound();
			SelectedCoinAmt = amt;
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (!backPopUp.activeInHierarchy)
				{
					backPopUp.SetActive(true);
				}
			}
		}

		public void ChangeAudioEnableDisable()
		{
			isMusicSoundEnabled = !isMusicSoundEnabled;
			if (isMusicSoundEnabled)
			{
				PlayerPrefs.SetInt(AudioPlayerPrefs, 1);
				soundImg.color = Color.white;
			}
			else
			{
				PlayerPrefs.SetInt(AudioPlayerPrefs, 0);
				soundImg.color = Color.grey;

			}

		}

		private void SendPendingDrawDetailsJeetoJoker()
		{
			base.SendPendingDrawDetails(gameId, (result) =>
			{
				mainData.pendingDrawDetails = JsonUtility.FromJson<PendingDrawDetails>(result);
				gameIdText.text = mainData.pendingDrawDetails.Draws[0].GID;
				drawTimeText.text = mainData.pendingDrawDetails.Draws[0].DrawDate + "  " + mainData.pendingDrawDetails.Draws[0].DrawTime;
				spinPanelGameIdText.text = mainData.pendingDrawDetails.Draws[0].GID;
				ProcessTimer();
			});
		}

		void ProcessTimer()
		{
			

            var timeOfDay = Convert.ToDateTime(mainData.pendingDrawDetails.Now).TimeOfDay;
			var drawTime = Convert.ToDateTime(mainData.pendingDrawDetails.Draws[0].DrawTime).TimeOfDay;
			Debug.Log(drawTime);
			Debug.Log(timeOfDay);
			timer.SetCurrentDrawTime(drawTime);
			var remainingTime = drawTime.Subtract(timeOfDay);
			print(remainingTime);
			if(remainingTime.TotalSeconds>0)
			timer.RunTimer((float)remainingTime.TotalSeconds);
		}

		private void GetLastFewDrawDetailsJeetoJoker()
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

					string xFVal = mainData.lastFewDrawDetails.Draws[i].XF;
					if (xFVal == "N" || xFVal.ToLower() == "1x")
						betHistoryMultiplier[i].transform.parent.gameObject.SetActive(false);
					else
					{
						betHistoryMultiplier[i].transform.parent.gameObject.SetActive(true);
						betHistoryMultiplier[i].text = xFVal.ToLower();
					}

					string cardRankAndSuit = GetCardRankAndSuitAccordingToNumber(mainData.lastFewDrawDetails.Draws[i].Result);
					print("Card Rank and Suit : " + cardRankAndSuit);
					switch (cardRankAndSuit[0])
					{
						case 'J':
							betHistoryCardRanks[i].sprite = jokerSprite;
							break;

						case 'Q':
							betHistoryCardRanks[i].sprite = queenSprite;
							break;

						case 'K':
							betHistoryCardRanks[i].sprite = kingSprite;
							break;

						default:
							break;
					}

					switch (cardRankAndSuit[1])
					{
						case 'H':
							betHistoryCardSuits[i].sprite = heartSprite;
							break;

						case 'S':
							betHistoryCardSuits[i].sprite = spadeSprite;
							break;

						case 'D':
							betHistoryCardSuits[i].sprite = diamondSprite;
							break;

						case 'C':
							betHistoryCardSuits[i].sprite = clubSprite;
							break;

						default:
							break;
					}
				}

			});
		}

		public string GetCardRankAndSuitAccordingToNumber(string result)
		{
			string cardRankAndSuit;

			switch (result)
			{
				case "00":
					cardRankAndSuit = "JH";
					break;

				case "01":
					cardRankAndSuit = "JS";
					break;

				case "02":
					cardRankAndSuit = "JD";
					break;

				case "03":
					cardRankAndSuit = "JC";
					break;

				case "04":
					cardRankAndSuit = "QH";
					break;

				case "05":
					cardRankAndSuit = "QS";
					break;

				case "06":
					cardRankAndSuit = "QD";
					break;

				case "07":
					cardRankAndSuit = "QC";
					break;

				case "08":
					cardRankAndSuit = "KH";
					break;

				case "09":
					cardRankAndSuit = "KS";
					break;

				case "10":
					cardRankAndSuit = "KD";
					break;

				case "11":
					cardRankAndSuit = "KC";
					break;

				default:
					cardRankAndSuit = "";
					break;
			}

			return cardRankAndSuit;
		}

		public void EnableCardButtons()
		{
			foreach (var eachCard in eachCards)
			{
				eachCard.SwitchButtonInteraction(true);
			}

			clearButton.interactable = true;
			doubleButton.interactable = true;
			repeatButton.interactable = true;

			allClubsButton.interactable = true;
			allDiamondsButton.interactable = true;
			allHeartsButton.interactable = true;
			allSpadesButton.interactable = true;

			allJokersButton.interactable = true;
			allKingsButton.interactable = true;
			allQueensButton.interactable = true;
			isAllButtonDiasabled = false;
		}

		public void DisableCardButtons()
		{
			foreach (var eachCard in eachCards)
			{
				eachCard.SwitchButtonInteraction(false);
			}
			isAllButtonDiasabled = true;

			clearButton.interactable = false;
			doubleButton.interactable = false;
			repeatButton.interactable = false;

			allClubsButton.interactable = false;
			allDiamondsButton.interactable = false;
			allHeartsButton.interactable = false;
			allSpadesButton.interactable = false;

			allJokersButton.interactable = false;
			allKingsButton.interactable = false;
			allQueensButton.interactable = false;
		}

		public void BetOnAllJokerCards()
		{
			foreach (var eachCard in allJokerCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		public void BetOnAllQueenCards()
		{
			foreach (var eachCard in allQueenCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		public void BetOnAllKingCards()
		{
			foreach (var eachCard in allKingCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		public void BetOnAllHearts()
		{
			foreach (var eachCard in allHeartCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		public void BetOnAllSpades()
		{
			foreach (var eachCard in allSpadeCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		public void BetOnAllDiamonds()
		{
			foreach (var eachCard in allDiamondCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		public void BetOnAllClubs()
		{
			foreach (var eachCard in allClubCards)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}
		#region Pick and Place Random Card Bet
		public void OnRandomButtonClick(int id)
		{
			if (isAllButtonDiasabled) return;

			int count = randomCardCount[id];
			if (GetTotalAmountToClearOnRandomButtonClick(count))
			{
				Debug.Log("<color =red>Not enough points</color>");
				messageView.ShowMsg("Not enough balance");
				return;
			}
			OnPressedClear();
			PickRandomCards(count);


		}
		private bool GetTotalAmountToClearOnRandomButtonClick(int count)
		{
			int totalBet = selectedCoinAmt * count;
			Debug.Log(totalBet);
			if (balance - totalBet > 0)
			{
				return false;
			}
			else return true;

		}
		private void PickRandomCards(int count)
		{
			List<EachCard> eachCards = new List<EachCard>(allCards);
			List<EachCard> tempcard = new List<EachCard>();
			for (int i = 0; i < count; i++)
			{
				int randIndex = UnityEngine.Random.Range(0, eachCards.Count);
				tempcard.Add(eachCards[randIndex]);
				eachCards.RemoveAt(randIndex);
			}
			foreach (var eachCard in tempcard)
			{
				eachCard.PlaceBetOnCard(SelectedCoinAmt);
			}
		}

		#endregion Pick and Place Random Card Bet
		public void GetCurrentDrawDetailsResultJeetoJoker()
		{
			base.GetCurrentDrawDetailsResult(gameId, (result) =>
			{
				mainData.currentDrawDetails = JsonUtility.FromJson<CurrentDrawDetails>(result);

				var gameResult = int.Parse(mainData.currentDrawDetails.Draws[0].Result);
				var multiplier = int.Parse(mainData.currentDrawDetails.Draws[0].XF[0].ToString());

				AppearWheelBoard();
				if (string.IsNullOrEmpty((mainData.currentDrawDetails.Draws[0].TotWin)))
					mainData.currentDrawDetails.Draws[0].TotWin = "0";

                bool win = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin) > 0;
				wheelController.PlaceBet(gameResult, multiplier, win, () =>
                {
                    OnWin?.Invoke();
                    balance = originalBalance = int.Parse(mainData.currentDrawDetails.Balance);
                    winText.text = spinPanelWinText.text = spinPanelPopupWinText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
                    //	ShowResults();
                    //OnWin?.Invoke();
                    //balance = originalBalance = int.Parse(mainData.currentDrawDetails.Balance);
                    //winText.text = winPopupText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
                },
                () =>
                {
                    winText.text = spinPanelWinText.text = spinPanelPopupWinText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
                }
                );

              /*  SpinWheel.Instance.SetupResults(gameResult, multiplier, win, () =>
				{
					OnWin?.Invoke();
					balance = originalBalance = int.Parse(mainData.currentDrawDetails.Balance);
					winText.text = spinPanelWinText.text = spinPanelPopupWinText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
					//	ShowResults();
					//OnWin?.Invoke();
					//balance = originalBalance = int.Parse(mainData.currentDrawDetails.Balance);
					//winText.text = winPopupText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
				},
				() =>
				{
					winText.text = spinPanelWinText.text = spinPanelPopupWinText.text = int.Parse(mainData.currentDrawDetails.Draws[0].TotWin).ToString();
				}
				);*/
			});
		}

		public void OnPressedClear()
		{
			foreach (EachCard eachCard in eachCards)
			{
				eachCard.ClearCard();
			}

			balance = originalBalance;
			ShowBalance();
			TotalPointsSpent = 0;
		}

		public void RestartGame()
		{
			Invoke(nameof(SendPendingDrawDetailsJeetoJoker), 3f);
			Invoke(nameof(GetLastFewDrawDetailsJeetoJoker), 3f);
			Invoke(nameof(OnPressedClear), 3f);
		}

		public void OnPressedDouble()
		{
			foreach (EachCard eachCard in eachCards)
			{
				eachCard.DoubleBet();
			}
		}

		public void OnPressedRepeat()
		{
			foreach (EachCard eachCard in eachCards)
			{
				eachCard.PlaceBetOnCard(eachCard.LastStoredAmount);
			}
		}

		public void ShowBalance()
		{
			balanceText.text = balance.ToString();
			spinPanelBalanceText.text = balance.ToString();
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

		public void ShowResults()
		{
			//GameObject duplicatedBoard = Instantiate(boardObject, boardHolder);
			//duplicatedBoard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			//duplicatedBoard.transform.localScale = Vector3.one * 1.8f;
			AppearWheelBoard();
		}
	

		[ContextMenu("please show")]
		private void AppearWheelBoard()
		{
			Debug.LogError("Calling for show");
			boardWheelPanel.transform.DOLocalMoveX(0f, 1.25f).SetDelay(2).OnComplete(() => wheelController.StartRotation());
			
		}

		public void DisappearWheelBoard()
		{
			boardWheelPanel.transform.DOLocalMoveX(-2755f, 1.25f);
		}


		public void BookTicket()
		{
			StartCoroutine(BookTicketCoroutine());
		}

		IEnumerator BookTicketCoroutine()
		{
			List<BetsDetails> betsDetails = new List<BetsDetails>();

			foreach (var item in allCards)
			{
				betsDetails.Add(new BetsDetails(item.Number, item.CardAmt.ToString(), ""));
			}


			var bookTicketDetails = new BookTicketDetails(
				mainData.receivedLoginData.UserID,
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
				mainData.receivedLoginData.Balance = currentBalance.ToString();
				balance = originalBalance = currentBalance;
				www.downloadHandler.Dispose();
			}
		}
		public void ShowWinAmount(string winAmount)
		{
			winText.text = winAmount;
			spinPanelWinText.text = winAmount;
		}
		public void OnyesBtnClick()
		{
			SceneManager.LoadScene("LobbyScene");
		}

		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				SendPendingDrawDetailsJeetoJoker();
			}
		}
	}
}
