using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
namespace TripleChanceProTimer
{
    public class API_Manager : MonoBehaviour
    {
        public static API_Manager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        public void RetrievesLast_n_number_of_results(RetrievesLast_n_number_of_results_sendData retrievesLast_N_Number_Of_Results_SendData, System.Action<RetrievesLast_n_number_of_results_response> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            StartCoroutine(RetrievesLast_n_number_of_results_C(retrievesLast_N_Number_Of_Results_SendData, OnSuccess, OnError));
        }
        IEnumerator RetrievesLast_n_number_of_results_C(RetrievesLast_n_number_of_results_sendData retrievesLast_N_Number_Of_Results_SendData, System.Action<RetrievesLast_n_number_of_results_response> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            var _sendData = JsonUtility.ToJson(retrievesLast_N_Number_Of_Results_SendData);
            string sendData = _sendData;
            Debug.Log(sendData);
            using (UnityWebRequest www = new UnityWebRequest(Constant.HOST + Constant.RetrievesLast_n_number_of_results_API, "POST"))
            {
                www.SetRequestHeader("content-Type", "application/json");
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(sendData);
                www.uploadHandler = new UploadHandlerRaw(rawData);
                www.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Submitting");
                yield return www.SendWebRequest();
                Debug.Log("Submitted");
                if (!string.IsNullOrWhiteSpace(www.error))
                {
                    OnError.Invoke(www);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Submitted Successfully");
                    RetrievesLast_n_number_of_results_response retrievesLast_N_Number_Of_Results_Response = JsonUtility.FromJson<RetrievesLast_n_number_of_results_response>(www.downloadHandler.text);
                    OnSuccess.Invoke(retrievesLast_N_Number_Of_Results_Response);
                }
            }
        }
        public void BookTicket(BookTicket_sendData bookTicket_Send, System.Action<BookTicket_response> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            StartCoroutine(BookTicket_C(bookTicket_Send, OnSuccess, OnError));
        }
        IEnumerator BookTicket_C(BookTicket_sendData bookTicket_Send, System.Action<BookTicket_response> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            var _sendData = JsonUtility.ToJson(bookTicket_Send);
            string sendData = _sendData;
            Debug.Log(sendData);
            using (UnityWebRequest www = new UnityWebRequest(Constant.HOST + Constant.Book_Ticket_API, "POST"))
            {
                www.SetRequestHeader("content-Type", "application/json");
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(sendData);
                www.uploadHandler = new UploadHandlerRaw(rawData);
                www.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Submitting");
                yield return www.SendWebRequest();
                Debug.Log("Submitted");
                if (!string.IsNullOrWhiteSpace(www.error))
                {
                    OnError.Invoke(www);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Submitted Successfully");
                    BookTicket_response bookTicket_Response = JsonUtility.FromJson<BookTicket_response>(www.downloadHandler.text);
                    OnSuccess.Invoke(bookTicket_Response);
                }
            }
        }
        public void ViewTicket(ViewTicket_sendData viewTicket_Send, System.Action<ViewTicket_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            StartCoroutine(ViewTicket_C(viewTicket_Send, OnSuccess, OnError));
        }
        IEnumerator ViewTicket_C(ViewTicket_sendData viewTicket_Send, System.Action<ViewTicket_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            var _sendData = JsonUtility.ToJson(viewTicket_Send);
            string sendData = _sendData;
            Debug.Log(sendData);
            using (UnityWebRequest www = new UnityWebRequest(Constant.HOST + Constant.View_Ticket_API, "POST"))
            {
                www.SetRequestHeader("content-Type", "application/json");
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(sendData);
                www.uploadHandler = new UploadHandlerRaw(rawData);
                www.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Submitting");
                yield return www.SendWebRequest();
                Debug.Log("Submitted");
                if (!string.IsNullOrWhiteSpace(www.error))
                {
                    OnError.Invoke(www);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Submitted Successfully");
                    ViewTicket_responceData viewTicket_Response = JsonUtility.FromJson<ViewTicket_responceData>(www.downloadHandler.text);
                    OnSuccess.Invoke(viewTicket_Response);
                }
            }
        }
        public void ViewTicketDetails(ViewTicketDetails_sendData viewTicketDetails_Send, System.Action<ViewTicketDetails_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            StartCoroutine(ViewTicketDetails_C(viewTicketDetails_Send, OnSuccess, OnError));
        }
        IEnumerator ViewTicketDetails_C(ViewTicketDetails_sendData viewTicketDetails_Send, System.Action<ViewTicketDetails_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            var _sendData = JsonUtility.ToJson(viewTicketDetails_Send);
            string sendData = _sendData;
            Debug.Log(sendData);
            using (UnityWebRequest www = new UnityWebRequest(Constant.HOST + Constant.View_TicketDetails_API, "POST"))
            {
                www.SetRequestHeader("content-Type", "application/json");
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(sendData);
                www.uploadHandler = new UploadHandlerRaw(rawData);
                www.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Submitting");
                yield return www.SendWebRequest();
                Debug.Log("Submitted");
                if (!string.IsNullOrWhiteSpace(www.error))
                {
                    OnError.Invoke(www);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Submitted Successfully");
                    ViewTicketDetails_responceData viewTicketDetails_Response = JsonUtility.FromJson<ViewTicketDetails_responceData>(www.downloadHandler.text);
                    OnSuccess.Invoke(viewTicketDetails_Response);
                }
            }
        }
        public void ReportDetails(Report_sendData report_Send, System.Action<Report_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            StartCoroutine(ReportDetails_C(report_Send, OnSuccess, OnError));
        }
        IEnumerator ReportDetails_C(Report_sendData report_Send, System.Action<Report_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            var _sendData = JsonUtility.ToJson(report_Send);
            string sendData = _sendData;
            Debug.Log(sendData);
            using (UnityWebRequest www = new UnityWebRequest(Constant.HOST + Constant.ReportDetails_API, "POST"))
            {
                www.SetRequestHeader("content-Type", "application/json");
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(sendData);
                www.uploadHandler = new UploadHandlerRaw(rawData);
                www.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Submitting");
                yield return www.SendWebRequest();
                Debug.Log("Submitted");
                if (!string.IsNullOrWhiteSpace(www.error))
                {
                    OnError.Invoke(www);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Submitted Successfully");
                    Report_responceData report_ResponceData = JsonUtility.FromJson<Report_responceData>(www.downloadHandler.text);
                    OnSuccess.Invoke(report_ResponceData);
                }
            }
        }
        public void ResultDetails(Result_sendData result_Send, System.Action<Result_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            StartCoroutine(ResultDetails_C(result_Send, OnSuccess, OnError));
        }
        IEnumerator ResultDetails_C(Result_sendData result_Send, System.Action<Result_responceData> OnSuccess, System.Action<UnityWebRequest> OnError)
        {
            var _sendData = JsonUtility.ToJson(result_Send);
            string sendData = _sendData;
            Debug.Log(sendData);
            using (UnityWebRequest www = new UnityWebRequest(Constant.HOST + Constant.ResultDetails_API, "POST"))
            {
                www.SetRequestHeader("content-Type", "application/json");
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(sendData);
                www.uploadHandler = new UploadHandlerRaw(rawData);
                www.downloadHandler = new DownloadHandlerBuffer();
                Debug.Log("Submitting");
                yield return www.SendWebRequest();
                Debug.Log("Submitted");
                if (!string.IsNullOrWhiteSpace(www.error))
                {
                    OnError.Invoke(www);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Submitted Successfully");
                    Result_responceData result_ResponceData = JsonUtility.FromJson<Result_responceData>(www.downloadHandler.text);
                    OnSuccess.Invoke(result_ResponceData);
                }
            }
        }
    }
}
