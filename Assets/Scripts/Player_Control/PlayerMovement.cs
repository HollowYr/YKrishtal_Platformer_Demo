/***
 * Take input for player movement, transfer it to CharacterController2D and control the movement animation.
 ***/

// TODO: Split PlayerMovement to MovementAnimation and MovementInput

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;

    [SerializeField] internal CharacterController2D controller;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private float dashDistance = 15f, dashDelay = .4f;



    internal float horizontalMove = 0f;
    bool jumpInput = false,
        crouchInput = false, crouchState;

    private void FixedUpdate()
    {
        // sending input to movement Logic
        if (!controller.isDashing)
            controller.Move(horizontalMove * GameTime.Instance.fixedDeltaTime, crouchInput, jumpInput);
    }

    void Update()
    {
        if (!GameTime.Instance.isPaused)
            horizontalMove = fixedJoystick.Horizontal * runSpeed;
        else horizontalMove = 0f;

#if UNITY_EDITOR
        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

#endif

        if (playerScript.heartsHealthVisual.IsDead())
        {
            playerScript.ChangeAnimationState("Player_Die");
        }
        else
        {
            if (Mathf.Abs(horizontalMove) > 0 && controller.isGrounded())
            {
                if (crouchState)
                {
                    playerScript.ChangeAnimationState("Player_Crouch_Walk");
                }
                else
                {
                    playerScript.ChangeAnimationState("Player_Run");
                }
            }
            else if (Mathf.Abs(horizontalMove) == 0f && controller.isGrounded() && (!playerScript.isHighRankAnimation))
            {
                if (crouchState)
                {
                    playerScript.ChangeAnimationState("Player_Crouch");
                }
                else
                {
                    playerScript.ChangeAnimationState("Player_Idle");
                }
            }
        }


        if (playerScript.rb.velocity.y < -1f)
        {
            playerScript.ChangeAnimationState("Player_Fall");
        }

    }

    private void CreateDust()
    {
        particleSystem.Play();
    }

    public void Dash()
    {
        playerScript.ChangeAnimationState("Player_Dash");
        if (controller.isGrounded())
        {
            CreateDust();
        }

        controller.Dash(dashDistance, dashDelay);

    }

    public void crouchDown()
    {
        crouchInput = true;
        crouchState = crouchInput;
    }

    public void crouchUp()
    {
        crouchInput = false;
        if (!playerScript.characterController2D.isUnderCeiling())
        {
            crouchState = crouchInput;
        }
    }

    public void OnCrouching(bool isCrouching)
    {
        crouchState = isCrouching;
    }
    public void Jump()
    {
        if (!crouchState && controller.isGrounded())
        {
            jumpInput = true;
            playerScript.ChangeAnimationState("Player_Jump");
            CreateDust();
        }
    }

    public void onLanding()
    {
        jumpInput = false;
        CreateDust();
    }

}
