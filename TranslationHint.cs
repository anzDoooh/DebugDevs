using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;

public class TranslationHint : MonoBehaviour
{
    public Text questionText;        // Text component to display the question.
    public Button hintButton;        // Button to trigger the hint/translation.
    public InputField userInputField; // Input field for the user's input.
    public InputField hintInputField; // Input field for the hint word.
    public Text translationText;     // Text component to display the translation or error message.
    public Text feedbackText;        // Text component to display feedback if the answer is correct or not.
    public Button submitButton;      // Button to submit the user's answer.

    // List of questions and their answers.
    private List<KeyValuePair<string, string>> questions = new List<KeyValuePair<string, string>>()
    {
        new KeyValuePair<string, string>("A greeting word", "hello"),
        new KeyValuePair<string, string>("The planet we live on", "world"),
        new KeyValuePair<string, string>("A sentence or phrase asking for information", "question"),
        new KeyValuePair<string, string>("A response to a question", "answer")
        // Add more questions as needed.
    };

    private List<int> usedQuestionIndices = new List<int>(); // List to track used questions.
    private System.Random random = new System.Random();      // Random number generator.

    void Start()
    {
        // Shuffle questions.
        ShuffleQuestions();

        // Add event listeners to buttons.
        hintButton.onClick.AddListener(OnHintButtonClick);
        submitButton.onClick.AddListener(OnSubmitButtonClick);

        // Display the first question.
        DisplayNextQuestion();
    }

    void ShuffleQuestions()
    {
        // Fisher-Yates shuffle algorithm
        int n = questions.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            var value = questions[k];
            questions[k] = questions[n];
            questions[n] = value;
        }
    }

    void DisplayNextQuestion()
    {
        // Reset feedback and input fields.
        feedbackText.text = "";
        userInputField.text = "";
        hintInputField.text = "";
        translationText.text = "";

        // Get a random question index that hasn't been used yet.
        int questionIndex;
        do
        {
            questionIndex = random.Next(questions.Count);
        } while (usedQuestionIndices.Contains(questionIndex) && usedQuestionIndices.Count < questions.Count);

        // Mark this question as used.
        usedQuestionIndices.Add(questionIndex);

        // Display the current question.
        questionText.text = questions[questionIndex].Key;
    }

    void OnSubmitButtonClick()
    {
        // Retrieve and trim the user's input.
        string userInput = userInputField.text.Trim().ToLower();

        // Find the correct answer for the current question.
        string correctAnswer = questions.Find(q => q.Key == questionText.text).Value.ToLower();

        // Check if the user's input matches the correct answer.
        if (userInput == correctAnswer)
        {
            feedbackText.text = "Correct!";
        }
        else
        {
            feedbackText.text = "Incorrect. Try again or use a hint!";
        }
    }

    void OnHintButtonClick()
    {
        // Retrieve and trim the hint input.
        string hintInput = hintInputField.text.Trim().ToLower();

        // Check if the hint input is not empty.
        if (string.IsNullOrEmpty(hintInput))
        {
            translationText.text = "Please enter a word to get its translation!";
            return;
        }

        // Start the coroutine to get the translation from the API.
        StartCoroutine(GetTranslation(hintInput));
    }

    IEnumerator GetTranslation(string word)
    {
        string url = $"https://lingva.ml/api/v1/en/si/{word}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            translationText.text = "Error: " + request.error;
        }
        else
        {
            var response = JObject.Parse(request.downloadHandler.text);
            string translation = response["translation"].ToString();
            translationText.text = translation;
        }
    }

    void Update()
    {
        // Check if the user has answered the current question correctly.
        if (feedbackText.text == "Correct!")
        {
            // Move to the next question after a short delay.
            Invoke("GoToNextQuestion", 2f); // 2-second delay before showing the next question.
        }
    }

    void GoToNextQuestion()
    {
        // Check if there are more questions to display.
        if (usedQuestionIndices.Count < questions.Count)
        {
            // Display the next question.
            DisplayNextQuestion();
        }
        else
        {
            // If there are no more questions, display a completion message.
            questionText.text = "Congratulations!";
            userInputField.gameObject.SetActive(false);
            hintButton.gameObject.SetActive(false);
            hintInputField.gameObject.SetActive(false);
            translationText.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
        }
    }
}
