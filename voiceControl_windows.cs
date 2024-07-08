using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class VoiceControl : MonoBehaviour {
    public float position_change = 1000f;
    public float continuousMoveSpeed = 1f;
    public float jumpForce = 5f;
    public float rotationSpeed = 2f;
    public Rigidbody rb;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private bool isMovingForwardContinuously = false;
    private bool isStoppedDueToCollision = false; // collision controlling
    // control the animation of theplayer moment
    private Animator animator;
    void Start(){
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        

        actions.Add("walk", Forward);
        actions.Add("go back", Backward);
        actions.Add("left", Left);
        actions.Add("right", Right);
        actions.Add("stop", StopMovingForward);
        actions.Add("turn left", TakeLeft);
        actions.Add("turn right", TakeRight);

        actions.Add("restart", RestartLevel); // optionla for the development process
        


        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }    

    void Update(){
        if (isMovingForwardContinuously && !isStoppedDueToCollision){
            transform.Translate(0, 0, continuousMoveSpeed * Time.deltaTime);
        }
    }

    private void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void RecognizedSpeech(PhraseRecognizedEventArgs speech){
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Forward(){
        isStoppedDueToCollision = false;
        animator.SetBool("walkAlong", true);
        isMovingForwardContinuously = true;
    }

    private void Backward(){
        if(!isStoppedDueToCollision){
            StopMovingForward();
            StartCoroutine(UturnAndMove());
        }
    }

    private IEnumerator UturnAndMove() {
        // Perform 180-degree rotation
        yield return Rotate(Vector3.up, -180, 2.0f);
        // Set animation and start forward movement after rotation is complete
        animator.SetBool("walkAlong", true);
        isMovingForwardContinuously = true;
    }

    private void StopMovingForward(){
        animator.SetBool("walkAlong", false);
        isMovingForwardContinuously = false;
    }

    private void Left(){
        //transform.Translate(-1, 0, 0);
        if(!isStoppedDueToCollision){
            rb.MovePosition(transform.position - transform.right);
        }
    }

    private void Right(){
        //transform.Translate(1, 0, 0);
        if(!isStoppedDueToCollision){
            rb.MovePosition(transform.position + transform.right);
        }
    } 


     private void TakeRight() {
//        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        StartCoroutine(Rotate(Vector3.up, 90, 1.0f));
    }

    private void TakeLeft() {
//        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        StartCoroutine(Rotate(Vector3.up, -90, 1.0f));
    }

    private IEnumerator Rotate(Vector3 axis, float angle, float duration) {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation * Quaternion.Euler(axis * angle);
        for (float t = 0; t < duration; t += Time.deltaTime) {
            transform.rotation = Quaternion.Slerp(from, to, t / duration);
            yield return null;
        }
        transform.rotation = to;
    }

    private bool Isgrounded(){
        return Physics.Raycast(transform.position, Vector3.down, 3f);
    }

    private void OnCollisionEnter(Collision collision){
        Debug.Log("Collied with" + collision.gameObject.name);

        if(collision.gameObject.CompareTag("Obstacles")){
            StopMovingForward();
            isStoppedDueToCollision = true;
            // rb.velocity = Vector3.zero;
            // rb.angularVelocity = Vector3.zero; // FDP
        }
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("Enter trigger with" + other.gameObject.name);

        // if(other.gameObject.name == "TrigerZone"){

        // }
    }

    private void OnTriggerExit(Collider other){
        Debug.Log("Exited trigger with " + other.gameObject.name);

    }

    // private IEnumerator ResetCollisionFlagAfterDelay(float delay){
    //     yield return new WaitForSeconds(delay);
    //     isStoppedDueToCollision = false;
    // }
}
