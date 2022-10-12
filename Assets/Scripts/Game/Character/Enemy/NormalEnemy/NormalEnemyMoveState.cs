using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<NormalStateEnemy>.State;

public partial class NormalStateEnemy : EnemyBase
{
    public class NormalEnemyMoveState : State
    {
        Vector3 _axis = Vector3.zero;
        protected override void OnEnter(State prevState)
        {
            if (owner._debagMode) Debug.Log("InChase");
            owner.PlayAnim("Run", 0.2f);
        }
        protected override void OnUpdate()
        {

            if (owner._mover.IsGround())
            {
                _axis = (owner._targetTrans.position - owner._selfTrans.position).normalized;
                _axis.y = 0.0f;
                owner._targetRot = Quaternion.LookRotation(_axis);  
                owner._currentVelocity = new(_axis.x, owner._mover.Velocity.y, _axis.z);
                if (owner._distance < owner._attackRange) owner.ChangeState(StateType.Attack);
            }
        }
        protected override void OnExit(State nextState)
        {

        }
    }
}