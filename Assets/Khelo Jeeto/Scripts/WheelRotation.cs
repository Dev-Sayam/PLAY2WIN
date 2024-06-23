using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace KheloJeeto
{
    public class WheelRotation : MonoBehaviour
    {
        [SerializeField] private RectTransform holder_rect;
        private RectTransform this_rect;
        private WheelController wheel_controller;
        private bool do_rotate;
        [SerializeField] private bool rotate_reverse;
        [SerializeField] private bool show_win, show_Multiplier;
        float tmp_counter, tmp_fraction_value;
        int dir;
        float total_rotation_value;
        public Animator spinWheelClip;
        public const float WHEEL_ROTATION_SET_DELAY = 2f;
        private int fix_rotation_angle;
        [SerializeField][Range(0, 1)] private float rotation_speed = 1;
        private void Awake()
        {
            this_rect = GetComponent<RectTransform>();
            wheel_controller = GetComponentInParent<WheelController>();
            dir = 1;
            
            total_rotation_value = 360 * wheel_controller.rotation_count;
        }

        // Update is called once per frame
        /*void Update()
        {
            if (do_rotate)
            {
                tmp_counter += Time.deltaTime * rotation_speed;
                tmp_fraction_value = wheel_controller.wheel_rotate_curve.Evaluate(tmp_counter);
               
                this_rect.localEulerAngles = Vector3.forward * Mathf.Lerp(0, total_rotation_value, tmp_fraction_value) * dir;
                SoundManager.instance.PlayOneShotSound(wheel_controller.audioClip);
                if (tmp_fraction_value == 0)
                {
                    Debug.LogError("Calld for stopping the wheel");
                    do_rotate = false;
                    tmp_counter = 0;
                    if (show_win)
                    {
                        OnRotateComplete();
                    }
                    if (show_Multiplier)
                    {
                        wheel_controller.ShowMultiPlierImage();
                    }
                    
                }
            }
    }*/

        public void DoCompleteAnimationEvent()
        {
            if (show_win)
            {
                SoundManager.instance.StopSpinAudio();
                Invoke(nameof(OnRotateComplete), 1f);
                //OnRotateComplete();
            }
            if (show_Multiplier)
            {
                wheel_controller.ShowMultiPlierImage();
            }
        }
        private void OnRotateComplete()
        {
            Debug.Log(this.gameObject.name + "  Rotation complete");

            wheel_controller.WinImgShow();
           // TripleChanceManger.instence.InfoButtonEnableDisable(true);
        }
        public void StartRotation(int value)
        {
            do_rotate = true;
            fix_rotation_angle = value;
            if (show_Multiplier)
                SoundManager.instance.PlaySpinAudio(wheel_controller.audioClip);
            if (rotate_reverse)
                spinWheelClip.Play("SpinWheelAnimationReverse");
            else
                spinWheelClip.Play("SpinWheelAnimation");
            Invoke(nameof(Set_Fixed_Rotation), WHEEL_ROTATION_SET_DELAY);
        }
        private void Set_Fixed_Rotation()
        {
            holder_rect.localEulerAngles = Vector3.forward * fix_rotation_angle;
        }
        public void Set_Fixed_Rotation_ForFirstTime(int fix_rotation_angle)
        {
            holder_rect.localEulerAngles = Vector3.forward * fix_rotation_angle;
        }
    }
}
