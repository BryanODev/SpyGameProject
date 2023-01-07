using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPMCS : PlayerMovementState
{
    public JumpingPMCS(PlayerMovementComponent _playerMovement) : base(_playerMovement)
    {

    }

    public override void OnStateEnter()
    {
        if (playerMovement.IsCrouching()) 
        {
            playerMovement.UnCrouch();

            return;
        }

        Debug.Log("Jumping");
        playerMovement.Jump();
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateFixedUpdate()
    {

    }

    public override void OnStateUpdate()
    {
        if (playerMovement.CanMoveInAir) 
        {
            //playerMovement.Move(playerMovement.ConsumeInput() * playerMovement.MaxWalkSpeed);

            if (playerMovement.GetIsSprinting())
            {
                playerMovement.Move(playerMovement.ConsumeInput() * playerMovement.MaxSpringSpeed);
            }
            else
            {
                playerMovement.Move(playerMovement.ConsumeInput() * playerMovement.MaxWalkSpeed);
            }
        }

        playerMovement.ApplyGravity();

        if (playerMovement.ConsumeWantsToJump() && playerMovement.CanJump())
        {
            //We jump again if we can!
            playerMovement.Jump();
        }

        if (playerMovement.velocity.y < 0)
        {
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Falling);
        }

        if ((playerMovement.GetCharacterController().collisionFlags & CollisionFlags.Above) != 0)
        {
            playerMovement.StopYVelocity();
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Falling);
        }
    }
}
