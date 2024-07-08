using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGame : MonoBehaviour
{
    public void ReturnToMainGame()
    {
        SceneManager.LoadScene("MainGameScene"); // Replace with your main game scene name
    }
}
