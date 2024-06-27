using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace TripleChanceProTimer
{
    public class WheelController : MonoBehaviour
    {
        public AnimationCurve wheel_rotate_curve;
        public int rotation_count = 10;
        //[Range(0, 1)] public float rotation_speed = 1;
        [SerializeField] int[] indes_Wise_Angle;
        private int bet_number;
        [SerializeField] private List<WheelRotation> all_wheel;
        [SerializeField] private Text text;
        [SerializeField] private GameObject win_obj, triple_ChanceProImage;
        [SerializeField] private Animator winAnim;
        public TextMeshProUGUI winShowText, youwinText;
        [SerializeField] private Animator circleAnim;
        [SerializeField] private GameObject[] multiplierObj;
        private int tempBetNumber;
        public int multiplierObjId;
        [SerializeField] private GameObject WinPopUp;
        [SerializeField] private TextMeshProUGUI WinPopUpAmountText;
        public void PlaceBet(int betNumber)
        {
            circleAnim.transform.gameObject.SetActive(true);
            circleAnim.Rebind();
            //Invoke(nameof(ShowMultiPlierImage), 2.8f);
            for (int i = 0; i < multiplierObj.Length; i++)
            {
                multiplierObj[i].SetActive(false);
            }
            WinImgHide();
            bet_number = betNumber;
            tempBetNumber = bet_number;
            for (int i = 0; i < all_wheel.Count; i++)
            {
                all_wheel[i].HideWinImage();
                all_wheel[i].StartRotation(indes_Wise_Angle[bet_number % 10], (bet_number % 10));
                bet_number /= 10;
            }
        }
        public void SetFixedRotationForFirstTime(int betNumber)
        {
            bet_number = betNumber;
            tempBetNumber = bet_number;
            for (int i = 0; i < all_wheel.Count; i++)
            {
                all_wheel[i].Set_Fixed_Rotation_ForFirstTime(indes_Wise_Angle[bet_number % 10]);
                bet_number /= 10;
            }
        }
        public void WinImgShow()
        {
            //win_obj.SetActive(true);
            //winAnim.Play("WinImgWin");
            // StartCoroutine(DelayShow());

          //  MultilierObjShow(multiplierObjId, false);

            // winShowText.text = tempBetNumber.ToString();
            TripleChanceManger.instence.OnWheelRotateComplete();
            if (TripleChanceManger.instence.winAmount > 0)
            {
                WinPopUpAmountText.text = TripleChanceManger.instence.winAmount.ToString();
                StartCoroutine(DelayShow());
            }
            else
            {
                winShowText.gameObject.SetActive(true);
                MultilierObjShow(multiplierObjId, true);
            }
        }
        private void WinImgHide()
        {
            //win_obj.SetActive(false);
            winShowText.gameObject.SetActive(false);
            triple_ChanceProImage.SetActive(false);
        }
        IEnumerator DelayShow()
        {
            WinPopUp.SetActive(true);
            youwinText.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);
            youwinText.gameObject.SetActive(false);
            WinPopUp.SetActive(false);
            winShowText.gameObject.SetActive(true);
            MultilierObjShow(multiplierObjId, true);
        }
        public void ShowMultiPlierImage()
        {
            circleAnim.transform.gameObject.SetActive(false);
           // MultilierObjShow(multiplierObjId, true);
            SoundManager.instance.PlaySfx(7);
        }
        private void MultilierObjShow(int id, bool active)
        {
            if (id > 1)
            {
                multiplierObj[id].SetActive(active);
            }
        }
    }
}
