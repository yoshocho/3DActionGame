using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
   
    public class AttackState : PlayerStateBase
    {
        //bool m_lunchEnd = false;
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            owner._currentVelocity.x = 0f;
            owner._currentVelocity.z = 0f;

            owner._weaponHolder.ChangeWeapon(owner.m_weaponType);
            owner._hitCtrl = owner._weaponHolder.HitCtrl;
            owner.ChangeAttacks(owner.m_weaponType);

            owner.m_poseKeep = true;
            if (owner.IsGround())
            {
                if (prevState is AvoidState)
                {
                    owner._stateKeep = true;
                    //owner.ChangeAttacks(owner.m_weaponType);
                    owner.NextAction(0, owner.m_currentSkillList[0].Layer, owner.m_currentSkillList);
                }
                else if (owner.m_lunchAttack)
                {
                    owner._inKeepAir = true;
                    owner.m_lunchEnd = true;
                    owner.NextAction(1, owner.m_currentSkillList[1].Layer, owner.m_currentSkillList);
                }
                else
                {
                    //owner.ChangeAttacks(owner.m_weaponType);
                    owner.NextAction(owner.m_comboStep, owner.m_currentAttackList[owner.m_comboStep].Layer, owner.m_currentAttackList);
                    owner.m_comboStep++;
                }
            }
            else
            {
                //owner.ChangeAttacks(owner.m_weaponType);
                owner.m_currentAttackList = owner.m_currentAirialAttackList;
                owner.NextAction(owner.m_comboStep, owner.m_currentAttackList[owner.m_comboStep].Layer, owner.m_currentAttackList);
                owner.m_comboStep++;
            }
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            owner.AttackEnd();
            owner._stateKeep = false;
            owner.m_waitTimer = 0.0f;
            owner.m_reserveAction = false;
            owner.m_actionKeepingTimer = 0.0f;
            owner.m_comboStep = 0;
            owner.m_lunchAttack = false;
            owner._targetEnemys.Clear();
            //owner.m_inKeepAir = false;
            if (nextState is not IdleState)
            {
                owner._weaponHolder.ResetHolder();
                owner.m_poseKeep = false;
            }
        }

        public override void OnUpdate(Player owner)
        {
            if (owner.m_actionKeepingTimer > 0.0f)
            {
                //Debug.Log("攻撃持続時中");
                owner.m_actionKeepingTimer -= Time.deltaTime;
                if (owner.m_actionKeepingTimer <= 0.0f)
                {
                    owner.m_actionKeepingTimer = 0.0f;
                    //if (!owner.IsGround() && !owner.m_reserveAction)
                    //{
                    //    owner.m_inKeepAir = false;
                    //}
                }
            }
            else if (owner.m_waitTimer > 0.0f)
            {
                //Debug.Log("攻撃受付時間中");
                owner.m_waitTimer -= Time.deltaTime;
                if (owner.m_waitTimer <= 0.0f)
                {
                    owner.m_waitTimer = 0.0f;
                }
            }

            if (owner._inputManager.LunchKey is KeyStatus.STAY) owner.m_lunchAttack = true;
            else owner.m_lunchAttack = false;

            if (owner._inputProvider.GetAttack() && owner.m_actionKeepingTimer <= 0.0f)
            {
                owner.m_reserveAction = true;
                owner._stateKeep = false;
            }
            if (owner.m_reserveAction && owner.m_actionKeepingTimer <= 0.0f)
            {
                //owner.ChangeAttacks(owner.m_weaponType);
                if (owner.m_lunchAttack && !owner.m_lunchEnd)
                {
                    owner._inKeepAir = true;
                    owner.m_lunchEnd = true;
                    owner.NextAction(1, owner.m_currentSkillList[1].Layer, owner.m_currentSkillList);
                }
                else
                {
                    owner.NextAction(owner.m_comboStep, owner.m_currentAttackList[owner.m_comboStep].Layer, owner.m_currentAttackList);
                }
                owner.m_comboStep++;
                if (owner.m_comboStep >= owner.m_currentAttackList.Count)
                {
                    owner.m_comboStep = 0;
                }
                owner.m_reserveAction = false;
            }
            if (!owner.m_reserveAction && owner.m_waitTimer <= 0.0f)
            {
                owner.m_comboStep = 0;
                owner._targetEnemys.Clear();
            }
            //if (!owner.IsGround() && owner.m_actionKeepingTimer <= 0.0f && !owner.m_reserveAction)
            //{
            //    owner.ChangeState(owner.m_fallState);
            //}

            if (!owner.IsGround() && !owner.m_reserveAction && !owner._inKeepAir)
            {
                owner.ChangeState(owner._fallState);
            }
            if (owner.IsGround())
            {
                if (!owner.m_reserveAction && owner.m_actionKeepingTimer <= 0.0f)
                {
                    if (owner._inputDir.sqrMagnitude > 0.1f && owner._inputProvider.GetAttack())
                    {
                        owner.ChangeState(owner._walkState);
                    }
                    else if (!owner._animCtrl.IsPlayingAnimatin())
                    {
                        owner.ChangeState(owner._idleState);
                    }
                }
            }

            if (owner._inputProvider.GetAvoid() && !owner._stateKeep)
            {
                owner.ChangeState(owner._avoidState);
            }
            if (owner._inputProvider.GetJump() && owner._currentJumpStep >= 0)
            {
                owner.ChangeState(owner._jumpState);
            }
        }
    }
}
