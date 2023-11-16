﻿using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace KheloJeeto
{
	public class EachCard : MonoBehaviour
	{
		[SerializeField] Sprite[] allCoinsSprites;

		[SerializeField] private string number;
		[SerializeField] GameObject betCoinOnCard;
		[SerializeField] Image coinImage;
		[SerializeField] TextMeshProUGUI coinAmtText;
		[SerializeField] GameObject selectedCard;
		[SerializeField] private UnityEvent OnNotEnoughPointsEvent;

		private Button button;
		private int cardAmt;
		private int lastStoredAmount;

		public int CardAmt { get { return cardAmt; } }


		public string Number { get => number; private set => number = value; }
		public int LastStoredAmount { get => lastStoredAmount; private set => lastStoredAmount = value; }

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		private void Start()
		{
			button.onClick.AddListener(() =>
			{
				PlaceBetOnCard(JeetoJokerManager.Instance.SelectedCoinAmt);
			});
		}

		public void PlaceBetOnCard(int debitAmount)
		{
			if (JeetoJokerManager.Instance.DebitBalance(debitAmount))
			{
				Debug.Log(cardAmt + "    " + debitAmount);
				cardAmt += debitAmount;

				if (cardAmt == 0)
				{
					return;
				}

				JeetoJokerManager.Instance.TotalPointsSpent += debitAmount;
				LastStoredAmount = cardAmt;
				selectedCard.SetActive(true);
				betCoinOnCard.SetActive(true);
				coinAmtText.text = cardAmt.ToString();

				if (cardAmt >= 1000)
				{
					coinImage.sprite = allCoinsSprites[6];
				}
				else if (cardAmt >= 500)
				{
					coinImage.sprite = allCoinsSprites[5];
				}
				else if (cardAmt >= 100)
				{
					coinImage.sprite = allCoinsSprites[4];
				}
				else if (cardAmt >= 50)
				{
					coinImage.sprite = allCoinsSprites[3];
				}
				else if (cardAmt >= 10)
				{
					coinImage.sprite = allCoinsSprites[2];
				}
				else if (cardAmt >= 5)
				{
					coinImage.sprite = allCoinsSprites[1];
				}
				else if (cardAmt >= 2)
				{
					coinImage.sprite = allCoinsSprites[0];
				}
			}
			else
			{
				Debug.Log("<color =red>Not enough points</color>");
				OnNotEnoughPointsEvent?.Invoke();
			}


		}

		public void ClearCard()
		{
			cardAmt = 0;
			coinImage.sprite = allCoinsSprites[0];
			betCoinOnCard.SetActive(false);
			selectedCard.SetActive(false);
		}

		public void DoubleBet()
		{
			PlaceBetOnCard(cardAmt);
		}

		public void SwitchButtonInteraction(bool flag)
		{
			button.interactable = flag;
		}
	}
}