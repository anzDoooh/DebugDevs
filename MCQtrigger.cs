using UnityEngine;
using UnityEngine.UI;
using TMPro; // for using tmp text

public class QuestionTrigger : MonoBehaviour
{
    public GameObject canvasToShow; // Reference to the Canvas GameObject
    public MonoBehaviour playerMovementScript; // Reference to the player's movement script
    public Button option1Button; // Reference to Option 1 Button
    public Button option2Button; // Reference to Option 2 Button
    public Button option3Button; // Reference to Option 3 Button
    public Button option4Button; // Reference to Option 4 Button
    public TMP_Text questionText; // Reference to the Text component for the question
    
    private int attemptCount;
    private void Start(){
        option1Button.onClick.AddListener(() => OnOptionSelected(1));
        option2Button.onClick.AddListener(() => OnOptionSelected(2));
        option3Button.onClick.AddListener(() => OnOptionSelected(3));
        option4Button.onClick.AddListener(() => OnOptionSelected(4));
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            canvasToShow.SetActive(true); // Show the Canvas when the player enters the trigger zone
            playerMovementScript.enabled = false; // Disable the player's movement script
            DisplayQuestion();

            attemptCount = 0;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            canvasToShow.SetActive(false); // Hide the Canvas when the player exits the trigger zone
            playerMovementScript.enabled = true; // Re-enable the player's movement script
        }
    }


    private void DisplayQuestion(){
        questionText.text = "I ______ an apple."; // Set your question here

        // Set options text
        option1Button.GetComponentInChildren<Text>().text = "drink";
        option2Button.GetComponentInChildren<Text>().text = "eat"; // Correct answer
        option3Button.GetComponentInChildren<Text>().text = "walk";
        option4Button.GetComponentInChildren<Text>().text = "play";
    }

    private void OnOptionSelected(int optionNumber){
        attemptCount++;
    if (optionNumber == 2){
        Debug.Log("Correct Answer");

        if (canvasToShow != null){
            canvasToShow.SetActive(false); // Hide the Canvas
        }
        else{
            Debug.LogError("canvasToShow is null");
        }

        if (playerMovementScript != null){
            playerMovementScript.enabled = true; // Re-enable the player's movement script
        }
        else{
            Debug.LogError("playerMovementScript is null");
        }

        if (attemptCount == 1){
            if (ScoreManagement.Instance != null){
                ScoreManagement.Instance.AddScore(50);
            }
            else{
                Debug.LogError("ScoreManagement.Instance is null");
            }
        }
        // else{
        //     ScoreManagement.Instance.AddScore(25);
        // }
    }
        else{
            Debug.Log("Incorrect Answer");
            // Optionally, provide feedback or allow the player to try again
        }
    }
}