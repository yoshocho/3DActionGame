using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

public partial class NewPlayer : CharacterBase
{
    public class PlayerWalkState : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.PlayAnimation("Walk",0.1f);
            owner._moveSpeed = owner._walkSpeed;
        }
        protected override void OnUpdate()
        {
            if (owner.IsGround())
            {
                if (owner._inputAxis.magnitude > 0.1f)
                {
                    owner._targetRot = Quaternion.LookRotation(owner._moveForward);
                    owner._currentVelocity = new Vector3(owner._selfTrans.forward.x, owner.
                        _currentVelocity.y, owner._selfTrans.forward.z);
                    if (owner._inputManager.InputActions.Player.Attack.WasPressedThisFrame())
                        owner.ChangeState(StateEvent.Attack);
                    if (owner._inputManager.InputActions.Player.Avoid.WasPressedThisFrame())
                        owner.ChangeState(StateEvent.Avoid);
                    if (owner._inputManager.InputActions.Player.Jump.WasPressedThisFrame())
                        owner.ChangeState(StateEvent.Jump);
                }
                else owner.ChangeState(StateEvent.Idle);
            }
            else owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {

        }
    }
}