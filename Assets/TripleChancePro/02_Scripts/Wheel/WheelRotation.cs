using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class WheelRotation : MonoBehaviour
    {
        [SerializeField] private RectTransform holder_rect;
        private RectTransform this_rect;
        private WheelController wheel_controller;
        private bool do_rotate;
        [SerializeField]private bool rotate_reverse;
        [SerializeField] private bool show_win,show_Multiplier;
        float tmp_counter, tmp_fraction_value;
        int dir;
        float total_rotation_value;
        private int fix_rotation_angle;
        [SerializeField] [Range(0, 1)] private float rotation_speed = 1;
        [SerializeField] private GameObject winImg;
        [SerializeField] private TextMeshProUGUI winNumText;
        [SerializeField] private RectTransform currentWinNum, lastWinNum;
        [SerializeField] private int lastWinShiftVal = 0;

        private void Awake()
        {
            this_rect = GetComponent<RectTransform>();
            wheel_controller = GetComponentInParent<WheelController>();
            if (rotate_reverse)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }
            total_rotation_value = 360 * wheel_controller.rotation_count;
        }

        // Update is called once per frame
        void Update()
        {
            if (do_rotate)
            {
                tmp_counter += Time.deltaTime * rotation_speed;
                tmp_fraction_value = wheel_controller.wheel_rotate_curve.Evaluate(tmp_counter);
                this_rect.localEulerAngles = Vector3.forward * Mathf.Lerp(0, total_rotation_value, tmp_fraction_value) * dir;
                if (tmp_fraction_value == 0)
                {
                    do_rotate = false;
                    tmp_counter = 0;
                    winImg.SetActive(true);
                    lastWinNum.anchoredPosition = Vector2.up * lastWinShiftVal;
                    if (show_win)
                    {
                        Invoke(nameof(OnRotateComplete), 2f);
                    }
                    if (show_Multiplier)
                    {
                        wheel_controller.ShowMultiPlierImage();
                    }
                    if (rotate_reverse)
                    {
                        SoundManager.instance.PlaySfx(7);
                    }
                }
            }
        }
        private void OnRotateComplete()
        {
            Debug.Log(this.gameObject.name + "  Rotation complete");
            wheel_controller.WinImgShow();
            TripleChanceManger.instence.InfoButtonEnableDisable(true);
        }
        public void StartRotation(int value, int resultNumber)
        {
            currentWinNum.anchoredPosition = Vector2.zero;
            winNumText.text = resultNumber.ToString();
            do_rotate = true;
            fix_rotation_angle = value;
            Invoke(nameof(Set_Fixed_Rotation), Constant.WHEEL_ROTATION_SET_DELAY);
        }
        private void Set_Fixed_Rotation()
        {
            holder_rect.localEulerAngles = Vector3.forward * fix_rotation_angle;
        }
        public void Set_Fixed_Rotation_ForFirstTime(int fix_rotation_angle)
        {
            holder_rect.localEulerAngles = Vector3.forward * fix_rotation_angle;
        }

        public void HideWinImage()
        {
            winImg.SetActive(false);
        }
    }
}
