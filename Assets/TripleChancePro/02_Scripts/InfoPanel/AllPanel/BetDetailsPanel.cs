using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class BetDetailsPanel : MonoBehaviour
    {
        [SerializeField] private List<string> bettypes;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject betDeailsPrefab;
        private ViewTicketDetails_sendData viewTicketDetails_SendData = new ViewTicketDetails_sendData();
        private void OnEnable()
        {
            ClearContent();
            TripleChanceManger.instence.loadingPanel.SetActive(true);
            viewTicketDetails_SendData.TicketID = TripleChanceManger.instence.selectedTicketIdInHistoryPanel;
            API_Manager.instance.ViewTicketDetails(viewTicketDetails_SendData, (OnSuccessData) =>
            {
                Debug.LogError(OnSuccessData.Bets.Count);
                for (int i = 0; i < OnSuccessData.Bets.Count; i++)
                {
                    BetDeailsPrefabData betDeailsPrefabData = Instantiate(betDeailsPrefab, content).GetComponent<BetDeailsPrefabData>();
                    betDeailsPrefabData.betType.text = bettypes[int.Parse(OnSuccessData.Bets[i].SDigit)];
                    betDeailsPrefabData.digit.text = OnSuccessData.Bets[i].Digit;
                    betDeailsPrefabData.play.text = OnSuccessData.Bets[i].Qty;
                    betDeailsPrefabData.win.text = OnSuccessData.Bets[i].Win;
                }
                TripleChanceManger.instence.loadingPanel.SetActive(false);
            }, (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
            });
        }
        private void ClearContent()
        {
            BetDeailsPrefabData[] betDeailsPrefabList = content.GetComponentsInChildren<BetDeailsPrefabData>();
            for (int i = 0; i < betDeailsPrefabList.Length; i++)
            {
                Destroy(betDeailsPrefabList[i].gameObject);
            }
        }
    }
   
}
