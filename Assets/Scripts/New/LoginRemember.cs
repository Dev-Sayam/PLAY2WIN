
using UnityEngine;
using UnityEngine.UI;

public class LoginRemember : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Toggle rememberMeToggle;

    private const string rememberMeKey = "RememberMe";
    private const string usernameKey = "SavedUsername";
    private const string passwordKey = "SavedPassword";

    private void Start()
    {
        if (PlayerPrefs.GetInt(rememberMeKey) == 1)
        {
            string savedUsername = PlayerPrefs.GetString(usernameKey);
            string savedPassword = PlayerPrefs.GetString(passwordKey);

            usernameField.text = savedUsername;
            passwordField.text = savedPassword;
            rememberMeToggle.isOn = true;
        }
    }

    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        // Your login authentication logic here
        // ...

        // If the "Remember Me" checkbox is checked, save the credentials
        if (rememberMeToggle.isOn)
        {
            PlayerPrefs.SetInt(rememberMeKey, 1);
            PlayerPrefs.SetString(usernameKey, username);
            PlayerPrefs.SetString(passwordKey, password);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt(rememberMeKey, 0);
            PlayerPrefs.DeleteKey(usernameKey);
            PlayerPrefs.DeleteKey(passwordKey);
            PlayerPrefs.Save();
        }

        // Perform login action
        // ...
    }

}
