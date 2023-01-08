using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerMovementState
{
    Walking,
    Jumping,
    Falling
}

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementComponent : PlayerMovementStateMachine
{
    public float MaxWalkSpeed = 3.0f;
    public float MaxSpringSpeed = 6.0f;
    public float JumpForce = 10.0f;
    public float GravityForce = 30.0f;
    bool sprinting;

    public EPlayerMovementState currentPlayerState;
    CharacterController characterController;
    public Vector3 receivedInput;
    public Vector3 velocity;
    public Vector3 launchVelocity;
    public Vector3 velocityXY;
    public bool bCanJump = true;
    public bool CanMoveInAir = true;
    bool bWantsToJump;

    public bool bWantsToUnCrouch;
    bool bIsCrouching;

    public int jumpCount;
    public int MaxJumpCount = 2;

    public float crouchHeight = .50f;
    private float UncrouchHeight;

    [SerializeField] private float groundRayLenght;

    //States
    WalkingPMCS walkingPMCS;
    JumpingPMCS jumpingPMCS;
    FallingPMCS fallingPMCS;

    RaycastHit groundInfo;

    AudioSource audioSource;
    public AudioClip FootSteps;
    public AudioClip JumpClip;
    public AudioClip LandedClip;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        walkingPMCS = new WalkingPMCS(this);
        jumpingPMCS = new JumpingPMCS(this);
        fallingPMCS = new FallingPMCS(this);

        SwitchState(fallingPMCS);

        UncrouchHeight = characterController.height;
        crouchHeight = UncrouchHeight / 2;

        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();

        characterController.Move(velocity * Time.deltaTime);

        if (launchVelocity != Vector3.zero)
        {
            characterController.Move(launchVelocity * Time.deltaTime);
        }

        if (velocity.magnitude == 0)
        {
            characterController.Move(Vector3.down);
        }
    }

    public Vector3 ConsumeInput()
    {
        Vector3 tmp = receivedInput;
        receivedInput = Vector3.zero;
        return tmp;
    }

    public bool WantsToMove()
    {
        return true;
    }

    public bool IsGrounded()
    {
        Vector3 rayPos = transform.position;

        rayPos.y -= characterController.height / 2;

        Debug.DrawRay(rayPos, Vector3.down * groundRayLenght, Color.red);

        if (Physics.Raycast(rayPos, Vector3.down, out groundInfo, groundRayLenght))
        {
            return true;
        }

        return false;
    }

    public void ReceiveInput(Vector3 NewInput)
    {
        receivedInput = NewInput;
    }

    public CharacterController GetCharacterController()
    {
        return characterController;
    }

    public void SetPlayerMovementState(EPlayerMovementState newState)
    {
        switch (newState)
        {
            case EPlayerMovementState.Walking:

                if (currentPlayerState == EPlayerMovementState.Falling || currentPlayerState == EPlayerMovementState.Jumping) 
                {
                    audioSource.PlayOneShot(LandedClip);
                }

                currentPlayerState = EPlayerMovementState.Walking;
                SwitchState(walkingPMCS);
                break;

            case EPlayerMovementState.Jumping:
                currentPlayerState = EPlayerMovementState.Jumping;
                SwitchState(jumpingPMCS);
                break;

            case EPlayerMovementState.Falling:
                currentPlayerState = EPlayerMovementState.Falling;
                SwitchState(fallingPMCS);
                break;
        }
    }
    public void Move(Vector3 Direction)
    {
        velocity.x = Direction.x;
        velocity.z = Direction.z;
    }

    public void Jump()
    {
        jumpCount++;

        //reset Y Velocity to 0 before trying to jump, so that gravity wont accumulate
        StopYVelocity();
        velocity.y += JumpForce;

        audioSource.PlayOneShot(JumpClip);
    }

    public void PerformJump()
    {
        if (bCanJump)
        {
            if (jumpCount < MaxJumpCount)
            {
                bWantsToJump = true;
            }
        }
    }

    public bool ConsumeWantsToJump()
    {
        bool tmp = bWantsToJump;
        bWantsToJump = false;
        return tmp;
    }

    public void ApplyGravity()
    {
        velocity.y -= GravityForce * Time.deltaTime;

        //Set a max velocity for falling? Optional
        velocity.y = Mathf.Clamp(velocity.y, -60.0f, velocity.y);
    }

    public void ResetJumpCount()
    {
        jumpCount = 0;
    }

    public bool CanJump()
    {
        if (jumpCount < MaxJumpCount)
        {
            return true;
        }

        return false;
    }

    public void StopYVelocity()
    {
        velocity.y = 0;
    }

    public void StopMovement(bool StopYVelocity)
    {
        velocity.x = 0;
        velocity.z = 0;

        if (StopYVelocity)
        {
            velocity.y = 0;
        }
    }

    public float GetVelocityXYLenght()
    {
        velocityXY.Set(velocity.x, 0, velocity.z);
        return velocityXY.magnitude;
    }

    public void SetIsSprinting(bool newSprinting)
    {
        sprinting = newSprinting;
    }

    public void LaunchCharacter(Vector3 Direction)
    {
        if (Direction != Vector3.zero)
        {
            //We launch the character
            launchVelocity = Direction;
        }
    }

    public void OnLaunchCharacterLand()
    {
        launchVelocity = Vector3.zero;
    }

    public bool GetIsSprinting()
    {
        return sprinting;
    }

    public bool IsCrouching()
    {
        return bIsCrouching;
    }

    public void Crouch()
    {
        if (IsGrounded() && !bIsCrouching)
        {
            characterController.height = crouchHeight;
            StopYVelocity();

            bIsCrouching = true;
        }
    }

    public void UnCrouch()
    {
        if (bIsCrouching)
        {
            bWantsToUnCrouch = true;
        }
    }

    public bool CanUncrouch()
    {
        Vector3 rayPos = transform.position;

        rayPos.y += characterController.height / 2;

        Debug.DrawRay(rayPos, Vector3.up * groundRayLenght, Color.red);

        if (Physics.Raycast(rayPos, Vector3.up, out groundInfo, groundRayLenght + .5f))
        {
            return false;
        }

        return true;
    }

    public void UnCrouchCharacter() 
    {
        if (CanUncrouch())
        {
            characterController.height = UncrouchHeight;
            bIsCrouching = false;
            bWantsToUnCrouch = false;
        }
    }
}

