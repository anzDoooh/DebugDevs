using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    public GameObject panel;
    public Text messageText;
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(Hide);
        Hide(); // Hide the panel initially
    }

    public void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}