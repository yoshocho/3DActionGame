using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    /// <summary>
    /// �U���̃G�t�F�N�g�̃^�C�v
    /// </summary>
    public enum AttackEffect 
    {
        CameraShake,
        ControllerShake,
        ZoomIn,
        ZoomOut,
        SetEffect,
    }

    public partial class ActionCtrl : MonoBehaviour
    {

        [SerializeField]
        AnimationCtrl m_animCtrl;
        [SerializeField]
        List<ActionData> m_lightSwordAttacks = new List<ActionData>();

        List<ComboData> m_comboDatas = new List<ComboData>();

        NewHitCtrl m_hitCtrl;

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
            //m_hitCtrl = GetComponentInChildren<HitCtrl>();
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
                    ReserveAction = false;
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

            if (m_comboCount >= m_lightSwordAttacks.Count)
            {
                m_comboCount = 0;
            }

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
                    //var x = m_comboDatas.Find(v => v.Type == AttackType.Airial);
                    //SetAction(x.ActionDatas[m_comboCount]);
                    break;
                case AttackType.Launch:
                    break;
                case AttackType.Counter:
                    break;
                default:
                    break;
            }

            // SetAction();

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

            SetEf(attack);
            m_animCtrl.ChangeClip(m_clipName.ToString(), attack.AnimSet.Clip);
            m_animCtrl.Play(m_clipName.ToString(), attack.AnimSet.Duration);

            if (m_clipName is ClipName.Second)
                m_clipName = ClipName.First;
            else
                m_clipName = ClipName.Second;
        }

        public void HitCallBack(Collider target)
        {
            target.gameObject.GetComponent<IDamage>()?.AddDamage(m_currentAction.Damage);
            if(m_currentAction.Effect.HitEff)
            EffectManager.PlayEffect(m_currentAction.Effect.HitEff,target.ClosestPoint(transform.position));


        }

        private void TriggerOnEnable()
        {

        }
        private void TriggerOnDisable()
        {

        }
    }
}