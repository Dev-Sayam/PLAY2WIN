using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace TripleChanceProTimer
{
    public class TripleChanceManger : MonoBehaviour
    {
        public static TripleChanceManger instence;
        [SerializeField] private TextMeshProUGUI userIdText, drawTimeText, gIdText;
        [SerializeField] private WheelController wheelController;
        [SerializeField] private TextMeshProUGUI time_Text, last_Chance_text;
        [SerializeField] private Animator messageAnim;
        [SerializeField] private int total_time = 90;
        private int temp_time;
        [SerializeField] private List<GameObject> allBetButton;
        private int? selectedBetButtonId = null;
      /*  [HideInInspector]*/ public int currentBetnumber;
        [HideInInspector] public List<ButtonTap> allbetButtons;
        private List<ButtonTap> allbetButtonsForRebetUse = new List<ButtonTap>();
        public PanelSlide panelSlide;
        [SerializeField] private TripleBoardSelecter tripleBoardSelecter;
        [SerializeField] private TripleBoardSelecter[] tripleBoardSelecters;
        [HideInInspector] public bool isRemoveButtonEnabled;
        [SerializeField] private Button doubleBtn, clearBtn, removeBtn, infoBtn;
        [SerializeField] private Animator doubleBtnAnim, clearBtnAnim, removeBtnAnim, infoBtnAnim;
        public bool isAllButtonDiasabled;
        [SerializeField] private GameObject infoPanel;
        public GameObject loadingPanel, satrtingLoadingPanel;
        [SerializeField] private GameObject muxBetReachedPopUp;
        [SerializeField] private GameObject notEnoughPointsPopUp;
        [SerializeField] private TextMeshProUGUI balance_Text;
        [SerializeField] private TextMeshProUGUI totalPlayAmountText;
        public int totalBetAmount;
        public float balanceAmount;
        [SerializeField] private TextMeshProUGUI[] allWinNumberText;
        [SerializeField] private TextMeshProUGUI[] allWinMultiplierText;
        public TextMeshProUGUI WinText;
        [HideInInspector]public float winAmount;
         public string userId = "U002";
        [HideInInspector] public string gameId = "03";
        private RetrievesLast_n_number_of_results_response retrievesLast_N_Number_Of_Results_Response_P = new RetrievesLast_n_number_of_results_response();
        private RetrievesLast_n_number_of_results_response retrievesLast_N_Number_Of_Results_Response_D = new RetrievesLast_n_number_of_results_response();
        [SerializeField] private SliderBar[] allSlideBarAscending;
        [HideInInspector] public string selectedTicketIdInHistoryPanel;
        [SerializeField] private GameObject backPopUp;
        [SerializeField] Color[] time_textColor;
        [Space(20)]
        [SerializeField] private MainData mainData;
        private void Awake()
        {
            if (instence == null)
            {
                instence = this;
            }
            else
            {
                Debug.LogError("TripleChanceManger double instances");
                Destroy(this.gameObject);
            }
        }
        void Update()
        {
            // Make sure user is on Android platform
            if (Application.platform == RuntimePlatform.Android)
            {
                // Check if Back was pressed this frame
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!backPopUp.activeInHierarchy)
                    {
                        backPopUp.SetActive(true);
                    }
                }
            }
        }
        private void Start()
        {
            userId =mainData.receivedLoginData.UserID;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            userIdText.text = userId;
            RetriveResults((OnSuccessData) =>
            {
                retrievesLast_N_Number_Of_Results_Response_D = OnSuccessData;
                balanceAmount = retrievesLast_N_Number_Of_Results_Response_D.Balance;
                BalanceAmountSet(balanceAmount);
                if (allWinNumberText.Length > 0)
                {
                    wheelController.SetFixedRotationForFirstTime(int.Parse(retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Result));
                }
                SetResultText(false);
                RetriveResults((OnSuccessData1) =>
                {
                    retrievesLast_N_Number_Of_Results_Response_P = OnSuccessData1;
                    string[] time = retrievesLast_N_Number_Of_Results_Response_P.Draws[0].DrawTime.Split(':');
                    string[] date = retrievesLast_N_Number_Of_Results_Response_P.Draws[0].DrawDate.Split('-');
                    DateTime dateTime = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), 0);
                  
                    DateTime dateTime1;
                    DateTime.TryParse(retrievesLast_N_Number_Of_Results_Response_P.Now, out dateTime1);
                    total_time = (int)(dateTime - dateTime1).TotalSeconds;
                  
                    Debug.LogError("system" + DateTime.Now);
                    Debug.LogError("api" + dateTime);
                    Debug.LogError(total_time);

                    drawTimeText.text = dateTime.ToString("hh:mm tt");
                    gIdText.text = OnSuccessData1.Draws[0].GID;
                    satrtingLoadingPanel.SetActive(false);
                    TotalBetAmountSet();
                    BetButtonClick(0);
                    TimerStart();
                    AllThreeButtonEnableDisable(false, false);
                }, "P");
            }, "D");
        }
        private void RetriveResults(System.Action<RetrievesLast_n_number_of_results_response> OnComplete, string status)
        {
            RetrievesLast_n_number_of_results_sendData retrievesLast_N_Number_Of_Results_SendData = new RetrievesLast_n_number_of_results_sendData();
            retrievesLast_N_Number_Of_Results_SendData.GameID = gameId;
            retrievesLast_N_Number_Of_Results_SendData.Limit = "10";
            retrievesLast_N_Number_Of_Results_SendData.Status = status;
            retrievesLast_N_Number_Of_Results_SendData.UserID = userId;
            retrievesLast_N_Number_Of_Results_SendData.AutoClaim = "N";
            API_Manager.instance.RetrievesLast_n_number_of_results(retrievesLast_N_Number_Of_Results_SendData, (OnSuccessData) =>
            {
                OnComplete.Invoke(OnSuccessData);
            }, (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
            });
        }
        private void TimerStart()
        {
            time_Text.text = total_time.ToString();
            StartCoroutine(Delay_Time());
        }
        IEnumerator Delay_Time()
        {
            time_Text.color = time_textColor[0];
            ClearBet(false);
            isAllButtonDiasabled = false;
            temp_time = total_time;
            while (temp_time >= 0)
            {
                if (temp_time < 10)
                {
                    time_Text.text = "0" + temp_time.ToString();
                    
                }
                else
                {
                    time_Text.text = temp_time.ToString();
                }
                if (temp_time == total_time)
                {
                    SoundManager.instance.PlaySfx(0);
                    MessageTextShow(Constant.PLACE_YOUR_CHIPS);
                    Invoke(nameof(DisappearMessageText), Constant.MESSAGE_TEXT_DISAPPEAR_DELAY);
                }
                yield return new WaitForSecondsRealtime(1);
                temp_time--;
                if (temp_time == Constant.LAST_CHANCE_TIME)
                {
                    SoundManager.instance.PlaySfx(1);
                    Debug.Log("15 second left");
                    MessageTextShow(Constant.LAST_CHANCE);
                    Invoke(nameof(DisappearMessageText), Constant.MESSAGE_TEXT_DISAPPEAR_DELAY);
                }
                if (temp_time <= Constant.NO_MORE_BET_TIME && !isAllButtonDiasabled)
                {
                    time_Text.color = time_textColor[1];
                    SoundManager.instance.PlaySfx(2);
                    Debug.Log("10 second left");
                    MessageTextShow(Constant.BET_HAVE_NOT_BEEN_ACCEPTED);
                    Invoke(nameof(DisappearMessageText), Constant.MESSAGE_TEXT_DISAPPEAR_DELAY);
                    OnTimer10SecondReached();
                }
            }
            yield return new WaitForSecondsRealtime(Constant.TIMER_RECURSION_DELAY);
            RetriveResults((OnSuccessData) =>
            {
                retrievesLast_N_Number_Of_Results_Response_P = OnSuccessData;
                string[] time = OnSuccessData.Draws[0].DrawTime.Split(':');
                string[] date = OnSuccessData.Draws[0].DrawDate.Split('-');
                System.DateTime dateTime = new System.DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), 0);

                DateTime dateTime1;
                DateTime.TryParse(retrievesLast_N_Number_Of_Results_Response_P.Now, out dateTime1);
                total_time = (int)(dateTime - dateTime1).TotalSeconds;

                drawTimeText.text = dateTime.ToString("hh:mm tt");
                gIdText.text = OnSuccessData.Draws[0].GID;
                StartCoroutine(Delay_Time());
            }, "P");
        }

        private void MessageTextShow(string message)
        {
            messageAnim.Play("message");
            last_Chance_text.color = Color.red;
            last_Chance_text.text = message;
        }
        public void BetButtonClick(int id)
        {
            if (isAllButtonDiasabled) return;
            if (selectedBetButtonId != null)
            {
                allBetButton[(int)selectedBetButtonId].transform.GetChild(0).gameObject.SetActive(false);
            }
            selectedBetButtonId = id;
            allBetButton[id].transform.GetChild(0).gameObject.SetActive(true);
            currentBetnumber = int.Parse(allBetButton[id].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
            if (!(totalBetAmount <= 0))
            {
                DeselectRemoveButton();
            }
        }
        public void EnableLastBetButton()
        {
            allBetButton[(int)selectedBetButtonId].transform.GetChild(0).gameObject.SetActive(true);
        }
        public void DoDoubleBet()
        {
            if (totalBetAmount * 2 > balanceAmount)
            {
                NoEnoughMoneyAlertPopUp();
                return;
            }
            for (int i = 0; i < allbetButtons.Count; i++)
            {
                if (!allbetButtons[i].CanPlaceBet(true))
                {
                    return;
                }
            }
            for (int i = 0; i < allbetButtons.Count; i++)
            {
                allbetButtons[i].DoDoubleBet(2);
            }
            DeselectRemoveButton();
        }
        public void ClearBet(bool setBalanceAmount = true)
        {
            SoundManager.instance.PlaySfx(5);
            panelSlide.CloseAllSlider();
            foreach (ButtonTap item in allbetButtons)
            {
                item.ClearBet(setBalanceAmount);
            }
            allbetButtons.Clear();
            for (int i = 0; i < tripleBoardSelecters.Length; i++)
            {
                tripleBoardSelecters[i].DeselectRandomButton();
            }
            DeselectRemoveButton();
            AllThreeButtonEnableDisable(false);
        }
        private void ClearAllBetTextIncludingBetButtons()
        {
            panelSlide.CloseAllSlider();
            AllThreeButtonEnableDisable(false,false);

        }
        public void OnRemoveButtonClick()
        {
            isRemoveButtonEnabled = true;
            DeSelectBetButton();
            removeBtn.interactable = false;
            removeBtnAnim.Rebind();
            removeBtnAnim.enabled = false;
        }
        public void DeselectRemoveButton()
        {
            isRemoveButtonEnabled = false;
            removeBtn.interactable = true;
            removeBtnAnim.enabled = true;
        }
        private void DeSelectBetButton()
        {
            if (selectedBetButtonId != null)
            {
                allBetButton[(int)selectedBetButtonId].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        public void AllThreeButtonEnableDisable(bool active,bool activeRebet=true)
        {
            doubleBtn.interactable = active;
            doubleBtnAnim.Rebind();
            doubleBtnAnim.enabled = active;
            clearBtn.interactable = active;
            clearBtnAnim.Rebind();
            clearBtnAnim.enabled = active;
            removeBtn.interactable = active;
            removeBtnAnim.Rebind();
            removeBtnAnim.enabled = active;
            doubleBtn.onClick.RemoveAllListeners();
            if (active)
            {
                doubleBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Constant.DOUBLE;
                doubleBtn.onClick.AddListener(DoDoubleBet);
            }
            else if(activeRebet)
            {
                doubleBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Constant.REBET;
                if (allbetButtonsForRebetUse.Count > 0)
                {
                    doubleBtn.onClick.AddListener(DoRebet);
                    doubleBtn.interactable = true;
                    doubleBtnAnim.enabled = true;
                }
            }
        }
        private void DisappearMessageText()
        {
            last_Chance_text.text = "";
        }
        private void OnTimer10SecondReached()
        {
            isAllButtonDiasabled = true;
            ClearAllBetTextIncludingBetButtons();
            OnInfoCloseButtonClick();
            InfoButtonEnableDisable(false);
            BookTicket(() =>
            {
                SaveDataForRebet();
                Invoke(nameof(GetResults), Constant.NO_MORE_BET_TIME);
            });
        }
        private void GetResults()
        {
            SoundManager.instance.PlaySfx(4);
            RetriveResults((OnSuccessData) =>
            {
                retrievesLast_N_Number_Of_Results_Response_D = OnSuccessData;
                balanceAmount = retrievesLast_N_Number_Of_Results_Response_D.Balance;
                winAmount = OnSuccessData.Draws[0].TotWin;
                Debug.LogError("W " + winAmount);
                Debug.LogError("B " + balanceAmount);
                if (retrievesLast_N_Number_Of_Results_Response_D.Draws[0].XF == "N")
                {

                    wheelController.multiplierObjId = 0;
                }
                else
                {
                    wheelController.multiplierObjId = int.Parse(retrievesLast_N_Number_Of_Results_Response_D.Draws[0].XF.TrimEnd('X'));
                }
                wheelController.PlaceBet(int.Parse(retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Result));
            }, "D");
        }
        private void BookTicket(System.Action OnComplete)
        {
            BookTicket_sendData bookTicket_Send = new BookTicket_sendData();
            bookTicket_Send.UserID = userId;
            bookTicket_Send.GameID = gameId;
            bookTicket_Send.Draw = retrievesLast_N_Number_Of_Results_Response_P.Draws[0].DrawTime;
            bookTicket_Send.Bets = new List<Bet>();
            for (int i = 0; i < allbetButtons.Count; i++)
            {
                Bet bet = new Bet();
                bet.Digit = allbetButtons[i].text1.text;
                bet.Qty = allbetButtons[i].bet_text.text;
                bet.SDigit = allbetButtons[i].text1.text.Length.ToString();
                bookTicket_Send.Bets.Add(bet);
            }
            API_Manager.instance.BookTicket(bookTicket_Send, (OnSuccessData) =>
            {
                Debug.Log("skjncjsdcnsjc    " + OnSuccessData.Balance);
                Debug.Log("BookTicketSucessFullyDone");
                OnComplete.Invoke();
            }, (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
                OnyesBtnClick();
            });
        }
        public void OnInfoButtonTap()
        {
            infoPanel.SetActive(true);
        }
        public void OnInfoCloseButtonClick()
        {
            infoPanel.SetActive(false);
        }
        public void InfoButtonEnableDisable(bool enanble)
        {
            infoBtn.interactable = enanble;
            infoBtnAnim.Rebind();
            infoBtnAnim.enabled = enanble;
        }
        public void NoEnoughMoneyAlertPopUp()
        {
            notEnoughPointsPopUp.SetActive(true);
        }
        public void ShowHighestBetAlertPopUp()
        {
            muxBetReachedPopUp.SetActive(true);
        }
        public void BalanceAmountSet(float balance)
        {
            Debug.Log("BT " + balance);
            balance_Text.text = balance.ToString();
        }
        public void TotalBetAmountSet()
        {
            totalPlayAmountText.text = totalBetAmount.ToString();
        }
        public void OnWheelRotateComplete()
        {
            SetResultText();
            WinText.text = winAmount.ToString();
            if (winAmount > 0)
            {
                SoundManager.instance.PlaySfx(3);
                MessageTextShow(Constant.YOU_WIN);
                last_Chance_text.color = Color.white;
                Invoke(nameof(DisappearMessageText), Constant.MESSAGE_TEXT_DISAPPEAR_DELAY);
            }
            else
            {
                SoundManager.instance.PlaySfx(7, 0.5f);
            }
        }
        private void SetResultText(bool ShowWinInSlideBar = true)
        {
            BalanceAmountSet(balanceAmount);
            for (int i = 0; i < retrievesLast_N_Number_Of_Results_Response_D.Draws.Count; i++)
            {
                allWinNumberText[i].text = retrievesLast_N_Number_Of_Results_Response_D.Draws[i].Result;
                string xFVal = retrievesLast_N_Number_Of_Results_Response_D.Draws[i].XF;
                allWinMultiplierText[i].text = xFVal == "N" ? xFVal : xFVal.ToLower();
            }
            wheelController.winShowText.text = retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Result;
            if (ShowWinInSlideBar && retrievesLast_N_Number_Of_Results_Response_D != null && retrievesLast_N_Number_Of_Results_Response_D.Draws.Count > 0)
            {
                if (retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Win_1 != null)
                    allSlideBarAscending[0].UpdateWinBetText(float.Parse(retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Win_1));
                allSlideBarAscending[1].UpdateWinBetText(retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Win_2);
                allSlideBarAscending[2].UpdateWinBetText(retrievesLast_N_Number_Of_Results_Response_D.Draws[0].Win_3);
            }
        }
        private void SaveDataForRebet()
        {
            allbetButtonsForRebetUse = new List<ButtonTap>(allbetButtons);
            for (int i = 0; i < allbetButtonsForRebetUse.Count; i++)
            {
                allbetButtonsForRebetUse[i].SetAllPlacedBetsForRebetUse();
            }
        }
        public void DoRebet()
        {
            int tmpCurrentBetnumber = currentBetnumber;
            for (int i = 0; i < allbetButtonsForRebetUse.Count; i++)
            {
                for (int j = 0; j < allbetButtonsForRebetUse[i].allPlacedBetsForRebetUse.Count; j++)
                {
                    currentBetnumber = allbetButtonsForRebetUse[i].allPlacedBetsForRebetUse[j];
                    allbetButtonsForRebetUse[i].OnBtnClick();
                }
            }
            currentBetnumber = tmpCurrentBetnumber;
        }
        public void OnyesBtnClick()
        {
            SceneManager.LoadScene("LobbyScene");
        }

    }
}
