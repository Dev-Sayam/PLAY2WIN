using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[SerializeField] bool showMinsAndSecs;
	public float timerTime = 120f;
	[SerializeField] float noMoreBetsThresholdMin, noMoreBetsThresholdMax, lastChanceThresholdMin, lastChanceThresholdMax, fetchDrawResultsThresholdMin, fetchDrawResultsThresholdax;
	[SerializeField] float placeBetsThresholdMin, placeBetsThresholdMax;
	[SerializeField] Image timerBarForeground;
	[Space(20)]
	[SerializeField] public UnityEvent OnTimerReset;
	[SerializeField] public UnityEvent OnTimerReachingNoMoreBets;
	[SerializeField] public UnityEvent FetchDrawResults;
	[SerializeField] public UnityEvent OnTimerReachingZero;
	[SerializeField] public UnityEvent OnLastChance;
	[SerializeField] public UnityEvent OnPlaceBets;
	private TextMeshProUGUI timerText;
	bool noMoreBetsFlag = false;
	bool lastChanceFlag = false;
	bool placeBetsFlag = false;
	bool fetchDrawResultsFlag = false;

	private TimeSpan lastDrawTime;
	private TimeSpan currentDrawTime;
	private Coroutine timerCoroutine;

	// Start is called before the first frame update
	void Start()
	{
		timerText = GetComponent<TextMeshProUGUI>();
	}

	public void ResetTimer(float timerTime)
	{
		fetchDrawResultsFlag = false;
		noMoreBetsFlag = false;
		lastChanceFlag = false;
		placeBetsFlag = false;
		this.timerTime = timerTime;
		OnTimerReset?.Invoke();
	}

	public void SetLastDrawTime(string drawTime)
	{
		lastDrawTime = Convert.ToDateTime(drawTime).TimeOfDay;
	}

	public void SetCurrentDrawTime(TimeSpan drawTime)
	{
		currentDrawTime = drawTime;
	}

	public void RunTimer(float timerTime)
	{
		ResetTimer(timerTime);
		if (timerCoroutine != null)
		{
			StopCoroutine(timerCoroutine);
		}
		timerCoroutine = StartCoroutine(RunTimerRoutine());
	}

	IEnumerator RunTimerRoutine()
	{
		timerTime += 1;
		while (timerTime >= 0)
		{
			timerTime -= Time.deltaTime;
			
			if (showMinsAndSecs)
			{
				int quotient = (int)timerTime / 60;
				int remainder = (int)timerTime % 60;
				timerText.SetText(quotient + ":" + remainder.ToString("D2"));
			}
			else
			{
				int timeInInt = (int)timerTime;
				timerText.SetText(timeInInt.ToString());
			}

			float drawTimeSpan = (float) currentDrawTime.Subtract(lastDrawTime).TotalSeconds;
			timerBarForeground.fillAmount = timerTime / drawTimeSpan;

			if (placeBetsFlag == false && timerTime >= placeBetsThresholdMin && timerTime <= placeBetsThresholdMax)
			{
				OnPlaceBets?.Invoke();
				placeBetsFlag = true;
			}

			if (lastChanceFlag == false && timerTime >= lastChanceThresholdMin && timerTime <= lastChanceThresholdMax)
			{
				OnLastChance?.Invoke();
				lastChanceFlag = true;
			}

			if (noMoreBetsFlag == false && timerTime >= noMoreBetsThresholdMin && timerTime <= noMoreBetsThresholdMax)
			{
				OnTimerReachingNoMoreBets?.Invoke();
				noMoreBetsFlag = true;
			}

			if (fetchDrawResultsFlag == false && timerTime >= fetchDrawResultsThresholdMin && timerTime <= fetchDrawResultsThresholdax)
			{
				FetchDrawResults?.Invoke();
				fetchDrawResultsFlag = true;
			}
			
			yield return null;

		}
		
		OnTimerReachingZero?.Invoke();
	}
}
