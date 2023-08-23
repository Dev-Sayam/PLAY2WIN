using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace TripleChanceProTimer
{
    public class SliderBar : MonoBehaviour
    {
        [SerializeField] private GameObject total_Bet_select_Img, win_Bet_select_Img;
        [SerializeField] private TextMeshProUGUI total_Bet_Text, win_Bet_Text;
        [HideInInspector] public int totalBet;
        public void UpdateTotalBetText(int betAmount, bool setBalanceAmount)
        {

            totalBet += betAmount;
            TripleChanceManger.instence.totalBetAmount += betAmount;
            TripleChanceManger.instence.TotalBetAmountSet();
            if (setBalanceAmount)
            {
                TripleChanceManger.instence.BalanceAmountSet(TripleChanceManger.instence.balanceAmount -= betAmount);
            }
            if (totalBet <= 0)
            {
                total_Bet_select_Img.SetActive(false);
            }
            else
            {
                total_Bet_select_Img.SetActive(true);
            }
            total_Bet_Text.text = totalBet.ToString();
            if (totalBet == 0)
            {
                ClearBetText();
            }
        }
        public void UpdateWinBetText(float amount)
        {
            if (amount > 0)
            {
                win_Bet_select_Img.SetActive(true);
                win_Bet_Text.text = amount.ToString();
            }
        }
        public void ClearBetText()
        {
            total_Bet_select_Img.SetActive(false);
            win_Bet_select_Img.SetActive(false);
            //total_Bet_Text.text = "0";
            win_Bet_Text.text = "0";
            //totalBet = 0;
        }
    }
}
