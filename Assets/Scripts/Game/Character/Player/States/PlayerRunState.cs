using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<PlayerStateMachine>.State;

public partial class PlayerStateMachine : CharacterBase {
    public class PlayerRunState : State
    {
        protected override void OnEnter(State prevState)
        {
            if (owner._debagMode) Debug.Log("RunState");
            owner.PlayAnimation("Sprint",0.2f);
            owner._mover.SetMoveSpeed = owner._runSpeed;
        }
        protected override void OnUpdate()
        {
            if (owner.IsGround())
            {
                if (owner._inputAxis.sqrMagnitude > 0.1f)
                {
                    owner._targetRot = Quaternion.LookRotation(owner._moveForward);
                    owner._mover.Velocity = new Vector3(owner._selfTrans.forward.x, owner.
                       _mover.Velocity.y, owner._selfTrans.forward.z);
                }
                else owner.ChangeState(StateEvent.Idle);

                if (owner._inputProvider.GetAttack(InputType.Down)) owner.ChangeState(StateEvent.Attack);
                if (owner._inputProvider.GetAvoid(InputType.Down)) owner.ChangeState(StateEvent.Avoid);
                if (owner._inputProvider.GetJump(InputType.Down)) owner.ChangeState(StateEvent.Jump);
            }
            else owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {

        }
    }
}