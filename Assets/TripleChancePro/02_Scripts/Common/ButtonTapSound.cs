using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TripleChanceProTimer
{
    class ButtonTapSound : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }
        void TaskOnClick()
        {
            SoundManager.instance.PlayButtonSound();
        }

    }
}
