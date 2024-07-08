using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PronunciationEvaluator : MonoBehaviour
{
    public string flaskServerUrl = "http://localhost:5000/evaluate";
    public TextToSpeech TextToSpeech; // Reference to TextToSpeech for hint audio

    public void EvaluatePronunciation(byte[] audioData, string word)
    {
        if (audioData != null)
        {
            StartCoroutine(SendRequest(audioData, word));
        }
        else
        {
            Debug.LogError("Audio data is null!");
        }
    }

    private IEnumerator SendRequest(byte[] audioData, string word)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");
        form.AddField("word", word);

        using (UnityWebRequest www = UnityWebRequest.Post(flaskServerUrl, form))
        {
            Debug.Log("Sending request to: " + flaskServerUrl);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log("Pronunciation evaluation result: " + result);
                // Parse the result and handle it
                // Assuming result contains hint information
                var jsonResult = JsonUtility.FromJson<PronunciationResult>(result);
                if (TextToSpeech != null)
                {
                    TextToSpeech.Speak(jsonResult.hint);
                }
            }
        }
    }

    [System.Serializable]
    public class PronunciationResult
    {
        public float score;
        public string hint;
    }
}