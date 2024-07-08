using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class LoginScript : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button loginButton;
    public Button registerButton;
    public Button forgotPasswordButton;

    void Start()
    {
        loginButton.onClick.AddListener(Login); // Changed to just pass the method name
        registerButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Register page"));
        forgotPasswordButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Forgot Password page"));
    }

    void Login() // Removed IEnumerator, as UnityWebRequest is handled in a coroutine separately
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity/login.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.downloadHandler.text == "Login successful")
            {
                Debug.Log("Login successful");
                // Load the main game scene here
            }
            else
            {
                Debug.Log("Invalid username or password");
            }
        }
    }
}
