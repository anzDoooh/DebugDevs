using UnityEngine;

public class PlayerAnimation : MonoBehaviour{
    // Start is called before the first frame update
    public static PlayerAnimation Instance{get; private set;}
    private Animator animator;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    void Start(){
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void SetWalking(bool isWalking){
        animator.SetBool("isWalking", isWalking);
        
    }
}
