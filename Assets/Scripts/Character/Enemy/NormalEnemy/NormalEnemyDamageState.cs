using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NormalStateEnemy>.State;

public partial class NormalStateEnemy : EnemyBase
{
    public class NormalEnemyDamageState : State
    {
        protected override void OnEnter(State prevState)
        {
            base.OnEnter(prevState);
        }


    }
}