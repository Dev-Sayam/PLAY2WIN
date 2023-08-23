using UnityEngine;

[CreateAssetMenu(fileName = "MainData", menuName = "ScriptableObjects/MainData")]
public class MainData : ScriptableObject
{
	public ReceivedLoginData receivedData;

	public PendingDrawDetails pendingDrawDetails;

	public CurrentDrawDetails currentDrawDetails;

	public CurrentDrawDetails lastFewDrawDetails;

}

[System.Serializable]
public class ReceivedLoginData
{
	public string UserID;
	public string retMsg;
	public string retStatus;
	public string Balance;
	public string IsLocked;
	public string Message;
	public int PID;
	public string AutoClaim;
	public string Print;
	public string Assign;
	public Games[] Games;
	public string Now;
}

[System.Serializable]
public class Games
{
	public string GCode;
	public string GName;
	public string GType;
}
