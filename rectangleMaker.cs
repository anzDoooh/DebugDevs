using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class rectangleMaker : MonoBehaviour
{
    public Text hintText;
    public Button captureButton;
    public Button processButton;
    public RawImage capturedImage;
    public Text resultText;

    private WebCamTexture webCamTexture;
    private Texture2D capturedTexture;
    private bool isCameraActive = false;

    void Start()
    {
        captureButton.onClick.AddListener(OnCaptureButtonClick);
        processButton.onClick.AddListener(OnProcessButtonClick);
        hintText.text = "Draw a picture of rectangle in a paper and upload it.This shape has four sides, with opposite sides being equal in length. It looks like a door or a piece of paper.";
        resultText.text = "";
    }

    void OnCaptureButtonClick()
    {
        if (!isCameraActive)
        {
            // Initialize and start the webcam texture
            webCamTexture = new WebCamTexture();
            capturedImage.texture = webCamTexture;
            capturedImage.material.mainTexture = webCamTexture;
            webCamTexture.Play();
            isCameraActive = true;
            captureButton.GetComponentInChildren<Text>().text = "Capture Image";
        }
        else
        {
            // Capture the image
            StartCoroutine(CaptureImageCoroutine());
        }
    }

    IEnumerator CaptureImageCoroutine()
    {
        // Wait a short delay to ensure the webcam texture starts capturing
        yield return new WaitForSeconds(0.5f); // Adjust delay time if needed

        // Capture the image
        capturedTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
        capturedTexture.SetPixels(webCamTexture.GetPixels());
        capturedTexture.Apply();
        capturedImage.texture = capturedTexture;

        // Stop the webcam texture after capturing
        webCamTexture.Stop();
        isCameraActive = false;
        captureButton.GetComponentInChildren<Text>().text = "Open Camera";
    }

    void OnProcessButtonClick()
    {
        StartCoroutine(ProcessImageCoroutine());
    }

    IEnumerator ProcessImageCoroutine()
    {
        if (capturedTexture == null)
        {
            resultText.text = "Please capture an image first.";
            yield break;
        }

        byte[] imageBytes = capturedTexture.EncodeToPNG();

        // Update the URL to match your Flask server's URL
        string serverUrl = "http://127.0.0.1:5060/process"; // Replace with your actual server URL

        UnityWebRequest www = UnityWebRequest.Post(serverUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(imageBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/octet-stream");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            resultText.text = "Error: " + www.error;
        }
        else
        {
            string result = www.downloadHandler.text;
            if (result.Contains("Correct"))
            {
                resultText.text = "Correct! This is a rectangle.";
                // Proceed with game logic for correct shape detection
            }
            else
            {
                resultText.text = "Wrong! Please try again.";
                // Prompt user to try capturing the shape again
            }
        }
    }
}
