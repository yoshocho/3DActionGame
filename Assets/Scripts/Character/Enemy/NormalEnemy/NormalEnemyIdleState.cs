using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NormalStateEnemy>.State;

public partial class NormalStateEnemy : EnemyBase
{
    public class NormalEnemyIdleState : State
    {
        protected override void OnEnter(State prevState)
        {
            if (owner._debagMode) Debug.Log("InIdle");
            //owner._mover.Velocity = new Vector3(0.0f, owner._mover.Velocity.y, 0.0f);
            owner._currentVelocity.x = 0.0f;
            owner._currentVelocity.z = 0.0f;
            owner.PlayAnim("Idle");
        }
        protected override void OnUpdate()
        {
            if (owner._mover.IsGround())
            {
                if (owner._distance > owner._attackRange) owner.ChangeState(StateType.Chase);
            }
        }
        protected override void OnExit(State nextState)
        {

        }

    }
}