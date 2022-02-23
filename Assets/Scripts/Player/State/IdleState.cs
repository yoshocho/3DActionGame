using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class IdleState : PlayerStateBase
    {
        public override void OnEnter(PlayerStateMachine owner, PlayerStateBase prevState)
        {
            if (prevState is WalkState) owner.PlayAnimation("WalkEnd", 0.2f);
            else if(prevState is RunState) owner.PlayAnimation("RunEnd", 0.2f);
            else owner.PlayAnimation("Idle", 0.1f);
            owner.m_currentVelocity.x = 0f;
            owner.m_currentVelocity.z = 0f;
            Debug.Log("InIdle");
        }

        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {

        }

        public override void OnUpdate(PlayerStateMachine owner)
        {
            if (owner.IsGround())
            {
                if (owner.m_inputDir.sqrMagnitude > 0.1f)
                {
                    owner.ChangeState(owner.m_walkState);
                }
                if (owner.m_inputManager.AvoidKey == KeyStatus.DOWN)
                {
                    owner.ChangeState(owner.m_avoidState);
                }
                if (owner.m_inputManager.AttackKey == KeyStatus.DOWN)
                {
                    owner.ChangeState(owner.m_attackState);
                }
                if (owner.m_inputManager.JumpKey == KeyStatus.DOWN && owner.m_currentJumpStep > 0)
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
