using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState
{
    protected PlayerMovementComponent playerMovement;

    public PlayerMovementState(PlayerMovementComponent _playerMovement)
    {
        playerMovement = _playerMovement;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
    public abstract void OnStateExit();
}
