using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using State = StateMachine<NormalStateEnemy>.State;

public partial class NormalStateEnemy : EnemyBase
{
    public class NormalEnemyAttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            if (owner._debagMode) Debug.Log("InAttack");

            owner._mover.Velocity = new Vector3(0.0f,owner._mover.Velocity.y,0.0f);

            owner._actCtrl.RequestAction(AttackType.Weak);
            owner._attackCoolTime = true;
            owner.StartCoroutine(AttackCoolTime());
        }
        protected override void OnUpdate()
        {
            if (!owner._actCtrl.ActionKeep)
            {
                if (owner._distance > owner.ActionParam.AttackRange)
                    owner.ChangeState(StateType.Move);
                else
                {

                    owner.ChangeState(StateType.Idle);
                }
            }
        }

        IEnumerator AttackCoolTime()
        {
            yield return new WaitForSeconds(owner.ActionParam.AttackCoolTime);
            owner._attackCoolTime = false;
        }
        protected override void OnExit(State nextState)
        {
        }
    }
}