using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleChanceProTimer
{
    public class InstantiatePrefab : MonoBehaviour
    {
        public GameObject button_prefab;
        public int intial, final;
        public Sprite[] sp;

        public void ChangeImage(int i, ButtonTap b, int x, int y)
        {
            if (i % 2 == 0)
            {
                b.img.sprite = sp[x];
            }
            else
            {
                b.img.sprite = sp[y];
            }
        }
        public void DeleteAllPrefab()
        {
            int c = transform.childCount;
            ButtonTap[] b = GetComponentsInChildren<ButtonTap>();
            for (int i = 0; i < c; i++)
            {
                DestroyImmediate(b[i].gameObject);
            }
        }
    }
}
