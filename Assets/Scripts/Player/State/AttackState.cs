using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class AttackState : PlayerStateBase
    {
        public override void OnEnter(PlayerStateMachine owner,PlayerStateBase prevState)
        {
            owner.m_currentVelocity.x = 0f;
            owner.m_currentVelocity.z = 0f;

            owner.m_weaponHolder.ChangeWeapon(owner.m_weaponType);
            owner.m_hitCtrl = owner.m_weaponHolder.HitCtrl;

            switch (owner.m_weaponType)
            {
                case WeaponType.HEAVY_SWORD: owner.m_currentAttackList = owner.m_actionCtrl.HeavySwordNormalCombos;
                                             owner.m_currentSkillList = owner.m_actionCtrl.HeavySwordSkillList;
                                             owner.m_currentAirialAttackList = owner.m_actionCtrl.HeavySwordAirialCombos;
                    break;
                case WeaponType.LIGHT_SWORD: Debug.Log("設定中です"); break;
                default: throw new System.ArgumentException("invalid enum value");
            }
            owner.m_poseKeep = true;
            if (owner.IsGround())
            {
                if (prevState is AvoidState)
                {
                    owner.m_stateKeep = true;
                    owner.NextAction(0, owner.m_currentSkillList[0].Layer, owner.m_currentSkillList);
                }
                else
                {
                    owner.NextAction(owner.m_comboStep, owner.m_currentAttackList[owner.m_comboStep].Layer, owner.m_currentAttackList);
                    owner.m_comboStep++;
                }
            }
            else
            {
                owner.m_currentAttackList = owner.m_currentAirialAttackList;
                owner.NextAction(owner.m_comboStep, owner.m_currentAttackList[owner.m_comboStep].Layer, owner.m_currentAttackList);
                owner.m_comboStep++;
            }
        }

        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {
            owner.AttackEnd();
            owner.m_stateKeep = false;
            owner.m_waitTimer = 0.0f;
            owner.m_isAnimationPlaying = false;
            owner.m_reserveAction = false;
            owner.m_actionKeepingTimer = 0.0f;
            owner.m_comboStep = 0;
            if(nextState is not IdleState) owner.m_weaponHolder.ResetHolder();
        }

        public override void OnUpdate(PlayerStateMachine owner)
        {
            if (owner.m_waitTimer > 0.0f && owner.m_actionKeepingTimer <= 0.0f)
            {
                //Debug.Log("攻撃受付時間中");
                owner.m_waitTimer -= Time.deltaTime;
                if (owner.m_waitTimer <= 0.0f)
                {
                    owner.m_waitTimer = 0.0f;
                }
            }
            if (owner.m_actionKeepingTimer > 0.0f)
            {
                //Debug.Log("攻撃持続時中");
                owner.m_actionKeepingTimer -= Time.deltaTime;
                if (owner.m_actionKeepingTimer <= 0.0f)
                {
                    owner.m_actionKeepingTimer = 0.0f;
                }
            }
            if (owner.m_inputManager.AttackKey == KeyStatus.DOWN && owner.m_actionKeepingTimer <= 0.0f)
            {
                owner.m_reserveAction = true;
                owner.m_stateKeep = false;
            }
            if (owner.m_reserveAction && owner.m_actionKeepingTimer <= 0.0f)
            {
                owner.NextAction(owner.m_comboStep, owner.m_currentAttackList[owner.m_comboStep].Layer, owner.m_currentAttackList);
                owner.m_comboStep++;
                if (owner.m_comboStep >= owner.m_currentAttackList.Count)
                {
                    owner.m_comboStep = 0;
                }
                owner.m_reserveAction = false;
            }
            if (!owner.m_reserveAction && owner.m_waitTimer <= 0.0f)
            {
                //Debug.Log("コンボ終了");
                owner.m_comboStep = 0;
            }
            if (!owner.m_isAnimationPlaying && owner.m_waitTimer <= 0.0f && !owner.m_reserveAction && owner.m_actionKeepingTimer <= 0.0f)
            {
                if (owner.IsGround())
                {
                    owner.m_comboStep = 0;
                    if (owner.m_inputDir.sqrMagnitude > 0.1f)
                    {
                        owner.ChangeState(owner.m_walkState);
                    }
                    else
                    {
                        owner.ChangeState(owner.m_idleState);
                    }
                }
                else
                {
                    owner.m_airAttackEnd = true;
                    owner.ChangeState(owner.m_fallState);
                }
            }
            if (owner.m_inputManager.AvoidKey == KeyStatus.DOWN && !owner.m_stateKeep)
            {
                owner.ChangeState(owner.m_avoidState);
            }
            if (owner.m_inputManager.JumpKey == KeyStatus.DOWN && owner.m_currentJumpStep >= 0)
            {
                owner.ChangeState(owner.m_jumpState);
            }
        }
    }
}
