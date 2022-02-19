using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class RunState : PlayerStateBase
    {
        public override void OnEnter(PlayerStateMachine owner, PlayerStateBase prevState)
        {
            owner.m_moveSpeed = owner.m_dashSpeed;
            owner.PlayAnimation("Run",0.3f,owner.m_currentAnimLayer);
        }

        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {
            owner.m_moveSpeed = owner.m_walkSpeed;
            //owner.PlayAnimation("RunToIdel");
        }

        public override void OnUpdate(PlayerStateMachine owner)
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
            }
            else
            {
                owner.ChangeState(owner.m_fallState);
            }
        }
    }
}
