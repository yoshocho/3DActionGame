using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

partial class NewPlayer : CharacterBase
{
    public class PlayerJumpState : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.PlayAnimation("Jump",0.1f);
            Debug.Log("Jump");
            if (owner._inputAxis.magnitude > 0.1f) owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            owner._currentVelocity = new Vector3(owner._moveForward.x,owner._jumpPower,owner._moveForward.z);

        }
        protected override void OnUpdate()
        {
            if (owner._inputAxis.sqrMagnitude > 0.1f)
            {
                owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            }
            if (owner._inputManager.InputActions.Player.Avoid.WasPressedThisFrame())
                owner.ChangeState(StateEvent.Avoid);
            if (owner._inputManager.InputActions.Player.Attack.WasPressedThisFrame())
                owner.ChangeState(StateEvent.Attack);
            if (owner._inputManager.InputActions.Player.Jump.WasPressedThisFrame())
                owner.ChangeState(StateEvent.Jump);

            if (owner._currentVelocity.y < 0.0f) owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {

        }
    }
}