using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpeechRecognitionManager : MonoBehaviour
{
    public string flaskServerUrl = "http://localhost:5000/recognize";

    public void StartRecording()
    {
        // Start recording logic
    }

    public void StopRecording(byte[] audioData)
    {
        StartCoroutine(SendRequest(audioData));
    }

    private IEnumerator SendRequest(byte[] audioData)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", audioData, "audio.wav", "audio/wav");

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
                Debug.Log("Speech recognition result: " + result);
                // Parse the result and handle it
            }
        }
    }
}