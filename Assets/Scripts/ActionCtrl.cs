using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace AttackSetting
{
    /// <summary>
    /// 攻撃のエフェクトのタイプ
    /// </summary>
    public enum AttackEffect
    {
        CameraShake,
        ControllerShake,
        ZoomIn,
        ZoomOut,
        SetEffect,
    }

    public enum OwerType
    {
        Player,
        Enemy,
    }


    public partial class ActionCtrl : MonoBehaviour
    {

        [SerializeField]
        AnimationCtrl m_animCtrl;
        [SerializeField]
        List<ComboData> m_comboDatas = new List<ComboData>();
        public List<ComboData> CurrentAttacks => m_comboDatas;
        public List<ComboData> SetAttacks { set => value = m_comboDatas; }

        [SerializeField]
        OwerType m_owerType;

        AttackType m_prevType;
        ComboData m_currentCombo;
        NewHitCtrl m_hitCtrl;

        float m_receiveTimer = 0.0f;
        float m_keepTimer = 0.0f;
        int m_comboCount = 0;

        public ActionData CurrentAction { get; private set; }
        public bool ReserveAction { get; private set; } = false;
        public bool InKeepTime { get; private set; } = false;
        public bool InReceiveTime { get; private set; } = false;

        public bool ActionKeep { get; private set; } = false;

        public bool ComboEnd { get; private set; } = false;
        /// <summary>
        /// 攻撃のアニメーションクリップの名前
        /// </summary>
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

                ActionKeep = true;
                ComboEnd = false;
                if (m_keepTimer <= 0.0f)
                {
                    m_keepTimer = 0.0f;
                    ActionKeep = false;
                    ReserveAction = false;
                }
            }
            else if (m_receiveTimer > 0.0f)
            {
                m_receiveTimer -= Time.deltaTime;

                if (m_receiveTimer <= 0.0f)
                {
                    m_receiveTimer = 0.0f;
                }
            }

            if (!ReserveAction && m_receiveTimer <= 0.0f)
            {
                m_comboCount = 0;
            }

        }

        public void RequestAction(AttackType attackType, int id = 0)
        {
            ReserveAction = true;
            if (!m_comboDatas.Any()) return;
            ActionData data = null;
            if (m_prevType != attackType) m_comboCount = 0;
            m_prevType = attackType;
            if (m_comboCount > m_currentCombo.ComboCount)
            {
                m_comboCount = 0;
                ComboEnd = true;
            }

            switch (attackType)
            {
                case AttackType.Weak:
                    m_currentCombo = m_comboDatas[0];
                    data = m_comboDatas[0].ActionDatas[m_comboCount];
                    break;

                case AttackType.Airial:
                    if (m_comboDatas.Count < 1) break;
                    m_currentCombo = m_comboDatas[1];
                    data = m_comboDatas[1].ActionDatas[m_comboCount];
                    break;
                case AttackType.Counter:
                    data = m_comboDatas[0].ActionDatas[-1];

                    break;
                case AttackType.Heavy:

                    break;
                case AttackType.Launch:

                    break;
                default:
                    break;
            }
            if (data) SetAction(data);
            m_comboCount++;
        }

        public void EndAttack()
        {
            TriggerOnEnable();
            m_keepTimer = 0.0f;
            m_receiveTimer = 0.0f;
            m_comboCount = 0;
        }

        void SetAction(ActionData attack)
        {
            CurrentAction = attack;
            m_receiveTimer = attack.ReceiveTime;
            m_keepTimer = attack.KeepTime;

            m_animCtrl.ChangeClip(m_clipName.ToString(), attack.AnimSet.Clip);
            m_animCtrl.Play(m_clipName.ToString(), attack.AnimSet.Duration);

            if (m_clipName is ClipName.Second)　//ブレンドするために二つのステートを交互に再生する
                m_clipName = ClipName.First;
            else
                m_clipName = ClipName.Second;
        }

        /// <summary>
        /// 攻撃ヒット時の関数
        /// </summary>
        /// <param name="target">ヒットしたコライダー</param>
        public void HitCallBack(Collider target)
        {
            target.gameObject.GetComponent<IDamage>()?.AddDamage(CurrentAction.Damage);
            EffectManager.HitStop(CurrentAction.HitStopPower);
            if (CurrentAction.Effect.HitEff)
                EffectManager.PlayEffect(CurrentAction.Effect.HitEff, target.ClosestPoint(transform.position));

        }

        private void TriggerOnEnable()
        {
            //m_hitCtrl
        }
        private void TriggerOnDisable()
        {

        }
    }
}