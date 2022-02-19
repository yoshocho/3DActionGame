using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class WalkState : PlayerStateBase
    {
        public override void OnEnter(PlayerStateMachine owner, PlayerStateBase prevState)
        {
            owner.m_moveSpeed = owner.m_walkSpeed;
            owner.PlayAnimation("Walk" ,0.2f,owner.m_currentAnimLayer);
        }
        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {
        }
        public override void OnUpdate(PlayerStateMachine owner)
        {
            if (owner.IsGround())
            {
                if (owner.m_inputDir.sqrMagnitude > 0.1)
                {
                    var dir = owner.m_moveForward;
                    dir.y = 0f;
                    owner.m_targetRot = Quaternion.LookRotation(dir);
                    owner.m_currentVelocity = new Vector3(dir.x ,owner.m_currentVelocity.y ,dir.z);
                    //owner.m_currentVelocity =
                    //    new Vector3(owner.m_selfTrans.forward.x, owner.m_currentVelocity.y, owner.m_selfTrans.forward.z);
                    if(owner.m_inputManager.AvoidKey == KeyStatus.DOWN)
                    {
                        owner.ChangeState(owner.m_avoidState);
                    }
                    if (owner.m_inputManager.AttackKey == KeyStatus.DOWN)
                    {
                        owner.ChangeState(owner.m_attackState);
                    }
                    if (owner.m_inputManager.JumpKey == KeyStatus.DOWN && owner.m_jumpStep > 0)
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