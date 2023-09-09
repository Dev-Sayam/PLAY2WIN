using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    [System.Serializable]
    public class RetrievesLast_n_number_of_results_sendData
    {
        public string GameID;
        public string Limit;
        public string Status;
        public string UserID;
        public string AutoClaim;
    }
    [System.Serializable]
    public class Bet
    {
        public string Digit;
        public string Qty;
        public string SDigit;
        public string Win;
    }
    [System.Serializable]
    public class BookTicket_sendData
    {
        public string UserID;
        public string GameID;
        public string Draw;
        public List<Bet> Bets;
    }
    [System.Serializable]
    public class ViewTicket_sendData
    {
        public string UserID;
        public string GameID;
        public string Filter;
        public string Limit;
    }
    [System.Serializable]
    public class ViewTicketDetails_sendData
    {
        public string TicketID;
    }
    [System.Serializable]
    public class Report_sendData
    {
        public string UserID;
        public string FromDate;
        public string ToDate;
    }
    [System.Serializable]
    public class Result_sendData
    {
        public string GameID;
        public string Date;
    }
}

