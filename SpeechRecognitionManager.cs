using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;

public class SpeechRecognitionManager : MonoBehaviour
{
    public string flaskServerUrl = "http://localhost:5000/recognize";
    private AudioClip audioClip;
    private bool isRecording = false;
    private string microphone;

    void Start()
    {
        // Get the default microphone
        if (Microphone.devices.Length > 0)
        {
            microphone = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    public void StartRecording()
    {
        if (microphone != null)
        {
            audioClip = Microphone.Start(microphone, false, 10, 16000); // Record for up to 10 seconds
            isRecording = true;
        }
    }

    public void StopRecording()
    {
        if (isRecording && microphone != null)
        {
            Microphone.End(microphone);
            isRecording = false;
            byte[] audioData = GetRecordedAudio();
            StartCoroutine(SendRequest(audioData));
        }
    }

    public byte[] GetRecordedAudio()
    {
        if (audioClip == null)
            return null;

        // Get the audio data
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        // Convert float array to WAV format byte array
        byte[] wavData = ConvertToWAV(samples, audioClip.channels, audioClip.frequency);
        return wavData;
    }

    private byte[] ConvertToWAV(float[] samples, int channels, int sampleRate)
    {
        MemoryStream stream = new MemoryStream();

        // WAV file header
        stream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
        stream.Write(BitConverter.GetBytes((samples.Length * 2) + 36), 0, 4);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
        stream.Write(BitConverter.GetBytes(16), 0, 4); // Subchunk1Size
        stream.Write(BitConverter.GetBytes((short)1), 0, 2); // AudioFormat
        stream.Write(BitConverter.GetBytes((short)channels), 0, 2);
        stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);
        stream.Write(BitConverter.GetBytes(sampleRate * channels * 2), 0, 4); // ByteRate
        stream.Write(BitConverter.GetBytes((short)(channels * 2)), 0, 2); // BlockAlign
        stream.Write(BitConverter.GetBytes((short)16), 0, 2); // BitsPerSample
        stream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
        stream.Write(BitConverter.GetBytes(samples.Length * 2), 0, 4);

        // WAV file data
        foreach (var sample in samples)
        {
            short intSample = (short)(sample * short.MaxValue);
            stream.Write(BitConverter.GetBytes(intSample), 0, 2);
        }

        return stream.ToArray();
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