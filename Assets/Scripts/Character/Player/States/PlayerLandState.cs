using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<PlayerStateMachine>.State;

public partial class PlayerStateMachine : CharacterBase
{
    public class PlayerLandState : State
    {
        protected override void OnEnter(State prevState)
        {
            if(owner._debagMode)Debug.Log("InLand");
            owner._mover.Velocity = Vector3.zero;
            owner.PlayAnimation("Land",0.1f);
            owner._currentJumpCount = 0;
        }
        protected override void OnUpdate()
        {
            if(owner._inputAxis.magnitude > 0.1f) owner.ChangeState(StateEvent.Run);
            else if (!owner._animCtrl.IsPlayingAnimatin()) owner.ChangeState(StateEvent.Idle);

            if (owner._inputProvider.GetAvoid()) owner.ChangeState(StateEvent.Avoid);
            if (owner._inputProvider.GetAttack()) owner.ChangeState(StateEvent.Attack);
            if (owner._inputProvider.GetJump()) owner.ChangeState(StateEvent.Jump);
        }
        protected override void OnExit(State nextState)
        {
            
        }
    }
}