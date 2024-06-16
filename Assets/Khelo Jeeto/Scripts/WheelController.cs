using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using TripleChanceProTimer;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine.Events;
namespace KheloJeeto
{
    public class WheelController : MonoBehaviour
    {
        public AnimationCurve wheel_rotate_curve;
        public int rotation_count = 10;
        //[Range(0, 1)] public float rotation_speed = 1;
        [SerializeField] Vector2[] indes_Wise_Angle;
        private int bet_number;
        [SerializeField] private List<WheelRotation> all_wheel;
        [SerializeField] private Animator circleAnim;
        [SerializeField] private GameObject[] multiplierObj;
        public int multiplierObjId;
        [SerializeField] private GameObject WinPopUp;
        [SerializeField] private TextMeshProUGUI WinPopUpAmountText;
        public AudioClip audioClip;
        private int currentNumber, currentMultiplier;
        private bool win;
        private Action winCallback;
        private Action looseCallback;


        

        [SerializeField] private List<GameObject> winCardObject;
        [SerializeField] private List<Animator> winAnimatorCard;
        [SerializeField] private Animator winBoxCard;
        [SerializeField] private GameObject winBoxCardObject;

        [SerializeField] Text numberText;
        [SerializeField] Text xFactor;

         public UnityEvent OnSpinEndEvent;
        public void PlaceBet(int betNumber, int multiplier, bool win, Action winCallback = null, Action looseCallback = null)
        {
            Debug.LogError(multiplier);
            currentNumber = betNumber;
            currentMultiplier = multiplier;
            bet_number = betNumber;
            this.win = win;
            this.winCallback = winCallback;
            this.looseCallback = looseCallback;

           
           // WinImgHide();
                     
        }
        public void StartRotation()
        {
            Debug.Log(bet_number);
            circleAnim.transform.gameObject.SetActive(true);
            circleAnim.Rebind();
            all_wheel[0].StartRotation((int)indes_Wise_Angle[bet_number].x);
            all_wheel[1].StartRotation((int)indes_Wise_Angle[bet_number].y);
        }
        
        public void WinImgShow()
        {
           // win_obj.SetActive(true);
            //winAnim.Play("WinImgWin");

            winCardObject[bet_number].SetActive(true);
            winAnimatorCard[bet_number].Play("winCardAnim");
            WinPopUp.SetActive(true);
            WinPopUp.transform.DOScale(1, 0.5f);
            winBoxCardObject.SetActive(true);
            winBoxCard.Play("winBoxAnim");
            circleAnim.transform.gameObject.SetActive(false);

            numberText.text = currentNumber.ToString();
            if(currentMultiplier > 1)
                xFactor.text = currentMultiplier.ToString() + "X";

            if (win)
            {
                winCallback?.Invoke();
            }
            else
                looseCallback?.Invoke();

            StartCoroutine(SendOnSpinCompleteEvent());            
        }

        public IEnumerator SendOnSpinCompleteEvent()
        {
            yield return new WaitForSeconds(3);
            xFactor.text = "";
            OnSpinEndEvent?.Invoke();

        }
        public void StopPlayingAnimation()
        {
            winBoxCardObject.SetActive(false);
            winCardObject[bet_number].SetActive(false);
        }

        public void ShowMultiPlierImage()
        {
            circleAnim.transform.gameObject.SetActive(false);
            // MultilierObjShow(multiplierObjId, true);
            //SoundManager.instance.PlaySfx(7);
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
