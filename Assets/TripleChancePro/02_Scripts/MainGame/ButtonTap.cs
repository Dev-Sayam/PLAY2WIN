using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace TripleChanceProTimer
{
    public class ButtonTap : MonoBehaviour
    {
        public Image img;
        public TextMeshProUGUI text1, text2;
        public RectTransform button_trans, text;
        [SerializeField] private GameObject select_tab;
        public TextMeshProUGUI bet_text;
        private List<int> allPlacedBets = new List<int>();
        [HideInInspector] public List<int> allPlacedBetsForRebetUse = new List<int>();
        //funtion use on Button Click reference ditrect go to butto on click event
        public void OnBtnClick()
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            TripleChanceManger.instence.WinText.text = "0";
            if (!TripleChanceManger.instence.isRemoveButtonEnabled)
            {
                SoundManager.instance.PlaySfx(6);
                int bet = TripleChanceManger.instence.currentBetnumber;
                if (!CanPlaceBet()) return;
                if (bet > TripleChanceManger.instence.balanceAmount)
                {
                    TripleChanceManger.instence.NoEnoughMoneyAlertPopUp();
                    return;
                }
                TripleChanceManger.instence.AllThreeButtonEnableDisable(true);
                select_tab.SetActive(true);
                bet_text.text = (int.Parse(bet_text.text) + bet).ToString();
                if (!TripleChanceManger.instence.allbetButtons.Contains(this))
                {
                    TripleChanceManger.instence.allbetButtons.Add(this);
                }
                allPlacedBets.Add(bet);
                TripleChanceManger.instence.panelSlide.selectedSliderBarId = text1.text.Length - 1;///////////////////////////////////////////////////
                TripleChanceManger.instence.panelSlide.UpdateTotalBetText(bet);
            }
            else if (allPlacedBets.Count > 0)
            {
                if (allPlacedBets.Count == 1)
                {
                    select_tab.SetActive(false);
                    if (TripleChanceManger.instence.allbetButtons.Contains(this))
                    {
                        SoundManager.instance.PlaySfx(5);
                        TripleChanceManger.instence.allbetButtons.Remove(this);
                        if (TripleChanceManger.instence.allbetButtons.Count <= 0)
                        {
                            TripleChanceManger.instence.EnableLastBetButton();
                            TripleChanceManger.instence.DeselectRemoveButton();
                            TripleChanceManger.instence.AllThreeButtonEnableDisable(false);
                        }
                    }
                }
                bet_text.text = (int.Parse(bet_text.text) - allPlacedBets[allPlacedBets.Count - 1]).ToString();
                TripleChanceManger.instence.panelSlide.selectedSliderBarId = text1.text.Length - 1;///////////////////////////////////////////////////
                TripleChanceManger.instence.panelSlide.UpdateTotalBetText(-allPlacedBets[allPlacedBets.Count - 1]);
                allPlacedBets.RemoveAt(allPlacedBets.Count - 1);
            }

        }
        public void DoDoubleBet(int doubleBetMultiplier)
        {
            int bet = int.Parse(bet_text.text);
            bet_text.text = (bet * doubleBetMultiplier).ToString();
            allPlacedBets.Add(bet);
            TripleChanceManger.instence.panelSlide.selectedSliderBarId = text1.text.Length - 1;///////////////////////////////////////////////////
            TripleChanceManger.instence.panelSlide.UpdateTotalBetText(bet);
        }
        public void OnBtnReset()
        {
            ClearBet();
            if (TripleChanceManger.instence.allbetButtons.Contains(this))
            {
                TripleChanceManger.instence.allbetButtons.Remove(this);
            }
        }
        public void ClearBet(bool setBalanceAmount = true)
        {
            allPlacedBets.Clear();
            select_tab.SetActive(false);
            TripleChanceManger.instence.panelSlide.selectedSliderBarId = text1.text.Length - 1;///////////////////////////////////////////////////
            TripleChanceManger.instence.panelSlide.UpdateTotalBetText(-int.Parse(bet_text.text), setBalanceAmount);
            bet_text.text = "0";
        }
        public bool CanPlaceBet(bool isForDoubleBet = false)
        {
            if (isForDoubleBet)
            {
                if (int.Parse(bet_text.text) * 2 <= Constant.HIGHEST_BET_AMOUNT)
                {
                    return true;
                }
            }
            else if (int.Parse(bet_text.text) + TripleChanceManger.instence.currentBetnumber <= Constant.HIGHEST_BET_AMOUNT)
            {
                return true;
            }
            TripleChanceManger.instence.ShowHighestBetAlertPopUp();
            return false;
        }
        public void SetAllPlacedBetsForRebetUse()
        {
            allPlacedBetsForRebetUse = new List<int>(allPlacedBets);
            allPlacedBets.Clear();
        }
    }
}
