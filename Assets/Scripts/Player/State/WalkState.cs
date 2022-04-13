using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class WalkState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.m_moveSpeed = owner.m_walkSpeed;
            owner.PlayAnimation("Walk" ,0.2f);
        }
        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
        }
        public override void OnUpdate(Player owner)
        {
            if (owner.m_inputManager.LunchKey is KeyStatus.STAY) owner.m_lunchAttack = true;
            else owner.m_lunchAttack = false;
            if (owner.IsGround())
            {
                if (owner.m_inputDir.sqrMagnitude > 0.1)
                {
                    owner.m_targetRot = Quaternion.LookRotation(owner.m_moveForward);
                    owner.m_currentVelocity = new Vector3(owner.m_moveForward.x, owner.m_currentVelocity.y , owner.m_moveForward.z);
                    //owner.m_currentVelocity =
                    //    new Vector3(owner.m_selfTrans.forward.x, owner.m_currentVelocity.y, owner.m_selfTrans.forward.z);
                    if(owner.m_inputManager.AvoidKey == KeyStatus.DOWN)
                    {
                        owner.ChangeState(owner.m_avoidState);
                    }
                    if (owner.m_inputManager.AttackKey is KeyStatus.DOWN)
                    {
                        owner.ChangeState(owner.m_attackState);
                    }
                    if (owner.m_inputManager.JumpKey == KeyStatus.DOWN && owner.m_jumpStep >= 0)
                    {
                        owner.ChangeState(owner.m_jumpState);
                    }
                }
                else
                {
                    owner.ChangeState(owner.m_idleState);
                }
            }
            else
            {
                owner.ChangeState(owner.m_fallState);
            }
        }
    }
}