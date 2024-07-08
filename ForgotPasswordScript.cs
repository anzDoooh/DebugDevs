using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class ForgotPasswordScript : MonoBehaviour
{
    public InputField usernameInput;
    public InputField answer1Input;
    public InputField answer2Input;
    public InputField answer3Input;
    public Button submitButton;
    public Button backButton;
    public Text errorMessage;

    void Start()
    {
        if (submitButton == null)
        {
            Debug.LogError("Submit Button is not assigned.");
            return;
        }

        if (backButton == null)
        {
            Debug.LogError("Back Button is not assigned.");
            return;
        }

        submitButton.onClick.AddListener(OnSubmit);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void OnSubmit()
    {
        if (usernameInput == null || answer1Input == null || answer2Input == null || answer3Input == null)
        {
            Debug.LogError("One or more InputFields are not assigned.");
            errorMessage.text = "One or more InputFields are not assigned.";
            return;
        }

        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(answer1Input.text) || string.IsNullOrEmpty(answer2Input.text) || string.IsNullOrEmpty(answer3Input.text))
        {
            errorMessage.text = "Please fill in all fields.";
            return;
        }

        StartCoroutine(ValidateAnswers());
    }

    IEnumerator ValidateAnswers()
    {
        string username = usernameInput.text;
        string answer1 = answer1Input.text;
        string answer2 = answer2Input.text;
        string answer3 = answer3Input.text;

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("answer_1", answer1);
        form.AddField("answer_2", answer2);
        form.AddField("answer_3", answer3);

        string url = "http://localhost/unity/forgot_password.php";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                errorMessage.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                // Check the response from the server
                if (responseText.Contains("\"status\":\"success\""))
                {
                    // Successfully validated, load the game scene
                    SceneManager.LoadScene("GameScene");
                }
                else
                {
                    // Validation failed, show error message
                    errorMessage.text = "Invalid input.";
                }
            }
        }
    }

    void OnBackButtonClicked()
    {
        SceneManager.LoadScene("Login page");
    }
}
