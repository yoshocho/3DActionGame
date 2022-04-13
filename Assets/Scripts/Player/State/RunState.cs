using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class RunState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner.m_moveSpeed = owner.m_dashSpeed;
            owner.PlayAnimation("Run",0.3f);
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            owner.m_moveSpeed = owner.m_walkSpeed;
            //owner.PlayAnimation("RunToIdel");
        }

        public override void OnUpdate(Player owner)
        {
            if (owner.IsGround())
            {
                if (owner.m_inputDir.sqrMagnitude > 0.1f)
                {
                    var dir = owner.m_moveForward;
                    dir.y = 0f;
                    owner.m_targetRot = Quaternion.LookRotation(dir);
                    owner.m_currentVelocity =
                      new Vector3(owner.m_selfTrans.forward.x, owner.m_currentVelocity.y, owner.m_selfTrans.forward.z);
                }
                else
                {
                    owner.ChangeState(owner.m_idleState);
                }
                if(owner.m_inputManager.AvoidKey is KeyStatus.DOWN)
                {
                    owner.ChangeState(owner.m_avoidState);
                }
                if (owner.m_inputManager.LunchKey is KeyStatus.STAY) owner.m_lunchAttack = true;
                else owner.m_lunchAttack = false;
                if (owner.m_inputManager.AttackKey is KeyStatus.DOWN)
                {
                    owner.ChangeState(owner.m_attackState);
                }
                if(owner.m_inputManager.JumpKey is KeyStatus.DOWN && owner.m_currentJumpStep >= 0)
                {
                    owner.ChangeState(owner.m_jumpState);
                }
            }
            else
            {
                owner.ChangeState(owner.m_fallState);
            }
        }
    }
}
