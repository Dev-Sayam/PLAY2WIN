using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

namespace TripleChanceProTimer
{
    public class TripleBoardSelecter : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allpanel;
        [SerializeField] private List<GameObject> allButton;
        private int? selectedButtonId = null;
        [SerializeField] private GameObject arrayParent;
        private GameObject[] allNumerButton = new GameObject[100];
        [SerializeField] private bool is_triple;
        private GameObject[,] twoDArray;
        private List<int> random_number = new List<int>();
        [SerializeField] private List<GameObject> allRandomButton;
        private int? selectedRandomButtonId = null;
        private void Start()
        {
            if (!is_triple)
            {
                Generate2DArray(arrayParent);
            }
            else
            {
                SelectButton(0);

            }
            for (int i = 0; i < 100; i++)
            {
                random_number.Add(i);
            }
        }
        public void OnRandomButtonClick(int id)
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            if (TripleChanceManger.instence.currentBetnumber > Constant.HIGHEST_BET_AMOUNT)
            {
                TripleChanceManger.instence.ShowHighestBetAlertPopUp();
                return;
            }
            int count = int.Parse(allRandomButton[id].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
            if (-GetTotalAmountToClearOnRandomButtonClick() + count * TripleChanceManger.instence.currentBetnumber > TripleChanceManger.instence.balanceAmount)
            {
                TripleChanceManger.instence.NoEnoughMoneyAlertPopUp();
                return;
            }
            if (selectedRandomButtonId != null)
            {
                allRandomButton[(int)selectedRandomButtonId].transform.GetChild(0).gameObject.SetActive(false);
            }
            selectedRandomButtonId = id;
            allRandomButton[id].transform.GetChild(0).gameObject.SetActive(true);
            random_number = random_number.OrderBy(a => System.Guid.NewGuid()).ToList();
            for (int i = 0; i < allNumerButton.Length; i++)
            {
                allNumerButton[i].GetComponent<ButtonTap>().OnBtnReset();
            }
            for (int i = 0; i < count; i++)
            {
                allNumerButton[random_number[i]].GetComponent<ButtonTap>().OnBtnClick();
            }
            TripleChanceManger.instence.DeselectRemoveButton();
        }
        public void OnButtonClick(int id)
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            SelectButton(id);
        }
        public void OnRowSelectButtonClick(int id)
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            GameObject[] rowarray = twoDArray.row(id);
            if (TripleChanceManger.instence.currentBetnumber * rowarray.Length > TripleChanceManger.instence.balanceAmount)
            {
                TripleChanceManger.instence.NoEnoughMoneyAlertPopUp();
                return;
            }
            for (int i = 0; i < rowarray.Length; i++)
            {
                if (!rowarray[i].GetComponent<ButtonTap>().CanPlaceBet())
                {
                    return;
                }
            }
            for (int i = 0; i < rowarray.Length; i++)
            {
                rowarray[i].GetComponent<ButtonTap>().OnBtnClick();
            }
        }
        public void OnColumSelectButtonClick(int id)
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            GameObject[] columnArray = twoDArray.column(id);
            if (TripleChanceManger.instence.currentBetnumber * columnArray.Length > TripleChanceManger.instence.balanceAmount)
            {
                TripleChanceManger.instence.NoEnoughMoneyAlertPopUp();
                return;
            }
            for (int i = 0; i < columnArray.Length; i++)
            {
                if (!columnArray[i].GetComponent<ButtonTap>().CanPlaceBet())
                {
                    return;
                }
            }
            for (int i = 0; i < columnArray.Length; i++)
            {
                columnArray[i].GetComponent<ButtonTap>().OnBtnClick();
            }
        }
        private void SelectButton(int id)
        {
            if (selectedButtonId != null)
            {
                allpanel[(int)selectedButtonId].SetActive(false);
                allButton[(int)selectedButtonId].transform.GetChild(0).gameObject.SetActive(false);
                allButton[(int)selectedButtonId].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            selectedButtonId = id;
            allpanel[id].SetActive(true);
            allButton[id].transform.GetChild(0).gameObject.SetActive(true);
            allButton[id].transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.black;
            if (is_triple)
            {
                arrayParent = allpanel[id];
                Generate2DArray(arrayParent);
            }
        }
        private void Generate2DArray(GameObject obj)
        {
            int allChild = arrayParent.transform.childCount;
            for (int i = 0; i < allChild; i++)
            {
                allNumerButton[i] = arrayParent.transform.GetChild(i).gameObject;
            }
            twoDArray = MyExtensions.Make2DArray(allNumerButton, allChild / 10, allChild / 10);
        }
        public void DeselectRandomButton()
        {
            if (selectedRandomButtonId != null)
            {
                allRandomButton[(int)selectedRandomButtonId].transform.GetChild(0).gameObject.SetActive(false);
                selectedRandomButtonId = null;
            }
        }
        private float GetTotalAmountToClearOnRandomButtonClick()
        {
            float tempAmount = 0;
            for (int i = 0; i < allNumerButton.Length; i++)
            {
                tempAmount += float.Parse(allNumerButton[i].GetComponent<ButtonTap>().bet_text.text);
            }
            return tempAmount;

        }
    }
}