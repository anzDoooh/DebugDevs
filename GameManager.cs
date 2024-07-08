using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text questionText;
    public InputField inputField;
    public Button submitButton;
    public Button hintButton;
    public PronunciationEvaluator pronunciationEvaluator;
    public SpeechRecognitionManager speechRecognitionManager;
    public PopupMessage popupMessage;

    private string currentWord;

    void Start()
    {   pronunciationEvaluator = GetComponent<PronunciationEvaluator>();

        if (submitButton != null)
            submitButton.onClick.AddListener(CheckAnswer);
        else
            Debug.LogError("SubmitButton is not assigned in the Inspector!");

        if (hintButton != null)
            hintButton.onClick.AddListener(ProvideHint);
        else
            Debug.LogError("HintButton is not assigned in the Inspector!");

        LoadNextQuestion();
        ShowInstructions();
    }

    void LoadNextQuestion()
    {
        currentWord = "example"; // Replace with logic to load the next word
        if (questionText != null)
            questionText.text = "Pronounce the word: " + currentWord;
        else
            Debug.LogError("QuestionText is not assigned in the Inspector!");
    }

    void CheckAnswer()
    {
        if (speechRecognitionManager != null)
        {
            speechRecognitionManager.StartRecording();
            popupMessage.Show("Please pronounce the word: " + currentWord);
            Invoke("StopRecordingAndEvaluate", 5); // Record for 5 seconds
        }
        else
            Debug.LogError("SpeechRecognitionManager is not assigned in the Inspector!");
    }

    void StopRecordingAndEvaluate()
    {
        if (speechRecognitionManager != null)
            speechRecognitionManager.StopRecording();
        else
            Debug.LogError("SpeechRecognitionManager is not assigned in the Inspector!");
    }

    void ProvideHint()
    {
        if (pronunciationEvaluator != null)
        {
            string userInput = inputField != null ? inputField.text : null;
            if (!string.IsNullOrEmpty(userInput))
            {
                // Use the entered word to provide a hint
                byte[] dummyAudioData = new byte[1]; // Dummy audio data to prevent null reference
                pronunciationEvaluator.EvaluatePronunciation(dummyAudioData, userInput);
                popupMessage.Show("Providing a hint for: " + userInput);
            }
            else
            {
                Debug.LogError("InputField is not assigned in the Inspector or user input is empty!");
                popupMessage.Show("Please enter a word to get a hint.");
            }
        }
        else
            Debug.LogError("PronunciationEvaluator is not assigned in the Inspector!");
    }



    void ShowInstructions()
    {
        if (popupMessage != null)
            popupMessage.Show("Welcome! To play the game, press the 'Submit' button and pronounce the word shown. You can get a hint by typing a word and pressing the 'Hint' button.");
        else
            Debug.LogError("PopupMessage is not assigned in the Inspector!");
    }
}