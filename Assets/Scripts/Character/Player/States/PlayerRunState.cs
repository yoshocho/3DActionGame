using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

public partial class NewPlayer : CharacterBase {
    public class PlayerRunState : State
    {
        protected override void OnEnter(State prevState)
        {
            owner.PlayAnimation("Run",0.2f);
            owner._moveSpeed = owner._runSpeed;
        }
        protected override void OnUpdate()
        {
            if (owner.IsGround())
            {
                if (owner._inputAxis.sqrMagnitude > 0.1f)
                {
                    owner._targetRot = Quaternion.LookRotation(owner._moveForward);
                    owner._currentVelocity = new Vector3(owner._selfTrans.forward.x, owner._currentVelocity.y,
                        owner._selfTrans.forward.z);
                }
                else owner.ChangeState(StateEvent.Idle);

                if (owner._inputManager.InputActions.Player.Attack.WasPressedThisFrame())
                    owner.ChangeState(StateEvent.Attack);
                if (owner._inputManager.InputActions.Player.Avoid.WasPressedThisFrame())
                    owner.ChangeState(StateEvent.Avoid);
                if (owner._inputManager.InputActions.Player.Jump.WasPressedThisFrame())
                    owner.ChangeState(StateEvent.Jump);
            }
            else owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {
            
        }
    }
}