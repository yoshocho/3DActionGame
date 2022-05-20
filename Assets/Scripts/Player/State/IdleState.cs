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
            owner._currentVelocity.x = 0f;
            owner._currentVelocity.z = 0f;
            Debug.Log("InIdle");
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            if (owner.m_poseKeep)
            {
                owner._weaponHolder.ResetHolder();
                owner.m_poseKeep = false;
            }
        }

        public override void OnUpdate(Player owner)
        {
            if (owner.IsGround())
            {
                if (owner._inputDir.sqrMagnitude > 0.1f)
                {
                    owner.ChangeState(owner._walkState);
                }
                if (owner._inputProvider.GetAvoid())
                {
                    owner.ChangeState(owner._avoidState);
                }
                if (owner._inputManager.LunchKey is KeyStatus.STAY) owner.m_lunchAttack = true;
                else owner.m_lunchAttack = false;
                if (owner._inputProvider.GetAttack())
                {
                    owner.ChangeState(owner._attackState);
                }
                if (owner._inputProvider.GetJump() && owner._currentJumpStep >= 0)
                {
                    owner.ChangeState(owner._jumpState);
                }
            }
            else
            {
                owner.ChangeState(owner._fallState);
            }
        }
    }
}
