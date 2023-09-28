using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
	[SerializeField] MainData mainData;
	[SerializeField] Text userIdText;
	[SerializeField] Text pointsText;
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	private void Start()
	{
		userIdText.text = "Welcome, " + mainData.receivedLoginData.UserID;
		pointsText.text = "POINTS : " +  mainData.receivedLoginData.Balance;
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
