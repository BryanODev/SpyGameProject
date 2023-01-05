using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPMCS : PlayerMovementState
{
    public WalkingPMCS(PlayerMovementComponent _playerMovement) : base(_playerMovement)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("Walking");

        playerMovement.velocity.y = 0;
        playerMovement.ResetJumpCount();
        playerMovement.OnLaunchCharacterLand();
    }

    public override void OnStateExit()
    {
        Debug.Log("Exit Walking");
        playerMovement.StopMovement(false);
    }

    public override void OnStateFixedUpdate()
    {

    }

    public override void OnStateUpdate()
    {
        if (playerMovement.GetIsSprinting())
        {
            playerMovement.Move(playerMovement.ConsumeInput() * playerMovement.MaxSpringSpeed);
        }
        else
        {
            playerMovement.Move(playerMovement.ConsumeInput() * playerMovement.MaxWalkSpeed);
        }

        if (playerMovement.ConsumeWantsToJump() && playerMovement.IsGrounded())
        {
            //We jump!
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Jumping);
        }

        if (!playerMovement.IsGrounded()) 
        {
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Falling);
        }


        if (playerMovement.bWantsToUnCrouch)
        {
            playerMovement.UnCrouchCharacter();
        }
    }
}
