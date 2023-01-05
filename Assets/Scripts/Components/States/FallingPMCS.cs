using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPMCS : PlayerMovementState
{
    public FallingPMCS(PlayerMovementComponent _playerMovement) : base(_playerMovement)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("Falling");
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
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Jumping);
        }

        if (playerMovement.IsGrounded())
        {
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Walking);
        }
    }
}
