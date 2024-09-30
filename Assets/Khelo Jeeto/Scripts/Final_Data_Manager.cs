using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KheloJeeto
{
    public class Final_Data_Manager : MonoBehaviour
    {
        [SerializeField] private List<Coin_Data> coin_datas;
        [SerializeField] private List<Sprite> all_Coins;
        [SerializeField] private List<GameObject> all_Rebons;
        [SerializeField] private List<Animator> winAnimatorCard;
        public void Initialize(int cardID, int cardAmount, int coinId)
        {
            Debug.Log("hhkjsakfskh "+JsonUtility.ToJson(cardID+""+ cardAmount+""+ coinId));
            if (cardAmount > 0)
            {
                coin_datas[cardID].Coin.sprite = all_Coins[coinId];
                coin_datas[cardID].CardAmount.text = cardAmount.ToString();
                coin_datas[cardID].Coin.gameObject.SetActive(true);
                all_Rebons[cardID].SetActive(true);
            }
            else
            {
                coin_datas[cardID].Coin.gameObject.SetActive(false);
                all_Rebons[cardID].SetActive(false);
            }
        }

        public void ResetData()
        {
            for (int i = 0; i < coin_datas.Count; i++)
            {
                coin_datas[i].CardAmount.text = null;
                coin_datas[i].Coin.gameObject.SetActive(false);
                winAnimatorCard[i].enabled = false;
                all_Rebons[i].SetActive(true);
                all_Rebons[i].SetActive(false);

            }
        }

    }
    [System.Serializable]
    public class Coin_Data
    {
        public Image Coin;
        public TextMeshProUGUI CardAmount;
    }
}
