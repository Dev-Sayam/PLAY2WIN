using System;
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
	[SerializeField] private Toggle rememberMeToggle;
	[SerializeField] private Button loginBtn;

    [Space(20)]
	[SerializeField] private GameObject alert_ContactOffice;
	[SerializeField] private Text errorText;

	[SerializeField] private MainData mainData;

	private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	private string uniqueId = "";

	void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if (PlayerPrefs.HasKey("userId") && PlayerPrefs.HasKey("password"))
		{
			loginUserIdField.text = PlayerPrefs.GetString("userId");
			loginPassField.text = PlayerPrefs.GetString("password");
		}
	}

	public void OnPressedLogin()
    {
        errorText.text = "";
        loginBtn.interactable = false;
		if (!string.IsNullOrEmpty(loginUserIdField.text) && !string.IsNullOrEmpty(loginPassField.text))
		{
			if (rememberMeToggle.isOn)
			{
				PlayerPrefs.SetString("userId", loginUserIdField.text);
				PlayerPrefs.SetString("password", loginPassField.text);
			}
			uniqueId = PlayerPrefs.GetString(loginUserIdField.text, GenerateRandomUniqueID(12));
		}
		StartCoroutine(Login());
	}

	IEnumerator Login()
	{
		LoginDetails loginDetails = new LoginDetails(
			loginUserIdField.text,
			loginPassField.text,
			//SystemInfo.deviceUniqueIdentifier,
			uniqueId,
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
			mainData.receivedLoginData = JsonUtility.FromJson<ReceivedLoginData>(www.downloadHandler.text);
			www.downloadHandler.Dispose();
            switch (mainData.receivedLoginData.retMsg)
            {
                case "Success":
                    PlayerPrefs.SetString(loginUserIdField.text, uniqueId);
                    SceneManager.LoadScene(1);
                    break;

                case "Admin Approval Required":
                case "Contact Admin":
                    PlayerPrefs.SetString(loginUserIdField.text, uniqueId);
                    alert_ContactOffice.SetActive(true);
                    break;

                case "Security Failed":
					alert_ContactOffice.SetActive(true);
					break;
                case "Password Mismatch":
                    errorText.text = "Invalid Credentials";
                    break;

                default:
                    break;
            }
        }

		loginBtn.interactable = true;
		www.Dispose();
    }

    private string GenerateRandomUniqueID(int stringLength)
    {
		var stringChars = new char[stringLength];
		var random = new System.Random();

		for (int i = 0; i < stringLength; i++)
		{
			stringChars[i] = chars[random.Next(chars.Length)];
		}
		return new String(stringChars);
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
