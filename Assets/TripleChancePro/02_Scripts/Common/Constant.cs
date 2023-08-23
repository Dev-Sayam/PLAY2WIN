using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class Constant
    {   //string use for bouble button functionality
        public const string REBET = "REBET";
        public const string DOUBLE = "DOUBLE";
        //string use for timer text
        public const string PLACE_YOUR_CHIPS = "Place Your Chips";
        public const string LAST_CHANCE = "Last Chance";
        public const string NO_MORE_BET = "No more bet";
        public const string BET_HAVE_NOT_BEEN_ACCEPTED = "Your bet have not been accepted";
        public const string YOU_WIN = "You Win";


        public const float WHEEL_ROTATION_SET_DELAY = 1;
        public const float TIMER_RECURSION_DELAY = 15;
        public const float LAST_CHANCE_TIME = 15;
        public const float NO_MORE_BET_TIME = 10;
        public const float MESSAGE_TEXT_DISAPPEAR_DELAY = 3;
      //  public const float GET_RESUT_DELAY = 10;

        public const float HIGHEST_BET_AMOUNT = 100;

        public const string HOST = "78.159.103.149/";
        public const string RetrievesLast_n_number_of_results_API = "play2win/drawdtl.php";
        public const string Book_Ticket_API = "play2win/bookticket.php";
        public const string View_Ticket_API = "play2win/viewticket.php";
        public const string View_TicketDetails_API = "play2win/TicketDtls.php";
        public const string ReportDetails_API = "play2win/report.php";
        public const string ResultDetails_API = "play2win/drawdtldt.php";
    }
}