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
            if(owner._debagMode) Debug.Log("Jump");

            owner.PlayAnimation("Jump",0.1f);
            if (owner._inputAxis.magnitude > 0.1f) owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            owner._currentVelocity = new Vector3(owner._moveForward.x,owner._jumpPower,owner._moveForward.z);

        }
        protected override void OnUpdate()
        {
            if (owner._inputAxis.sqrMagnitude > 0.1f) owner._targetRot = Quaternion.LookRotation(owner._moveForward);
            if (owner._inputProvider.GetAvoid()) owner.ChangeState(StateEvent.Avoid);
            if (owner._inputProvider.GetAttack()) owner.ChangeState(StateEvent.Attack);
            if (owner._inputProvider.GetJump()) owner.ChangeState(StateEvent.Jump);
            if (owner._currentVelocity.y < 0.0f) owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {

        }
    }
}