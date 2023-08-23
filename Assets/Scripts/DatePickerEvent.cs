using UnityEngine;

[CreateAssetMenu(menuName = "Date Picker Event", fileName = "New Date Picker Event")]
public class DatePickerEvent : ScriptableObject
{
	event System.Action<string> OnDatePickerEventInvoked;

	public void Invoke(DayToggle dayToggle)
	{
		var dateTime = dayToggle.dateTime.Value;
		var date = dateTime.ToString("yyyy/MM/dd");
		OnDatePickerEventInvoked?.Invoke(date);
	}

	public void AddObserver(System.Action<string> callback)
	{
		OnDatePickerEventInvoked += callback;
	}

	public void RemoveObserver(System.Action<string> callback) 
	{
		OnDatePickerEventInvoked -= callback;
	}

}
