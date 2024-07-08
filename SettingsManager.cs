using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;

    void Start()
    {
        settingsPanel.SetActive(false); // Hide the settings panel initially
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true); // Show the settings panel
        Time.timeScale = 0; // Pause the game
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Hide the settings panel
        Time.timeScale = 1; // Resume the game
    }

    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void ToggleMute()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void GoToHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Logout()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void DeleteAccount()
    {
        // Implement account deletion logic here
        SceneManager.LoadScene("LoginScene");
    }
}