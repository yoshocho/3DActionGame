using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

public partial class NewPlayer : CharacterBase
{
    public class PlayerIdleState : State
    {
        protected override void OnEnter(State prevState)
        {
            if(owner._debagMode) Debug.Log("InIdle");
            if (prevState is PlayerWalkState) owner.PlayAnimation("WalkEnd",0.2f);
            else owner.PlayAnimation("Idle",0.1f);
            owner._currentVelocity.x = 0.0f;
            owner._currentVelocity.z = 0.0f;
        }
        protected override void OnUpdate()
        {
            if (owner.IsGround())
            {
                if (owner._inputAxis.sqrMagnitude > 0.1f) owner.ChangeState(StateEvent.Walk);
                if (owner._inputProvider.GetAvoid()) owner.ChangeState(StateEvent.Avoid);
                if(owner._inputProvider.GetAttack()) owner.ChangeState(StateEvent.Attack);
                if (owner._inputProvider.GetJump() && owner._currentJumpCount < owner._jumpCount)
                    owner.ChangeState(StateEvent.Jump);
            }
            else owner.ChangeState(StateEvent.Fall);
        }
        protected override void OnExit(State nextState)
        {

        }
    }
}