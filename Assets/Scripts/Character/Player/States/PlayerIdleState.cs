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
            //owner.PlayAnimation();
        }
        protected override void OnUpdate()
        {

        }
        protected override void OnExit(State nextState)
        {

        }
    }
}