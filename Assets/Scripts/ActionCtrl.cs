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
        AnimationCtrl _animCtrl;
        [SerializeField]
        List<ComboData> _comboDatas = new List<ComboData>();
        public List<ComboData> CurrentAttacks => _comboDatas;

        [SerializeField]
        OwerType _owerType;

        AttackType _prevType;
        ComboData _currentCombo;
        NewHitCtrl _hitCtrl;

        float _receiveTimer = 0.0f;
        float _keepTimer = 0.0f;
        int _comboCount = 0;

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

        ClipName _clipName = ClipName.First;

        void Start()
        {
            if (!_animCtrl) _animCtrl = GetComponentInChildren<AnimationCtrl>();

        }
        void Update()
        {
            if (_keepTimer > 0.0f)
            {
                _keepTimer -= Time.deltaTime;

                ActionKeep = true;
                ComboEnd = false;
                if (_keepTimer <= 0.0f)
                {
                    _keepTimer = 0.0f;
                    ActionKeep = false;
                    ReserveAction = false;
                }
            }
            else if (_receiveTimer > 0.0f)
            {
                InReceiveTime = true;
                _receiveTimer -= Time.deltaTime;

                if (_receiveTimer <= 0.0f)
                {
                    _receiveTimer = 0.0f;
                    InReceiveTime = false;
                }
            }

            if (!ReserveAction && _receiveTimer <= 0.0f)
            {
                _comboCount = 0;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attackType"></param>
        /// <param name="id"></param>
        public void RequestAction(AttackType attackType, int id = 0)
        {
            ReserveAction = true;
            if (!_comboDatas.Any()) return;
            ActionData data = null;
            if (_prevType != attackType) _comboCount = 0;
            _prevType = attackType;
            if (_comboCount >= _currentCombo.ComboCount)
            {
                _comboCount = 0;
                ComboEnd = true;
            }

            switch (attackType)
            {
                case AttackType.Weak:
                    _currentCombo = _comboDatas[0];
                    data = _comboDatas[0].ActionDatas[_comboCount];
                    break;

                case AttackType.Airial:
                    if (_comboDatas.Count < 1) break;
                    _currentCombo = _comboDatas[1];
                    data = _comboDatas[1].ActionDatas[_comboCount];
                    break;
                case AttackType.Counter:
                    data = _comboDatas[0].ActionDatas[-1];

                    break;
                case AttackType.Heavy:

                    break;
                case AttackType.Launch:

                    break;
                default:
                    break;
            }
            if (data) SetAction(data);
            _comboCount++;
        }

        public void EndAttack()
        {
            TriggerOnEnable();
            _keepTimer = 0.0f;
            _receiveTimer = 0.0f;
            _comboCount = 0;
        }
        /// <summary>
        /// アクションをセットする関数
        /// </summary>
        /// <param name="attack"></param>
        void SetAction(ActionData attack)
        {
            CurrentAction = attack;
            _receiveTimer = attack.ReceiveTime;
            _keepTimer = attack.KeepTime;

            _animCtrl.ChangeClip(_clipName.ToString(), attack.AnimSet.Clip);
            _animCtrl.Play(_clipName.ToString(), attack.AnimSet.Duration);

            if (_clipName is ClipName.Second)　//ブレンドするために二つのステートを交互に再生する
                _clipName = ClipName.First;
            else
                _clipName = ClipName.Second;
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