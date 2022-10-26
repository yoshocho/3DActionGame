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
                Move(owner._moveType);
                
                if (owner._distance < owner.ActionParam.AttackRange && !owner._attackCoolTime)
                {
                    owner.ChangeState(StateType.Attack);
                }
                else
                {

                }
            }
            else
            {

            }
        }
        void Move(MoveType type)
        {
            switch (type)
            {
                case MoveType.Walk:
                    _axis = (owner._targetTrans.position - owner._selfTrans.position).normalized;
                    break;
                case MoveType.Stafe:
                    _axis = Vector3.zero;
                    break;
                case MoveType.Run:
                    break;
                case MoveType.Stop:
                    _axis = Vector3.zero;
                    break;
                default:
                    break;

            }
            _axis.y = 0.0f;
            owner._targetRot = Quaternion.LookRotation(_axis);
            owner._mover.Velocity = new(_axis.x, owner._mover.Velocity.y, _axis.z);
        }

        protected override void OnExit(State nextState)
        {
            _axis = Vector3.zero;
        }
    }
}