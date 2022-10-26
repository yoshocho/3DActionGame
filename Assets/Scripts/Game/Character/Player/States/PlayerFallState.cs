using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<PlayerStateMachine>.State;

public partial class PlayerStateMachine : CharacterBase
{
    public class PlayerFallState : State
    {
        protected override void OnEnter(State prevState)
        {
           if(owner._debagMode) Debug.Log("InFall");
            owner.PlayAnimation("Fall", 0.1f);
        }
        protected override void OnUpdate()
        {
            if (owner._inputAxis.sqrMagnitude > 0.1f)
            {
                owner._targetRot = Quaternion.LookRotation(owner._moveForward);
                owner._mover.Velocity = new Vector3(
                    owner._moveForward.x * owner._airDeceleration,
                    owner._mover.Velocity.y,
                    owner._moveForward.z);
            }
            if (owner._inputProvider.GetJump() && owner._currentJumpCount < owner._jumpCount)
                owner.ChangeState(StateEvent.Jump);
            //§ŒÀ
            if (owner._inputProvider.GetAvoid()) owner.ChangeState(StateEvent.Avoid);
            if (owner._inputProvider.GetAttack()) owner.ChangeState(StateEvent.Attack);
            if (owner.IsGround()) owner.ChangeState(StateEvent.Land);

        }
        protected override void OnExit(State nextState)
        {

        }
    }
}