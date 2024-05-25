using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TripleChanceProTimer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace KheloJeeto
{
	public class SpinWheel : MonoBehaviour
	{
		public static SpinWheel Instance;
		[SerializeField] private List<int> prize;
		[SerializeField] private List<GameObject> winCardObject;
		[SerializeField] private List<Animator> winAnimatorCard;
		[SerializeField] private Animator winBoxCard;
		[SerializeField] private GameObject winBoxCardObject;
		[SerializeField] Text numberText;
		[SerializeField] Text xFactor;

		private int firstWheelItemNumber;
		private int secondWheelItemNumber;
		private bool _isSpinning = false;
		[SerializeField] private float spinDuration = 8f;
		[Space(20), SerializeField] private UnityEvent OnSpinStartEvent;
		[SerializeField] public UnityEvent OnSpinEndEvent;
		[SerializeField] private UnityEvent OnWin;
		[SerializeField] private Transform wheelCircle;
		[SerializeField] private Transform secondWheelCircle;
		[SerializeField] private AudioClip audioClip;
		[SerializeField] private Animator circleAnim;
		[SerializeField] private GameObject winPopup;

		private int currentNumber, currentMultiplier;
		private float pieceAngle;
		private float halfPieceAngle;
		private float halfPieceAngleWithPaddings;
		private bool win;
		private Action winCallback;
		private Action looseCallback;

		public int result;



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
			//SetupResults(result, 1, false);
			
		}
		public int basemultiplier;
		public void SetupResults(int result, int multiplier, bool win, Action winCallback = null, Action looseCallback = null)
		{
			firstWheelItemNumber = prize.FindIndex(a => a == result);
			//int secondWheelNumber = result % 10;
			//secondWheelItemNumber = prize.FindIndex(a => a == secondWheelNumber);
			print("Item Number : " + firstWheelItemNumber);		

            Debug.LogError(multiplier);
			currentNumber = result;
			currentMultiplier = multiplier;
			this.win = win;
			this.winCallback = winCallback;
			this.looseCallback = looseCallback;
			//Spin();
		}

		[ContextMenu("Spin")]
		public void Spin()
		{
			/*if (!_isSpinning)
			{
				_isSpinning = true;

				OnSpinStartEvent?.Invoke();

				int firstIndex = firstWheelItemNumber;
				int secondIndex = firstWheelItemNumber;

				float firstAngle = -(pieceAngle * firstIndex);
				float secondAngle = -(pieceAngle * secondIndex);

				float rightOffset = (firstAngle - /*halfPieceAngleWithPaddings0) % 360;
				float leftOffset = (firstAngle + /*halfPieceAngleWithPaddings0) % 360;

				float randomAngle = UnityEngine.Random.Range(leftOffset, rightOffset);

				Vector3 firstTargetRotation = new Vector3(0.0f, 0.0f, -5700.0f);// Vector3.back * (randomAngle + 2 * 360 * spinDuration);
				Debug.LogError(firstTargetRotation);
				//float prevAngle = wheelCircle.eulerAngles.z + halfPieceAngle ;
				float prevAngle, currentAngle;
				prevAngle = currentAngle = wheelCircle.eulerAngles.z;

				bool isIndicatorOnTheLine = false;
				/*  secondWheelCircle.DORotate(firstTargetRotation, spinDuration, RotateMode.Fast).SetEase(Ease.OutQuad).OnStart(() => SoundManager.instance.PlaySpinAudio(audioClip))
                      .OnComplete(()=>SoundManager.instance.StopSpinAudio());
				StartCoroutine(SpinCoroutine(firstTargetRotation.z));






               circleAnim.transform.gameObject.SetActive(true);
				circleAnim.Rebind();
				wheelCircle.DORotate(firstTargetRotation, spinDuration, RotateMode.Fast)
				.SetEase(Ease.Linear)

				.OnUpdate(() =>
				{
					float diff = Mathf.Abs(prevAngle - currentAngle);
					if (diff >= halfPieceAngle)
					{
						if (isIndicatorOnTheLine)
						{
						//	SoundManager.instance.PlayOneShotSound(audioClip);
						}
						prevAngle = currentAngle;
						isIndicatorOnTheLine = !isIndicatorOnTheLine;
					}
					currentAngle = wheelCircle.eulerAngles.z;
				})
				.OnComplete(() =>
				{
					_isSpinning = false;

					winCardObject[firstWheelItemNumber].SetActive(true);
					winAnimatorCard[firstWheelItemNumber].Play("winCardAnim");
					winPopup.SetActive(true);
					winPopup.transform.DOScale(1, 0.5f);
					winBoxCardObject.SetActive(true);
					winBoxCard.Play("winBoxAnim");
					circleAnim.transform.gameObject.SetActive(false);

					numberText.text = currentNumber.ToString();
					xFactor.text = currentMultiplier.ToString() + "X";

					if (win)
					{
						winCallback?.Invoke();
					}
					else
						looseCallback?.Invoke();

					StartCoroutine(SendOnSpinCompleteEvent());
				});

			}*/
		}
        public AnimationCurve spinCurve; // Animation curve to control the speed
       // private void StartSpin()
        //{
         //   StartCoroutine(SpinCoroutine());
       // }

        private IEnumerator SpinCoroutine(float value)
        {
           
            float totalDegrees =  value;
            float elapsedTime = 0f;

			SoundManager.instance.PlaySpinAudio(audioClip);

            while (elapsedTime < spinDuration)
            {
                elapsedTime += Time.deltaTime*.25f;
                float t = elapsedTime / spinDuration;
                float currentDegrees =  spinCurve.Evaluate(t);
				secondWheelCircle.localEulerAngles = Vector3.forward * Mathf.Lerp(0, 360*30, currentDegrees) * 1;
                
                yield return null;
            }
            _isSpinning = false;
            secondWheelCircle.localRotation = Quaternion.Euler(0, 0, -totalDegrees);
            SoundManager.instance.StopSpinAudio();
        }

        public void StopPlayingAnimation()
		{
			winBoxCardObject.SetActive(false);
			winCardObject[firstWheelItemNumber].SetActive(false);
		}
		public IEnumerator SendOnSpinCompleteEvent()
		{
			yield return new WaitForSeconds(3);
			xFactor.text = "";
			OnSpinEndEvent?.Invoke();

		}
	}
}