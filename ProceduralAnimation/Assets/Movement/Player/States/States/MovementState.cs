﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : BaseState
{
    PlayerStateController stateController;

    public MovementState(PlayerStateController controller) : base(controller.gameObject)
    {
        stateController = controller;
    }

    public override void Enter()
    {
        Debug.Log("MoveState: Enter");
        stateController._movementComponent.disableMovement = false;
    }

    public override void Exit()
    {
        Debug.Log("MoveState: Exit");
        stateController._movementComponent.disableMovement = true;
    }

    public override Type Tick()
    {
        // Idle
        if (stateController._movementComponent.moveInput.magnitude == 0)
        {
            return typeof(IdleState);
        }

        //Dodge
        if (stateController._dodgeComponent.dodgeInput != -1)
        {
            return typeof(DodgeState);
        }

        ////Attack
        //if (stateController.heavyAttackInput || stateController.quickAttackInput || stateController.powerInput > 0)
        //{
        //    if (stateController.AttackStateReturnDelay <= Time.time)
        //    {
        //        return typeof(AttackState);
        //    }
        //    else
        //    {
        //        //If Inputed attack before they can return to the attack state, remove the input
        //    }
        //}

        //Dead
        if (stateController._playerAttributes.getHealth() <= 0)
        {
            return typeof(DeathState);
        }

        //Stunned
        if (stateController.Stunned)
        {
            return typeof(StunnedState);
        }

        //Respawn (Already in the respawn to state)
        if (stateController.Respawn)
        {
            stateController._animHandler.Respawn();
            stateController.Respawn = false;
        }

        return null;
    }
}