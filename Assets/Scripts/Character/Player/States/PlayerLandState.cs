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
            base.OnEnter(prevState);
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            base.OnExit(nextState);
        }
    }
}