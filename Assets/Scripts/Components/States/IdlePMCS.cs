using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePMCS : PlayerMovementState
{
    public IdlePMCS(PlayerMovementComponent _playerMovement) : base(_playerMovement)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("Idle");

        playerMovement.velocity.y = 0;
        playerMovement.ResetJumpCount();
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateFixedUpdate()
    {

    }

    public override void OnStateUpdate()
    {
        if (playerMovement.ConsumeWantsToJump() && playerMovement.IsGrounded())
        {
            //We jump!
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Jumping);
        }

        if (playerMovement.WantsToMove()) 
        {
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Walking);
        }

        if (!playerMovement.IsGrounded()) 
        {
            playerMovement.SetPlayerMovementState(EPlayerMovementState.Falling);
        }
    }
}
