using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RegisterScript : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField confirmPassword;
    public InputField age;
    public Dropdown gender;
    public InputField securityAnswer1;
    public InputField securityAnswer2;
    public InputField securityAnswer3;
    public Button submitButton;
    public Button backButton;

    void Start()
    {
        submitButton.onClick.AddListener(() => StartCoroutine(RegisterCoroutine()));
        backButton.onClick.AddListener(() => {
            Debug.Log("Attempting to load Login page");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login page");
        });
    }

    IEnumerator RegisterCoroutine()
    {
        if (password.text != confirmPassword.text)
        {
            Debug.Log("Passwords do not match");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("password", password.text);
        form.AddField("age", age.text);
        form.AddField("gender", gender.options[gender.value].text);
        form.AddField("security_answer1", securityAnswer1.text);
        form.AddField("security_answer2", securityAnswer2.text);
        form.AddField("security_answer3", securityAnswer3.text);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity/register.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.downloadHandler.text == "New record created successfully")
            {
                Debug.Log("Registration successful, attempting to load Login page");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Login page");
            }
            else
            {
                Debug.Log("Error: " + www.downloadHandler.text);
            }
        }
    }
}
