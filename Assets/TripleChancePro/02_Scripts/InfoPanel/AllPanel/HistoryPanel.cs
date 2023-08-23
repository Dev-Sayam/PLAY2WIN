using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class HistoryPanel : MonoBehaviour
    {
        [SerializeField] private GameObject historyPrefab;
        [SerializeField] private Transform content;
        private ViewTicket_sendData viewTicket_SendData;
        [SerializeField] private GameObject historyDetailsPanel;
        private void Awake()
        {
            viewTicket_SendData = new ViewTicket_sendData();
            viewTicket_SendData.UserID = TripleChanceManger.instence.userId;
            viewTicket_SendData.GameID = TripleChanceManger.instence.gameId;
            viewTicket_SendData.Filter = "Y";
            viewTicket_SendData.Limit = "25";
        }
        private void OnEnable()
        {
            TripleChanceManger.instence.loadingPanel.SetActive(true);
            TripleChanceManger.instence.selectedTicketIdInHistoryPanel = null;
            HistoryPrefabData[] previousHistoryList = content.GetComponentsInChildren<HistoryPrefabData>();
            for (int i = 0; i < previousHistoryList.Length; i++)
            {
                Destroy(previousHistoryList[i].gameObject);
            }
            API_Manager.instance.ViewTicket(viewTicket_SendData, (OnSuccessData) =>
            {
                for (int i = 0; i < OnSuccessData.Tickets.Count; i++)
                {
                    HistoryPrefabData historyPrefabData = Instantiate(historyPrefab, content).GetComponent<HistoryPrefabData>();
                    historyPrefabData.sNo.text = (i + 1).ToString();
                    historyPrefabData.result.text = OnSuccessData.Tickets[i].Result;
                    historyPrefabData.drawTime.text = OnSuccessData.Tickets[i].DrawTime;
                    historyPrefabData.play.text = OnSuccessData.Tickets[i].Play;
                    historyPrefabData.win.text = OnSuccessData.Tickets[i].Win;
                    historyPrefabData.ticketTime.text = OnSuccessData.Tickets[i].TicketTime;
                    historyPrefabData.claim.text = OnSuccessData.Tickets[i].Claim;
                    historyPrefabData.ticketId = OnSuccessData.Tickets[i].TicketID;
                }
                TripleChanceManger.instence.loadingPanel.SetActive(false);
            }, (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
            });
        }
        public void OnHistoryDetailsButtonClick()
        {
            if (TripleChanceManger.instence.selectedTicketIdInHistoryPanel != null)
                historyDetailsPanel.SetActive(true);
        }
    }
}
