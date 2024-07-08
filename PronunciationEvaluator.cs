using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PronunciationEvaluator : MonoBehaviour
{
    public string flaskServerUrl = "http://localhost:5000/evaluate";

    public void EvaluatePronunciation(byte[] audioData, string word)
    {
         StartCoroutine(SendRequest(audioData, word));
    }

    private IEnumerator SendRequest(byte[] audioData, string word)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");
        form.AddField("word", word);

        using (UnityWebRequest www = UnityWebRequest.Post(flaskServerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log("Pronunciation evaluation result: " + result);
                // Parse the result and handle it
            }
        }
    }
}