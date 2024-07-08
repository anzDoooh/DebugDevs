using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManagement : MonoBehaviour{

    public static ScoreManagement Instance;
    public TMP_Text scoreText;
    private int score = 0;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Update(){
        UpdateScoreText();
    }
    public void AddScore(int points){
            score += points;
            UpdateScoreText();
    }


    private void UpdateScoreText(){
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public int GetScore(){
        return score;
    }

    public void ResetScore(){
        score = 0;
        UpdateScoreText();
    }

}


