using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allInfoPanel;
        [SerializeField] private List<GameObject> allInfoButton;
        private int? selectedBetButtonId = null;
        private bool resultCalenderBool;
        private bool reportCalenderBool1, reportCalenderBool2;
        private void OnEnable()
        {
            InfoButtonClick(0);
        }
        public void InfoButtonClick(int id)
        {

            if (selectedBetButtonId != null)
            {
                allInfoPanel[(int)selectedBetButtonId].SetActive(false);
                allInfoButton[(int)selectedBetButtonId].transform.GetChild(0).gameObject.SetActive(false);
            }
            selectedBetButtonId = id;
            allInfoPanel[id].SetActive(true);
            allInfoButton[id].transform.GetChild(0).gameObject.SetActive(true);
        }

        public void OnCalenderBtnClick(GameObject calenderObj)
        {
            if (calenderObj.activeInHierarchy)
            {
                calenderObj.SetActive(false);
            }
            else
            {
                calenderObj.SetActive(true);
            }
        }

        private void OnDisable()
        {
            TripleChanceManger.instence.loadingPanel.SetActive(false);
        }
    }
}
