using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TripleChanceProTimer
{
    public class PanelSlide : MonoBehaviour
    {
        private event System.Action temp_event;
        private bool single_bool, double_bool, triple_bool;
        private bool single_Move, double_Move, triple_Move;
        [SerializeField] private RectTransform single_Trans, double_Trans, triple_Trans;
        [SerializeField] private RectTransform single_EndPos, double_EndPos, triple_EndPos;
        [SerializeField] private float move_Speed;
        private Vector2 singleDefPos, doubleDefPos, TripleDefPos;
        private Vector2 singleTempPos, doubleTempPos, TripleTempPos;
        [SerializeField] private BarImg single_img, double_img, triple_img;
        [SerializeField] private SliderBar[] sliderBars;
        [HideInInspector] public int selectedSliderBarId;
        private bool disableMulticlick;
        private void Start()
        {
            singleDefPos = single_Trans.anchoredPosition;
            doubleDefPos = double_Trans.anchoredPosition;
            TripleDefPos = triple_Trans.anchoredPosition;

            singleTempPos = singleDefPos;
            doubleTempPos = doubleDefPos;
            TripleTempPos = TripleDefPos;

        }
        void Update()
        {
            if (single_Move)
            {
                single_Trans.anchoredPosition = Vector3.MoveTowards(single_Trans.anchoredPosition, singleTempPos, move_Speed * Time.deltaTime);
                if (single_Trans.anchoredPosition == singleTempPos)
                {
                    single_Move = false;
                    temp_event?.Invoke();
                    temp_event = null;
                    disableMulticlick = false;
                }
            }
            if (double_Move)
            {
                double_Trans.anchoredPosition = Vector3.MoveTowards(double_Trans.anchoredPosition, doubleTempPos, move_Speed * Time.deltaTime);
                if (double_Trans.anchoredPosition == doubleTempPos)
                {
                    double_Move = false;
                    temp_event?.Invoke();
                    temp_event = null;
                    disableMulticlick = false;
                }
            }
            if (triple_Move)
            {
                triple_Trans.anchoredPosition = Vector3.MoveTowards(triple_Trans.anchoredPosition, TripleTempPos, move_Speed * Time.deltaTime);
                if (triple_Trans.anchoredPosition == TripleTempPos)
                {
                    triple_Move = false;
                    temp_event?.Invoke();
                    temp_event = null;
                    disableMulticlick = false;
                }
            }
        }
        public void OnSingleBarBtnTap()
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            if (disableMulticlick) return;
            disableMulticlick = true;
            if (double_Trans.anchoredPosition != doubleDefPos)
            {
                DoubleSlide();
                temp_event += SingleSlide;
            }
            else if (triple_Trans.anchoredPosition != TripleDefPos)
            {
                TripleSlide();
                temp_event += SingleSlide;
            }
            else
            {
                SingleSlide();
            }
            selectedSliderBarId = 0;
        }
        public void OnDoubleBarBtnTap()
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            if (disableMulticlick) return;
            disableMulticlick = true;
            if (single_Trans.anchoredPosition != singleDefPos)
            {
                SingleSlide();
                temp_event += DoubleSlide;
            }
            else if (triple_Trans.anchoredPosition != TripleDefPos)
            {
                TripleSlide();
                temp_event += DoubleSlide;
            }
            else
            {
                DoubleSlide();
            }
            selectedSliderBarId = 1;
        }
        public void OnTripleBarBtnTap()
        {
            if (TripleChanceManger.instence.isAllButtonDiasabled) return;
            if (disableMulticlick) return;
            disableMulticlick = true;
            if (single_Trans.anchoredPosition != singleDefPos)
            {
                SingleSlide();
                temp_event += TripleSlide;
            }
            else if (double_Trans.anchoredPosition != doubleDefPos)
            {
                DoubleSlide();
                temp_event += TripleSlide;
            }
            else
            {
                TripleSlide();
            }
            selectedSliderBarId = 2;
        }
        private void SingleSlide()
        {
            single_Move = true;
            if (!single_bool)
            {
                single_bool = true;
                singleTempPos = single_EndPos.anchoredPosition;
                single_img.leftImg.SetActive(true);
                single_img.rightImg.SetActive(false);
            }
            else
            {
                single_bool = false;
                singleTempPos = singleDefPos;
                single_img.leftImg.SetActive(false);
                single_img.rightImg.SetActive(true);
            }
        }
        private void DoubleSlide()
        {
            double_Move = true;
            if (!double_bool)
            {
                double_bool = true;
                doubleTempPos = double_EndPos.anchoredPosition;
                double_img.leftImg.SetActive(true);
                double_img.rightImg.SetActive(false);
            }
            else
            {
                double_bool = false;
                doubleTempPos = doubleDefPos;
                double_img.leftImg.SetActive(false);
                double_img.rightImg.SetActive(true);
            }
        }
        private void TripleSlide()
        {
            triple_Move = true;
            if (!triple_bool)
            {
                triple_bool = true;
                TripleTempPos = triple_EndPos.anchoredPosition;
                triple_img.leftImg.SetActive(true);
                triple_img.rightImg.SetActive(false);
            }
            else
            {
                triple_bool = false;
                TripleTempPos = TripleDefPos;
                triple_img.leftImg.SetActive(false);
                triple_img.rightImg.SetActive(true);
            }
        }
        public void CloseAllSlider()
        {
            if (double_Trans.anchoredPosition != doubleDefPos)
            {
                double_bool = true;
                DoubleSlide();
            }
            else if (triple_Trans.anchoredPosition != TripleDefPos)
            {
                triple_bool = true;
                TripleSlide();
            }
            else if (single_Trans.anchoredPosition != singleDefPos)
            {
                single_bool = true;
                SingleSlide();
            }
        }
        [System.Serializable]
        private struct BarImg
        {
            public GameObject leftImg;
            public GameObject rightImg;
        }
        public void ClearAllBetText()
        {
            //TripleChanceManger.instence.totalBetAmount = 0;
            for (int i = 0; i < sliderBars.Length; i++)
            {
                sliderBars[i].ClearBetText();
            }
        }
        public void UpdateTotalBetText(int betAmount, bool setBalanceAmount = true)
        {
            sliderBars[selectedSliderBarId].UpdateTotalBetText(betAmount, setBalanceAmount);
        }
    }
}
