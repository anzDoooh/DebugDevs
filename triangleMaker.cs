using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class TriangleMaker : MonoBehaviour
{
    public Button captureButton;
    public Button processButton;
    public RawImage capturedImage;
    public Text hintText;
    public Text resultText;

    private WebCamTexture webCamTexture;
    private Texture2D capturedTexture;
    private bool isCameraActive = false;

    void Start()
    {
        captureButton.onClick.AddListener(ToggleCamera);
        processButton.onClick.AddListener(ProcessImage);
        webCamTexture = new WebCamTexture();
    }

    void ToggleCamera()
    {
        if (isCameraActive)
        {
            CaptureImage();
            webCamTexture.Stop();
            isCameraActive = false;
        }
        else
        {
            webCamTexture.Play();
            capturedImage.texture = webCamTexture;
            isCameraActive = true;
        }
    }

    void CaptureImage()
    {
        if (webCamTexture.isPlaying)
        {
            capturedTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
            capturedTexture.SetPixels(webCamTexture.GetPixels());
            capturedTexture.Apply();
            capturedImage.texture = capturedTexture;
        }
    }

    void ProcessImage()
    {
        if (capturedTexture != null)
        {
            StartCoroutine(SendImageToServer());
        }
        else
        {
            resultText.text = "Please capture an image first.";
        }
    }

    IEnumerator SendImageToServer()
    {
        byte[] imageBytes = capturedTexture.EncodeToPNG();
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:6060/process", UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(imageBytes);
        www.SetRequestHeader("Content-Type", "application/octet-stream");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            resultText.text = "Error: " + www.error;
        }
        else
        {
            var jsonResponse = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
            resultText.text = jsonResponse.result;
        }
    }
}

[System.Serializable]
public class ServerResponse
{
    public string result;
}
