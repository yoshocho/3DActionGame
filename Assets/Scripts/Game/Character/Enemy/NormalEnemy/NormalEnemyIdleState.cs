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
            owner._mover.Velocity = new Vector3(0.0f, owner._mover.Velocity.y, 0.0f);
            owner.PlayAnim("Idle");
        }
        protected override void OnUpdate()
        {
            if (owner._mover.IsGround())
            {
                if (owner._distance > owner.ActionParam.AttackRange && !owner._attackCoolTime)
                {
                    owner._moveType = MoveType.Walk;
                    owner.ChangeState(StateType.Move);
                }
                else if(!owner._attackCoolTime) owner.ChangeState(StateType.Attack);
                else
                {
                    //owner._moveType = MoveType.Stafe;
                    //owner.ChangeState(StateType.Move);
                }
            }
        }
        protected override void OnExit(State nextState)
        {

        }

    }
}