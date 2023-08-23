using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace TripleChanceProTimer
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date_text_area;
        private int year, month, day;
        [SerializeField] private GameObject resultPrefab;
        private Result_sendData result_SendData = new Result_sendData();

        [SerializeField] private Transform content;
        [SerializeField] private Button leftButton, rightButton;
        private Result_responceData result_ResponceData;
        int initial, final, buffer = 95;

        private void OnEnable()
        {
            SetCurrentDate();
            ShowResultDetails();
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
        }
        public void ShowResultDetails()
        {
            initial = 0;
            final = buffer;
            TripleChanceManger.instence.loadingPanel.SetActive(true);
            result_SendData.GameID = TripleChanceManger.instence.gameId;
            string[] tmpDate = date_text_area.text.Split('/');
            tmpDate[1] = tmpDate[1].Length > 1 ? tmpDate[1] : "0" + tmpDate[1];
            tmpDate[0] = tmpDate[0].Length > 1 ? tmpDate[0] : "0" + tmpDate[0];
            result_SendData.Date = tmpDate[2] + "-" + tmpDate[1] + "-" + tmpDate[0];
            API_Manager.instance.ResultDetails(result_SendData, (OnSuccessData) =>
            {
                result_ResponceData = OnSuccessData;
                ShowResultPrefab();
            //for (int i = 0; i < OnSuccessData.Draws.Count; i++)
            //{
            //    ResultPanelPrefabData resultPanelPrefabData = Instantiate(resultPrefab, content).GetComponent<ResultPanelPrefabData>();
            //    resultPanelPrefabData.draw_Date.text = OnSuccessData.Date;
            //    resultPanelPrefabData.draw_Time.text = OnSuccessData.Draws[i].DrawTime;
            //    resultPanelPrefabData.result.text = OnSuccessData.Draws[i].Result;
            //}
            TripleChanceManger.instence.loadingPanel.SetActive(false);
            }, (OnErrorData) =>
            {
                Debug.Log(OnErrorData.error);
            });
        }

        private void ShowResultPrefab()
        {
            ClearContent();
            int count = result_ResponceData.Draws.Count;
            Debug.LogError(count);
            for (int i = initial; i < final; i++)
            {
                if (i >= count)
                {
                    break;
                }
                ResultPanelPrefabData resultPanelPrefabData = Instantiate(resultPrefab, content).GetComponent<ResultPanelPrefabData>();
                resultPanelPrefabData.draw_Date.text = result_ResponceData.Date;
                resultPanelPrefabData.draw_Time.text = result_ResponceData.Draws[i].DrawTime;
                resultPanelPrefabData.result.text = result_ResponceData.Draws[i].Result;
            }
        }
        public void LeftArrowClick()
        {
            if (initial == 0)
            {
                return;
            }
            initial -= buffer;
            final -= buffer;
            ShowResultPrefab();
        }
        public void RightArrowClick()
        {
            int count = result_ResponceData.Draws.Count;

            if (final >= count)
            {
                return;
            }
            initial += buffer;
            final += buffer;
            ShowResultPrefab();
        }
        private void ClearContent()
        {
            ResultPanelPrefabData[] previousHistoryList = content.GetComponentsInChildren<ResultPanelPrefabData>();
            for (int i = 0; i < previousHistoryList.Length; i++)
            {
                Destroy(previousHistoryList[i].gameObject);
            }
        }

    }
}
