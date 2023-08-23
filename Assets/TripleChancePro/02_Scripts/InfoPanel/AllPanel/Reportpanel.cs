using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
namespace TripleChanceProTimer
{
    public class Reportpanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date_text_area;
        [SerializeField] private TextMeshProUGUI date_text_area1;
        private int year, month, day;
        [SerializeField] private ReportPanelPrefabData reportPanelPrefabData;
        private Report_sendData report_SendData = new Report_sendData();
        private void OnEnable()
        {
            SetCurrentDate();
            ShowReportDeatails();
        }
        private void SetCurrentDate()
        {
            year = System.DateTime.Now.Year;
            month = System.DateTime.Now.Month;
            day = System.DateTime.Now.Day;
            SetDateToDateTextArea();
        }
        private void SetDateToDateTextArea()
        {
            date_text_area.text = day.ToString() + "/" + month.ToString() + "/" + year.ToString();
            date_text_area1.text = day.ToString() + "/" + month.ToString() + "/" + year.ToString();
        }


        public void ShowReportDeatails()
        {
            TripleChanceManger.instence.loadingPanel.SetActive(true);
            report_SendData.UserID = TripleChanceManger.instence.userId;
            string[] tmpDate = date_text_area.text.Split('/');
            report_SendData.FromDate = tmpDate[2] + "-" + tmpDate[1] + "-" + tmpDate[0];
            tmpDate = date_text_area1.text.Split('/');
            report_SendData.ToDate = tmpDate[2] + "-" + tmpDate[1] + "-" + tmpDate[0];
            API_Manager.instance.ReportDetails(report_SendData, (Onsuccess) =>
             {
                 reportPanelPrefabData._name.text = Onsuccess.UserID;
                 reportPanelPrefabData.play.text = Onsuccess.Sale;
                 reportPanelPrefabData.win.text = Onsuccess.Win;
                 reportPanelPrefabData.ntp.text = Onsuccess.NTP;
                 reportPanelPrefabData.optr.text = Onsuccess.Optr;
                 reportPanelPrefabData.comm.text = Onsuccess.Comm;
                 TripleChanceManger.instence.loadingPanel.SetActive(false);
             },
            (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
            });


        }
    }
}
