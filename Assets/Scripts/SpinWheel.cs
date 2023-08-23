using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SpinWheel : MonoBehaviour
{
	public static SpinWheel Instance;
	[SerializeField] private List<int> prize;
	[SerializeField] TextMeshProUGUI numberText;
	[SerializeField] TextMeshProUGUI xFactor;

	private int itemNumber;
	private bool _isSpinning = false;
	[SerializeField] private float spinDuration = 8f;
	[Space(20), SerializeField] private UnityEvent OnSpinStartEvent;
	[SerializeField] public UnityEvent OnSpinEndEvent;
	[SerializeField] private UnityEvent OnWin;
	[SerializeField] private Transform wheelCircle;
	[SerializeField] private AudioSource audioSource;

	private int currentNumber, currentMultiplier;
	private float pieceAngle;
	private float halfPieceAngle;
	private float halfPieceAngleWithPaddings;
	private bool win;
	private Action winCallback;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}


	void Start()
	{
		pieceAngle = 360 / prize.Count;
		halfPieceAngle = pieceAngle / 2f;
		halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);
	}

	public void SetupResults(int result, int multiplier, bool win, Action winCallback)
	{
		itemNumber = prize.FindIndex(a => a == result);
		print("Item Number : " + itemNumber);

		currentNumber = result;
		currentMultiplier = multiplier;
		this.win = win;
		this.winCallback = winCallback;
	}

	public void Spin()
	{
		if (!_isSpinning)
		{
			_isSpinning = true;

			OnSpinStartEvent?.Invoke();

			int index = itemNumber;

			float angle = -(pieceAngle * index);

			float rightOffset = (angle - /*halfPieceAngleWithPaddings*/0) % 360;
			float leftOffset = (angle + /*halfPieceAngleWithPaddings*/0) % 360;

			float randomAngle =UnityEngine.Random.Range(leftOffset, rightOffset);

			Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * spinDuration);

			//float prevAngle = wheelCircle.eulerAngles.z + halfPieceAngle ;
			float prevAngle, currentAngle;
			prevAngle = currentAngle = wheelCircle.eulerAngles.z;

			bool isIndicatorOnTheLine = false;

			wheelCircle
			.DORotate(targetRotation, spinDuration, RotateMode.Fast)
			.SetEase(Ease.InOutQuart)
			.OnUpdate(() =>
			{
				float diff = Mathf.Abs(prevAngle - currentAngle);
				if (diff >= halfPieceAngle)
				{
					if (isIndicatorOnTheLine)
					{
						audioSource.PlayOneShot(audioSource.clip);
					}
					prevAngle = currentAngle;
					isIndicatorOnTheLine = !isIndicatorOnTheLine;
				}
				currentAngle = wheelCircle.eulerAngles.z;
			})
			.OnComplete(() =>
			{
				_isSpinning = false;

				numberText.text = currentNumber.ToString();
				xFactor.text = currentMultiplier.ToString() + "X";

				if (win)
				{
					winCallback?.Invoke();
				}

				OnSpinEndEvent?.Invoke();
			});

		}
	}
}
