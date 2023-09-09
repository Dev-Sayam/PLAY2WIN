using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    [System.Serializable]
    public class Draw
    {
        public string GID;
        public string DrawDate;
        public string DrawTime;
        public string Status;
        public string Result;
        public string DBL_rslt;
        public string SNG_rslt;
        public string XF;
        public float TotWin;
        public string Win_1;
        public float Win_2;
        public float Win_3;
    }

    [System.Serializable]
    public class RetrievesLast_n_number_of_results_response
    {
        public string retMsg;
        public string retStatus;
        public string Query;
        public string GameID;
        public string Date;
        public string Now;
        public List<Draw> Draws;
        public string AutoClaimed;
        public float Balance;
        public string User;
        public int JSonErrNo;
    }
    [System.Serializable]
    public class BookTicket_response
    {
        public string Module;
        public string UserID;
        public string retMsg;
        public string retStatus;
        public string Ticket;
        public string Points;
        public string Balance;
        public string BookDate;
        public string BookTime;
    }

    [System.Serializable]
    public class ViewTicket_responceData
    {
        public string Module;
        public string UserID;
        public string GameID;
        public string Filter;
        public string Date;
        public List<Ticket> Tickets;
        public string retMsg;
        public string retStatus;
    }
    [System.Serializable]
    public class Ticket
    {
        public string TicketID;
        public string GID;
        public string Play;
        public string Win;
        public string Claim;
        public string Result;
        public string DrawTime;
        public string TicketTime;
    }
    [System.Serializable]
    public class ViewTicketDetails_responceData
    {
        public string Module;
        public string TicketID;
        public string UserID;
        public string EntryDate;
        public string EntryTime;
        public string DrawDate;
        public string DrawTime;
        public string Points;
        public string GameID;
        public string Status;
        public string Win;
        public string ClaimDate;
      //  public string Bets;
        public string retMsg;
        public string retStatus;
        public List<Bet> Bets;
    }

    [System.Serializable]
    public class Report_responceData
    {
        public string Module;
        public string UserID;
        public string FromDate;
        public string ToDate;
        public string Sale;
        public string Win;
        public string Comm;
        public string NTP;
        public string Optr;
    }
    [System.Serializable]
    public class ResultDraw
    {
        public string GameID;
        public string DrawTime;
        public string Result;
        public string DBL_rslt;
        public string SNG_rslt;
        public string XF;
    }
    [System.Serializable]
    public class Result_responceData
    {
        public string Module;
        public string GCode;
        public string Date;
        public string Now;
        public List<ResultDraw> Draws;
        public string retMsg;
        public string retStatus;
    }
}

