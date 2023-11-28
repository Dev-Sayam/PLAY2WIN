
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginBtn : MonoBehaviour
{

    /// <summary>
    /// Function used in the login btn for temporary basis.
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
