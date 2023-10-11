using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
	[SerializeField] private string number;
	private Button button;
	private TextMeshProUGUI amountText;
	private int amount;
	private int lastStoredAmount;
	[SerializeField] private UnityEvent OnNotEnoughPointsEvent;

	public string Number { get => number; private set => number = value; }
	public int Amount { get => amount; private set => amount = value; }
	public int LastStoredAmount { get => lastStoredAmount; private set => lastStoredAmount = value; }

	void Awake()
	{
		amountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		button = GetComponent<Button>();
	}

	// Start is called before the first frame update
	void Start()
	{
		button.onClick.AddListener(() =>
		{
			OnPressedButton(Spin2WinGameManager.Instance.selectedCoinAmt);
		});
	}

	public void Clear()
	{
		Amount = 0;
		amountText.SetText("");
	}

	public void OnPressedButton(int debitAmount)
	{
		if (Spin2WinGameManager.Instance.DebitBalance(debitAmount))
		{
			Amount += debitAmount;
			if (Amount != 0)
			{
				amountText.text = Amount.ToString();
			}
			Spin2WinGameManager.Instance.TotalPointsSpent += debitAmount;
			LastStoredAmount = Amount;
		}
		else
		{
			Debug.Log("<color =red>Not enough points</color>");
			OnNotEnoughPointsEvent?.Invoke();
		}
		
	}

	public void SwitchButtonInteraction(bool flag)
	{
		button.interactable = flag;
	}

	public void DoubleAmount()
	{
		OnPressedButton(Amount);
	}
}
