using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public partial class ActionCtrl : MonoBehaviour
    {

        [SerializeField]
        AnimationCtrl m_animCtrl;
        [SerializeField]
        List<ActionData> m_lightSwordAttacks = new List<ActionData>();

        float m_receiveTime = 0.0f;
        float m_keepTimer = 0.0f;
        int m_comboCount = 0;

        ActionData m_currentAction;
        public bool ReserveAction { get; private set; } = false;
        public bool InKeepTime { get; private set; } = false;
        public bool InReceiveTime { get; private set; } = false;

        public bool CanRequest { get; private set; } = true;

        enum ClipName
        {
            First,
            Second,
        }

        ClipName m_clipName = ClipName.First;

        void Start()
        {
            if (!m_animCtrl) m_animCtrl = GetComponentInChildren<AnimationCtrl>();

        }
        void Update()
        {
            if (m_keepTimer > 0.0f)
            {
                m_keepTimer -= Time.deltaTime;

                CanRequest = false;
                if (m_keepTimer <= 0.0f)
                {
                    m_keepTimer = 0.0f;
                    CanRequest = true;
                }
            }
            else if (m_receiveTime > 0.0f)
            {
                m_receiveTime -= Time.deltaTime;

                if (m_receiveTime <= 0.0f)
                {
                    m_receiveTime = 0.0f;

                }
            }

            if (!ReserveAction && m_receiveTime <= 0.0f)
            {
                m_comboCount = 0;
            }

            //if (m_comboCount >= ÉRÉìÉ{êî)
            //{
            //    m_comboCount = 0;
            //}

        }

        public void RequestAction(AttackType attackType, int step = 0)
        {
            ReserveAction = true;
            if (!CanRequest) return;

            switch (attackType)
            {
                case AttackType.Light:
                    SetAction(m_lightSwordAttacks[m_comboCount]);
                    break;
                case AttackType.Heavy:
                    break;
                case AttackType.Airial:
                    break;
                case AttackType.Launch:
                    break;
                case AttackType.Counter:
                    break;
                default:
                    break;
            }

            m_comboCount++;
        }

        public void EndAttack()
        {
            TriggerOnEnable();
            m_keepTimer = 0.0f;
            m_receiveTime = 0.0f;
            m_comboCount = 0;
        }

        void SetAction(ActionData attack)
        {
            m_currentAction = attack;
            m_receiveTime = attack.ReceiveTime;
            m_keepTimer = attack.KeepTime;

            m_animCtrl.ChangeClip(m_clipName.ToString(), attack.AnimSet.Clip);
            m_animCtrl.Play(m_clipName.ToString(), attack.AnimSet.Duration);
            if (m_clipName is ClipName.Second)
                m_clipName = ClipName.First;
            else
                m_clipName = ClipName.Second;
        }

        public void HitCallBack(GameObject target)
        {
            if (target.tag != "Enemy") return;
            target.gameObject.GetComponent<IDamage>()?.AddDamage(m_currentAction.Damage);
        }

        private void TriggerOnEnable()
        {

        }
        private void TriggerOnDisable()
        {

        }
    }
}