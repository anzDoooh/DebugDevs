using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardPanel; // Reference to the panel containing leaderboard UI elements
    public Button viewLeaderboardButton; // Reference to the button to view leaderboard
    public Button returnToGameButton; // Reference to the button to return to game

    public Text leaderboardText; // Reference to the UI Text element to display leaderboard
    public ScrollRect scrollView; // Reference to the ScrollView UI element

    void Start()
    {
        // Initially hide the leaderboard panel
        leaderboardPanel.SetActive(false);

        // Add button click listeners
        viewLeaderboardButton.onClick.AddListener(OnViewLeaderboardClicked);
        returnToGameButton.onClick.AddListener(OnReturnToGameClicked);
    }

    void OnViewLeaderboardClicked()
    {
        // Show the leaderboard panel
        leaderboardPanel.SetActive(true);

        // Fetch and display leaderboard
        StartCoroutine(FetchLeaderboard());
    }

    void OnReturnToGameClicked()
    {
        // Logic to return to the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameScene");
    }

    IEnumerator FetchLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/unity/leaderboard.php");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to fetch leaderboard: " + www.error);
        }
        else
        {
            string jsonText = www.downloadHandler.text;
            Debug.Log("Received JSON: " + jsonText); // Log the received JSON for debugging
            PlayerDataList players = JsonUtility.FromJson<PlayerDataList>(jsonText);

            if (players == null || players.players == null)
            {
                Debug.Log("Failed to parse JSON: " + jsonText);
                yield break;
            }

            DisplayLeaderboard(players.players);
        }
    }

    void DisplayLeaderboard(List<PlayerData> players)
    {
        // Clear previous entries
        foreach (Transform child in scrollView.content.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate leaderboard entries
        foreach (PlayerData player in players)
        {
            GameObject entry = new GameObject("LeaderboardEntry", typeof(RectTransform));
            entry.transform.SetParent(scrollView.content.transform, false);

            Text entryText = entry.AddComponent<Text>();
            entryText.text = $"{player.username}: {player.score}";
            entryText.font = leaderboardText.font;
            entryText.fontSize = leaderboardText.fontSize;
            entryText.color = leaderboardText.color;
            entryText.alignment = leaderboardText.alignment;
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string username;
    public int score;
}

[System.Serializable]
public class PlayerDataList
{
    public List<PlayerData> players;
}
