using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace TripleChanceProTimer
{
    public class HistoryPrefabData : MonoBehaviour
    {
        public TextMeshProUGUI sNo;
        public TextMeshProUGUI result;
        public TextMeshProUGUI drawTime;
        public TextMeshProUGUI play;
        public TextMeshProUGUI win;
        public TextMeshProUGUI ticketTime;
        public TextMeshProUGUI claim;
        [HideInInspector] public string ticketId;
        public void OnClick()
        {
            TripleChanceManger.instence.selectedTicketIdInHistoryPanel = ticketId;
        }
    }
}
