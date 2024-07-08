using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    public Button helpButton;
    public Button continueButton;

    void Start()
    {
        // Ensure the instructions panel is initially hidden
        instructionsPanel.SetActive(false);

        // Add listeners for the buttons
        helpButton.onClick.AddListener(ShowInstructions);
        continueButton.onClick.AddListener(HideInstructions);
    }

    void ShowInstructions()
    {
        // Pause the game
        Time.timeScale = 0;
        instructionsPanel.SetActive(true);
    }

    void HideInstructions()
    {
        // Resume the game
        Time.timeScale = 1;
        instructionsPanel.SetActive(false);
    }
}