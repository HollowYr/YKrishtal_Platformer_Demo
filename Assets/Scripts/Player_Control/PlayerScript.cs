/***
 * Contains all classes and components that other classes might need.
 * Made to implement only PlayerScript and have all other implemented automatically.
 * Also changes animation states.
 ***/

/// TODO: split animation state changes and 
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] internal PlayerCombo playerCombo;
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal HeartsHealthVisual heartsHealthVisual;

    internal Animator animator;
    internal CharacterController2D characterController2D;
    internal AudioManager audioManager;
    internal Rigidbody2D rb;
    internal bool isHighRankAnimation = false;

    string currentState;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        characterController2D = GetComponent<CharacterController2D>();

        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();
    }

    public string GetCurrentState()
    {
        return currentState;
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
