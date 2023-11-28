using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
	[SerializeField] MainData mainData;
	[SerializeField] TMP_Text userIdText;
	[SerializeField] TMP_Text pointsText;
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	private void Start()
	{
		//userIdText.text = "Welcome, " + mainData.receivedLoginData.UserID;
		userIdText.text = mainData.receivedLoginData.UserID;
		pointsText.text = mainData.receivedLoginData.Balance;
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
}
