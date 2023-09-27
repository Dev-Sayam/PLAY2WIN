using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserAccessController : MonoBehaviour
{
	[Header("Login Fields")]
	[SerializeField] private InputField loginUserIdField;
	[SerializeField] private InputField loginPassField;
	[SerializeField] private string loginServerLink;
	[SerializeField] Toggle rememberMeToggle;

	[Space(20)]
	[SerializeField] private MainData mainData;


	// Start is called before the first frame update
	void Start()
	{
        if (PlayerPrefs.HasKey("userId") && PlayerPrefs.HasKey("password"))
        {
            loginUserIdField.text = PlayerPrefs.GetString("userId");
			loginPassField.text = PlayerPrefs.GetString("password");
        }
    }

	public void OnPressedLogin()
	{
		if (!string.IsNullOrEmpty(loginUserIdField.text) && !string.IsNullOrEmpty(loginPassField.text))
		{
			if (rememberMeToggle.isOn)
			{
				PlayerPrefs.SetString("userId", loginUserIdField.text);
				PlayerPrefs.SetString("password", loginPassField.text);
			}

			StartCoroutine(Login());
		}	
	}

	IEnumerator Login()
	{
		LoginDetails loginDetails = new LoginDetails(
			loginUserIdField.text,
			loginPassField.text,
			SystemInfo.deviceUniqueIdentifier,
			"U",
			"M");

		var loginJson = JsonUtility.ToJson(loginDetails);
		print(loginJson);
		UnityWebRequest www = UnityWebRequest.Post(loginServerLink, loginJson);
		byte[] bodyRaw = Encoding.UTF8.GetBytes(loginJson);
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		www.uploadHandler.Dispose();
		Debug.Log(www.downloadHandler.text);
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			mainData.receivedData = JsonUtility.FromJson<ReceivedLoginData>(www.downloadHandler.text);
			www.downloadHandler.Dispose();
			if (mainData.receivedData.retMsg.Equals("Success"))
			{
				SceneManager.LoadScene(1);
			}
		}

		www.Dispose();
	}
}

public class LoginDetails
{
	public string UserID;
	public string Password;
	public string MacID;
	public string Type;
	public string Device;

	public LoginDetails(string username, string password, string macId, string type, string device)
	{
		this.UserID = username;
		this.Password = password;
		MacID = macId;
		Type = type;
		Device = device;
	}

	public LoginDetails()
	{
		UserID = "U002";
		Password = "123";
		MacID = SystemInfo.deviceUniqueIdentifier;
		Type = "U";
		Device = "M";
	}
}
