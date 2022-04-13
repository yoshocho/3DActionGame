using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class IdleState : PlayerStateBase
    {
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            if (prevState is WalkState) owner.PlayAnimation("WalkEnd", 0.2f);
            else if (prevState is RunState || prevState is AvoidState) owner.PlayAnimation("RunEnd", 0.2f);
            else if (prevState is AttackState)
            {
                switch (owner.m_weaponType)
                {
                    case WeaponType.HEAVY:
                        owner.PlayAnimation("HeavySwordIdle", 0.1f);
                        break;
                    case WeaponType.LIGHT:
                        owner.PlayAnimation("LightSwordIdle", 0.1f);
                        break;
                    default:
                        break;
                }
            }
            else owner.PlayAnimation("Idle", 0.1f);
            owner.m_currentVelocity.x = 0f;
            owner.m_currentVelocity.z = 0f;
            Debug.Log("InIdle");
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            if (owner.m_poseKeep)
            {
                owner.m_weaponHolder.ResetHolder();
                owner.m_poseKeep = false;
            }
        }

        public override void OnUpdate(Player owner)
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
                if (owner.m_inputManager.LunchKey is KeyStatus.STAY) owner.m_lunchAttack = true;
                else owner.m_lunchAttack = false;
                if (owner.m_inputManager.AttackKey == KeyStatus.DOWN)
                {
                    owner.ChangeState(owner.m_attackState);
                }
                if (owner.m_inputManager.JumpKey == KeyStatus.DOWN && owner.m_currentJumpStep >= 0)
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
