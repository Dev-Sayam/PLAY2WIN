using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace KheloJeeto
{
    public class UserTicketDetailsButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ticketIdText, drawTimeText, playText, winText, multiplierTxt;

        [SerializeField] private Image betHistoryCardRanks;
        [SerializeField] private Image betHistoryCardSuits;
        [SerializeField] private Sprite jokerSprite, queenSprite, kingSprite;
        [SerializeField] private Sprite heartSprite, spadeSprite, diamondSprite, clubSprite;
        public string ticketID;

        public void SetTicketDetails(string ticketId, string drawTime, string play, string win, string result)
        {
            ticketID = ticketId;
            ticketIdText.text = ticketId;
            drawTimeText.text = drawTime;
            playText.text = play;
            winText.text = win;
            var resultArray = result.Split('-');
            multiplierTxt.text = resultArray[1];
            string resultText = JeetoJokerManager.Instance.GetCardRankAndSuitAccordingToNumber(resultArray[0]);
            SetImageResult(resultText);
        }
        private void SetImageResult(string cardRankAndSuit)
        {
            print("Card Rank and Suit : " + cardRankAndSuit);
            switch (cardRankAndSuit[0])
            {
                case 'J':
                    betHistoryCardRanks.sprite = jokerSprite;
                    break;

                case 'Q':
                    betHistoryCardRanks.sprite = queenSprite;
                    break;

                case 'K':
                    betHistoryCardRanks.sprite = kingSprite;
                    break;

                default:
                    break;
            }

            switch (cardRankAndSuit[1])
            {
                case 'H':
                    betHistoryCardSuits.sprite = heartSprite;
                    break;

                case 'S':
                    betHistoryCardSuits.sprite = spadeSprite;
                    break;

                case 'D':
                    betHistoryCardSuits.sprite = diamondSprite;
                    break;

                case 'C':
                    betHistoryCardSuits.sprite = clubSprite;
                    break;

                default:
                    break;
            }
        }
    }
}
