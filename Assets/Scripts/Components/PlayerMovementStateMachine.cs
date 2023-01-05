using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : MonoBehaviour
{
    private PlayerMovementState currentState;

    protected virtual void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnStateFixedUpdate();
        }
    }

    public void SwitchState(PlayerMovementState newState)
    {
        if (newState == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = newState;

        currentState.OnStateEnter();

    }
}
