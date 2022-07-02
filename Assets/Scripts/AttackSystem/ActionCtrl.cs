using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
        AnimationCtrl _animCtrl;
        [SerializeField]
        List<AttackData> _attackDatas = new List<AttackData>();
        public List<AttackData> CurrentAttacks => _attackDatas;
        [SerializeField]
        NewHitCtrl _hitCtrl;

        AttackType _prevType;
        
        public float ReceiveTimer { get; private set; } = 0.0f;
        public float KeepTimer { get; private set; } = 0.0f;
        int _actId = 0;

        public ActionData CurrentAction { get; private set; }
        public bool ReserveAction { get; private set; } = false;
        public bool InReceiveTime { get; private set; } = false;

        public bool ActionKeep { get; private set; } = false;

        public bool ComboEnd { get; private set; } = false;

        /// <summary>
        /// �U���̃A�j���[�V�����N���b�v�̖��O
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
            if (!_hitCtrl) _hitCtrl = GetComponentInChildren<NewHitCtrl>();

            _hitCtrl.SetUp(this);
        }
        void Update()
        {
            if (KeepTimer > 0.0f)
            {
                KeepTimer -= Time.deltaTime;

                ActionKeep = true;
                ComboEnd = false;
                if (KeepTimer <= 0.0f)
                {
                    KeepTimer = 0.0f;
                    ActionKeep = false;
                    ReserveAction = false;
                }
            }
            else if (ReceiveTimer > 0.0f)
            {
                InReceiveTime = true;
                ReceiveTimer -= Time.deltaTime;

                if (ReceiveTimer <= 0.0f)
                {
                    ReceiveTimer = 0.0f;
                    InReceiveTime = false;
                }
            }

            if (!ReserveAction && ReceiveTimer <= 0.0f)
            {
                _actId = 0;
            }

        }
        /// <summary>
        /// �A�N�V������I�肷��
        /// </summary>
        /// <param name="attackType">�U���̃^�C�v</param>
        /// <param name="id">Id</param>
        public void RequestAction(AttackType attackType, int id = -1)
        {
            ReserveAction = true;
            if (_prevType != attackType) _actId = 0;
            _prevType = attackType;

            var datas = _attackDatas.FirstOrDefault(t => t.AttackType == attackType);

            if (id > -1) _actId = id;
            if (_actId > datas.ActionDatas.Count)
            {
                _actId = 0;
                ComboEnd = true;
            }
            SetAction(datas.ActionDatas[_actId]);
            _actId++;
            return;
        }

        public void EndAttack()
        {
            TriggerDisable();
            KeepTimer = 0.0f;
            ReceiveTimer = 0.0f;
            _actId = 0;
        }
        /// <summary>
        /// �Z�b�g���ꂽ�U���f�[�^�𔽉f����
        /// </summary>
        /// <param name="attack">�U���f�[�^</param>
        void SetAction(ActionData attack)
        {

            CurrentAction = attack;
            ReceiveTimer = attack.ReceiveTime;
            KeepTimer = attack.KeepTime;

            _animCtrl
                .ChangeClip(_clipName.ToString(), attack.AnimSet.Clip)
                .SetParameter("AttackSpeed", attack.AnimSet.Speed)
                .Play(_clipName.ToString(), attack.AnimSet.Duration);

            if (_clipName is ClipName.Second)�@//�u�����h���邽�߂ɓ�̃X�e�[�g�����݂ɍĐ�����
                _clipName = ClipName.First;
            else
                _clipName = ClipName.Second;
        }

        /// <summary>
        /// �U���q�b�g���̊֐�
        /// </summary>
        /// <param name="target">�q�b�g�����R���C�_�[</param>
        public void HitCallBack(Collider target)
        {
            target.gameObject.GetComponent<IDamage>()?.AddDamage(CurrentAction.Damage);
            EffectManager.HitStop(CurrentAction.HitStopPower);
            if (CurrentAction.Effect.HitEff)
                EffectManager.PlayEffect(CurrentAction.Effect.HitEff, target.ClosestPoint(transform.position));

        }
        /// <summary>
        /// �U���̓����蔻����o���A�j���[�V�����C�x���g�p�֐�
        /// </summary>
        private void TriggerEnable()
        {
            _hitCtrl.TriggerEnable();
        }
        /// <summary>
        /// �U���̓����蔻��������A�j���[�V�����C�x���g�p�֐�
        /// </summary>
        private void TriggerDisable()
        {
            _hitCtrl.TriggerDisable();
        }
    }
}