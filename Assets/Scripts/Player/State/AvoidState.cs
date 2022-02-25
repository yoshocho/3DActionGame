using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateMachine : MonoBehaviour
{
    public class AvoidState : PlayerStateBase
    {
        Vector3 m_avoidDir = default;

        float m_airDasuTime = default;

        float m_avoidEndTime = default;
        public override void OnEnter(PlayerStateMachine owner, PlayerStateBase prevState)
        {
            m_airDasuTime = owner.m_airDasuTime;
            m_avoidEndTime = owner.m_avoidEndTime;
            if (owner.IsGround())
            {
                m_avoidDir = owner.m_moveForward.normalized;
                owner.PlayAnimation("Avoid", 0.2f);
            }
            else
            {
                owner.PlayAnimation("AirDasu", 0.0f);
                m_avoidDir = new Vector3(owner.m_moveForward.x, 0.0f, owner.m_moveForward.z).normalized;
                owner.m_currentAirDushCount++;
            }
            owner.m_justTrigger = true;
            owner.m_currentRotateSpeed = 1000.0f;
            Debug.Log("InAvoid");
        }

        public override void OnExit(PlayerStateMachine owner, PlayerStateBase nextState)
        {
            //owner.m_avoidEndTime = 0.6f;
            //owner.m_airDasuTime = 0.5f;
            owner.m_justTime = owner.m_currntJustTime;
            owner.m_currentGravityScale = owner.m_gravityScale;
            owner.m_justTrigger = false;
            owner.m_currentRotateSpeed = owner.m_rotateSpeed;
            //m_avoidDir = Vector3.zero;
            owner.m_currentVelocity = Vector3.zero;
        }

        public override void OnUpdate(PlayerStateMachine owner)
        {
            m_avoidEndTime -= Time.deltaTime;
            m_airDasuTime -= Time.deltaTime;
            owner.m_justTime -= Time.deltaTime;
            if (owner.m_justTime < 0.0f)
            {
                owner.m_justTrigger = false;
                owner.m_justTime = owner.m_currntJustTime;
            }
            if (owner.IsGround())
            {
                if (m_avoidEndTime < 0.0f)
                {
                    StateCheck(owner);
                }
                else
                {
                    owner.m_inputDir.y = 0.0f;

                    if (owner.m_inputDir == Vector3.zero) m_avoidDir = owner.m_selfTrans.forward * owner.m_avoidDistance;
                    m_avoidDir.y = 0.0f;
                    owner.m_targetRot = Quaternion.LookRotation(m_avoidDir);

                    if (!owner.IsGround())
                    {
                        owner.m_currentGravityScale = 0.0f;
                        owner.m_currentVelocity = m_avoidDir * owner.m_airDasuSpeed;
                        //Debug.Log(owner.m_currentVelocity);
                    }
                    else
                    {
                        owner.m_currentVelocity = new Vector3(m_avoidDir.x, 0.0f, m_avoidDir.z) * owner.m_avoidDistance;
                    }
                    if (owner.m_inputManager.AttackKey == KeyStatus.DOWN)
                    {
                        owner.ChangeState(owner.m_attackState);
                    }
                }
            }
            else
            {
                if (m_airDasuTime < 0.0f)
                {
                    owner.ChangeState(owner.m_fallState);
                }
                else
                {
                    owner.m_inputDir.y = 0.0f;
                    if (owner.m_inputDir == Vector3.zero) m_avoidDir = owner.m_selfTrans.forward * owner.m_airDasuSpeed;
                    m_avoidDir.y = 0.0f;
                    owner.m_targetRot = Quaternion.LookRotation(m_avoidDir);

                    owner.m_currentGravityScale = 0.0f;
                    owner.m_currentVelocity = m_avoidDir * owner.m_airDasuSpeed;
                }
            }
        }
        void StateCheck(PlayerStateMachine owner)
        {
            if (owner.m_inputDir.sqrMagnitude > 0.1f)
            {
                if (owner.m_inputManager.AvoidKey == KeyStatus.STAY)
                {
                    owner.ChangeState(owner.m_runState);
                }
                else
                {
                    owner.ChangeState(owner.m_walkState);
                }
            }
            else if(!owner.m_animCtrl.IsPlayingAnimatin())
            {
                owner.ChangeState(owner.m_idleState);
            }
        }
    }
}
