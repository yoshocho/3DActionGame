using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using State = StateMachine<NormalStateEnemy>.State;

public partial class NormalStateEnemy : EnemyBase
{
    public class NormalEnemyAttackState : State
    {
        float _waitTimer = 0.0f;
        protected override void OnEnter(State prevState)
        {
            if (owner._debagMode) Debug.Log("InAttack");

            //owner._mover.Velocity = new Vector3(0.0f,owner._mover.Velocity.y,0.0f);
            //_waitTimer = owner._waitTime;
            owner._currentVelocity.x = 0.0f;
            owner._currentVelocity.z = 0.0f;
            owner._actCtrl.RequestAction(AttackType.Weak);
        }
        protected override void OnUpdate()
        {
            _waitTimer += Time.deltaTime;

            if (owner._distance < owner._attackRange)
            {
                switch (owner._attackType)
                {
                    case Attack.Single:
                        if (_waitTimer > owner._waitTime)
                        {
                            owner._actCtrl.RequestAction(AttackType.Weak);
                            _waitTimer = 0.0f;
                        }
                        break;
                    case Attack.Combo:
                        break;
                    default:
                        break;
                }
            }
            else if (!owner._actCtrl.ActionKeep)
            {
                if (owner._distance > owner._attackRange) owner.ChangeState(StateType.Chase);
            }
        }
        protected override void OnExit(State nextState)
        {
            _waitTimer = 0.0f;
        }
    }
}