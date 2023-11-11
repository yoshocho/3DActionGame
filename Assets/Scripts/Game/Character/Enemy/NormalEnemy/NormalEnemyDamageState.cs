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
            if(owner._debagMode) Debug.Log("InDamage");
            owner.PlayAnim("Damage",0.1f);
            owner._mover.Velocity = Vector3.zero;
        }
        protected override void OnUpdate()
        {
            if (!owner._animCtrl.IsPlayingAnimatin())
            {
                if (owner._distance < owner.ActionParam.AttackRange && !owner._attackCoolTime)
                {
                    owner.ChangeState(StateType.Attack);
                }
                else
                {
                    owner._moveType = MoveType.Walk;
                    owner.ChangeState(StateType.Move);
                }
            }
        }
        protected override void OnExit(State nextState)
        {
            
        }

    }
}