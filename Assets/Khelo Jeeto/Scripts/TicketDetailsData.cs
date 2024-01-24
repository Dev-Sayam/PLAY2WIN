using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KheloJeeto
    {


    public class TicketDetailsData : MonoBehaviour
    {

        public TMP_Text betAmountTxt;
        public TMP_Text winAmountTxt;
        public TMP_Text betOnTxt;

        public Bet betData;
      
        public void SetDetailsOFBetData(Bet bet)
        {
            betData = bet;
            betAmountTxt.text = betData.Qty;
             winAmountTxt.text = betData.Win;
            betOnTxt.text = JeetoJokerManager.Instance.GetCardRankAndSuitAccordingToNumber(betData.Digit);
        }

    }
}
