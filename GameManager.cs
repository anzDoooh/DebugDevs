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

    private string currentWord;

    void Start()
    {
        submitButton.onClick.AddListener(CheckAnswer);
        hintButton.onClick.AddListener(ProvideHint);
        LoadNextQuestion();
    }

    void LoadNextQuestion()
    {
        currentWord = "example"; // Replace with logic to load the next word
        questionText.text = "Pronounce the word: " + currentWord;
    }

    void CheckAnswer()
    {
        // Start recording and stop after a delay
        speechRecognitionManager.StartRecording();
        Invoke("StopRecordingAndEvaluate", 5); // Record for 5 seconds
    }

    void StopRecordingAndEvaluate()
    {
        speechRecognitionManager.StopRecording();
        byte[] audioData = speechRecognitionManager.GetRecordedAudio();
        pronunciationEvaluator.EvaluatePronunciation(audioData, currentWord);
    }

    void ProvideHint()
    {
        string userInput = inputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            // Use the entered word to provide a hint
            pronunciationEvaluator.EvaluatePronunciation(null, userInput);
        }
    }
}