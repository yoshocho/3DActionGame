using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public class AvoidState : PlayerStateBase
    {
        Vector3 m_avoidDir = default;

        float m_airDasuTime = default;

        float m_avoidEndTime = default;
        public override void OnEnter(Player owner, PlayerStateBase prevState)
        {
            m_airDasuTime = owner._airDasuTime;
            m_avoidEndTime = owner._avoidEndTime;
            if (owner.IsGround())
            {
                m_avoidDir = owner._moveForward.normalized;
                owner.PlayAnimation("Avoid", 0.2f);
            }
            else
            {
                owner.PlayAnimation("AirDasu", 0.0f);
                m_avoidDir = new Vector3(owner._moveForward.x, 0.0f, owner._moveForward.z).normalized;
                owner._currentAirDushCount++;
            }
            owner._justTrigger = true;
            owner._currentRotateSpeed = 1000.0f;
            Debug.Log("InAvoid");
        }

        public override void OnExit(Player owner, PlayerStateBase nextState)
        {
            owner._justTime = owner._currentJustTime;
            owner._currentGravityScale = owner._gravityScale;
            owner._justTrigger = false;
            owner._currentRotateSpeed = owner._rotateSpeed;
            m_avoidDir = Vector3.zero;
            owner._currentVelocity = Vector3.zero;
        }

        public override void OnUpdate(Player owner)
        {
            m_avoidEndTime -= Time.deltaTime;
            m_airDasuTime -= Time.deltaTime;
            owner._justTime -= Time.deltaTime;
            if (owner._justTime < 0.0f)
            {
                owner._justTrigger = false;
                owner._justTime = owner._currentJustTime;
            }
            if (owner.IsGround())
            {
                if (m_avoidEndTime < 0.0f)
                {
                    StateCheck(owner);
                }
                else
                {
                    owner._inputDir.y = 0.0f;

                    if (owner._inputDir == Vector3.zero) m_avoidDir = owner._selfTrans.forward * owner._avoidSpeed;
                    m_avoidDir.y = 0.0f;
                    owner._targetRot = Quaternion.LookRotation(m_avoidDir);

                    owner._currentVelocity = new Vector3(m_avoidDir.x, 0.0f, m_avoidDir.z) * owner._avoidSpeed;

                    if (owner._inputManager.LunchKey is KeyStatus.STAY)
                        owner.m_lunchAttack = true;
                    else owner.m_lunchAttack = false;

                    if (owner._inputProvider.GetAttack())
                    {
                        owner.ChangeState(owner._attackState);
                    }
                }
            }
            else
            {
                if (m_airDasuTime < 0.0f)
                {
                    owner.ChangeState(owner._fallState);
                }
                else
                {
                    owner._inputDir.y = 0.0f;
                    if (owner._inputDir == Vector3.zero) m_avoidDir = owner._selfTrans.forward * owner._airDasuSpeed;
                    m_avoidDir.y = 0.0f;
                    owner._targetRot = Quaternion.LookRotation(m_avoidDir);

                    owner._currentGravityScale = 0.0f;
                    owner._currentVelocity = m_avoidDir * owner._airDasuSpeed;
                }
            }
        }
        void StateCheck(Player owner)
        {
            if (owner._inputDir.sqrMagnitude > 0.1f)
            {
                if (owner._inputProvider.GetAvoid())
                {
                    owner.ChangeState(owner._runState);
                }
                else
                {
                    owner.ChangeState(owner._walkState);
                }
            }
            else if (!owner._animCtrl.IsPlayingAnimatin())
            {
                owner.ChangeState(owner._idleState);
            }
        }
    }
}
