using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NewPlayer>.State;

public partial class NewPlayer : CharacterBase
{
    public class PlayerLandState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("InLand");
            owner._currentVelocity = Vector3.zero;
            owner.PlayAnimation("Land",0.1f);

        }
        protected override void OnUpdate()
        {
            if(owner._inputAxis.magnitude > 0.1f)
            {
                owner.ChangeState(StateEvent.Walk);
            }
            else if (!owner._animCtrl.IsPlayingAnimatin()) 
            {
                owner.ChangeState(StateEvent.Idle);
            }
            if (owner._inputManager.InputActions.Player.Avoid.WasPressedThisFrame())
                owner.ChangeState(StateEvent.Avoid);
            if (owner._inputManager.InputActions.Player.Attack.WasPressedThisFrame())
                owner.ChangeState(StateEvent.Attack);
            if (owner._inputManager.InputActions.Player.Jump.WasPressedThisFrame())
                owner.ChangeState(StateEvent.Jump);
        }
        protected override void OnExit(State nextState)
        {
            
        }
    }
}