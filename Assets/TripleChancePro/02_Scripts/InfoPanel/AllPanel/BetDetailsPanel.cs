using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class BetDetailsPanel : MonoBehaviour
    {
        [SerializeField] private BetDeailsPrefabData betDeailsPrefabData;
        private ViewTicketDetails_sendData viewTicketDetails_SendData = new ViewTicketDetails_sendData();
        private void OnEnable()
        {
            TripleChanceManger.instence.loadingPanel.SetActive(true);
            viewTicketDetails_SendData.TicketID = TripleChanceManger.instence.selectedTicketIdInHistoryPanel;
            API_Manager.instance.ViewTicketDetails(viewTicketDetails_SendData, (OnSuccessData) =>
            {
                betDeailsPrefabData.claim.text = OnSuccessData.Status;
                betDeailsPrefabData.play.text = OnSuccessData.Points;
                betDeailsPrefabData.win.text = OnSuccessData.Win;
                TripleChanceManger.instence.loadingPanel.SetActive(false);
            }, (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
            });
        }
    }
}
