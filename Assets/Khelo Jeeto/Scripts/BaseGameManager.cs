using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

namespace KheloJeeto
{
	public class BaseGameManager : MonoBehaviour
	{
		[SerializeField] protected MainData mainData;
		[SerializeField] string drawDetailsUrl;

		protected void GetLastFewDrawDetails(string gameId, int numOfDrawDetails,
			Action<string> onSuccess, Action<string> onFail = null)
		{
			StartCoroutine(GetLastFewDrawDetailsCoroutine(gameId, numOfDrawDetails, onSuccess, onFail));
		}

		IEnumerator GetLastFewDrawDetailsCoroutine(string gameId, int numOfDrawDetails,
			Action<string> onSuccess, Action<string> onFail = null)
		{
			var drawDetails = new SentDrawDetails(gameId, numOfDrawDetails.ToString(), "D");

			var drawDetailsJson = JsonUtility.ToJson(drawDetails);
			print(drawDetailsJson);
			UnityWebRequest www = UnityWebRequest.Post(drawDetailsUrl, drawDetailsJson);
			byte[] bodyRaw = Encoding.UTF8.GetBytes(drawDetailsJson);
			www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.SendWebRequest();
			www.uploadHandler.Dispose();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
				onFail?.Invoke(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
				onSuccess?.Invoke(www.downloadHandler.text);
			}
		}

		protected void SendPendingDrawDetails(string gameId, Action<string> onSuccess,
			Action<string> onFail = null)
		{
			StartCoroutine(SendPendingDrawDetailsCoroutine(gameId, onSuccess, onFail));
		}

		IEnumerator SendPendingDrawDetailsCoroutine(string gameId, Action<string> onSucess,
			Action<string> onFail = null)
		{
			var drawDetails = new SentDrawDetails(gameId, "1", "P");

			var drawDetailsJson = JsonUtility.ToJson(drawDetails);
			print(drawDetailsJson);
			UnityWebRequest www = UnityWebRequest.Post(drawDetailsUrl, drawDetailsJson);
			byte[] bodyRaw = Encoding.UTF8.GetBytes(drawDetailsJson);
			www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.SendWebRequest();
			www.uploadHandler.Dispose();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
				onFail?.Invoke(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
				onSucess?.Invoke(www.downloadHandler.text);
			}
		}

		public void GetCurrentDrawDetailsResult(string gameId, Action<string> onSuccess,
			Action<string> onFail = null)
		{
			StartCoroutine(GetCurrentDrawDetailsResultCoroutine(gameId, onSuccess, onFail));
		}

		IEnumerator GetCurrentDrawDetailsResultCoroutine(string gameId, Action<string> onSucess,
			Action<string> onFail = null)
		{
			var drawDetails = new SendDrawDetailsCurrent(mainData.receivedLoginData.UserID,
				gameId, "1", "D", "Y");

			var drawDetailsJson = JsonUtility.ToJson(drawDetails);
			print(drawDetailsJson);
			UnityWebRequest www = UnityWebRequest.Post(drawDetailsUrl, drawDetailsJson);
			byte[] bodyRaw = Encoding.UTF8.GetBytes(drawDetailsJson);
			www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.SendWebRequest();
			www.uploadHandler.Dispose();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
				onFail?.Invoke(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
				onSucess?.Invoke(www.downloadHandler.text);
			}
		}

		public void GoBackToLobby()
		{
			SceneManager.LoadScene("Lobby Scene");
		}

	}
}

/*	[System.Serializable]
	public class CurrentDrawDetails
	{
		public string retMsg;
		public string retStatus;
		public string Query;
		public string Now;
		public string GameID;
		public string Date;
		public CurrentDraws[] Draws;
		public WinDetails WinDtls;
		public string AutoClaimed;
		public string Balance;

		[System.Serializable]
		public class CurrentDraws
		{
			public string GID;
			public string DrawDate;
			public string DrawTime;
			public string Status;
			public string Result;
			public string XF;
			public string TotWin;
		}

		[System.Serializable]
		public class WinDetails
		{
			public string GIDGName;
			public string TicketID;
			public string Win;
			public string DrawDate;
			public string DrawTime;
		}


	}*/

/*	[System.Serializable]
	public class BookTicketDetails
	{
		public string UserID;
		public string GameID;
		public string Draw;
		public BetsDetails[] Bets;

		[System.Serializable]
		public class BetsDetails
		{
			public string Digit;
			public string Qty;
			public string SDigit;

			public BetsDetails(string digit, string qty, string sDigit)
			{
				Digit = digit;
				Qty = qty;
				SDigit = sDigit;
			}
		}

		public BookTicketDetails(string userID, string gameID, string draw, BetsDetails[] bets)
		{
			UserID = userID;
			GameID = gameID;
			Draw = draw;
			Bets = bets;
		}
	}
*/

/*
	[System.Serializable]
	public class PendingDrawDetails
	{
		public string retMsg;
		public string retStatus;
		public string Query;
		public string Now;
		public string GameID;
		public string Date;
		public Draw[] Draws;
	}

	[System.Serializable]
	public class Draw
	{
		public string GID;
		public string DrawDate;
		public string DrawTime;
		public string Status;
	}

*/