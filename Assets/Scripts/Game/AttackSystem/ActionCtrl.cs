using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace AttackSetting
{
    /// <summary>
    /// �U���A�N�V�����̊Ǘ��N���X
    /// </summary>
    public partial class ActionCtrl : MonoBehaviour
    {
        [SerializeField]
        List<AttackList> _attackDatas = new List<AttackList>();
        public List<AttackList> AttackDatas { get => _attackDatas; set { _attackDatas = value; } }
        
        [SerializeField]
        protected HitCtrl _hitCtrl;
        [SerializeField]
        protected AnimationCtrl _animCtrl;
        [SerializeField]
        CharaAnimEventCtrl _animEventCtrl;
        RigidMover _mover;
        protected AttackType _prevType;
        
        public float ReceiveTimer { get; private set; } = 0.0f;
        public float KeepTimer { get; private set; } = 0.0f;
        int _actId = 0;

        GameObject _userObj;
        protected Transform _userTrans;

        public ActionData CurrentAction { get; private set; }
        public bool ReserveAction { get; private set; } = false;
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
        
        public virtual void SetUp(GameObject user)
        {
            _userTrans = user.transform;
            if (!_animCtrl) _animCtrl = GetComponentInChildren<AnimationCtrl>();
            if (!_hitCtrl) _hitCtrl = GetComponentInChildren<HitCtrl>();
            if (!_animEventCtrl) _animEventCtrl = GetComponentInChildren<CharaAnimEventCtrl>();
            if (!_mover) _mover = GetComponent<RigidMover>();
            _animEventCtrl.SetEffectEvent(SetEf);
            _animEventCtrl.SetTriggerEvent(TriggerActive);
            _animEventCtrl.SetAttackAction(AttackAction);

            _hitCtrl.SetUp(this, user);
        }
        void Update()
        {
            if (KeepTimer > 0.0f)
            {
                KeepTimer -= Time.deltaTime;

                ComboEnd = false;
                if (KeepTimer <= 0.0f)
                {
                    KeepTimer = 0.0f;
                    ReserveAction = false;
                }
            }
            else if (ReceiveTimer > 0.0f)
            {
                ReceiveTimer -= Time.deltaTime;

                if (ReceiveTimer <= 0.0f)
                {
                    ReceiveTimer = 0.0f;
                }
            }

            if (!ReserveAction && ReceiveTimer <= 0.0f)
            {
                _actId = 0;
            }

        }

        public bool IsActionKeep()
        {
            if(KeepTimer <= 0.0f) return false;

            return KeepTimer > 0.0f;
        }

        public bool IsReserveInput()
        {
            if(ReceiveTimer <= 0.0f) return false;
            return ReceiveTimer > 0.0f;
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

            var data = _attackDatas.FirstOrDefault(t => t.AttackType == attackType);

            if(data == null) 
            {
                Debug.LogError("�w�肳�ꂽ�^�C�v�͌�����܂���ł���");
                return;
            }
            if (id > -1) _actId = id;
            if (_actId > data.ActionDatas.Count)
            {
                _actId = 0;
                ComboEnd = true;
            }

            if (data[_actId] == null) return;
            SetAction(data[_actId]);
            _actId++;
        }

        public void EndAttack()
        {
            TriggerActive(AnimBool.False);
            KeepTimer = 0.0f;
            ReceiveTimer = 0.0f;
            ReserveAction = false;
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

            foreach (var item in CurrentAction.HitEvents)
            {
                item.SetUp(_userTrans);
                item.HitEvent(target);
            }
        }
        /// <summary>
        /// �U���̓����蔻����o���A�j���[�V�����C�x���g�p�֐�
        /// </summary>
        private void TriggerActive(AnimBool enable)
        {
            if (enable is AnimBool.True) _hitCtrl.TriggerEnable();
            else _hitCtrl.TriggerDisable();
        }

        void AttackAction()
        {
            CurrentAction.AttackAction.ForEach(action =>
            {
                action.SetUp(_mover);
                action.AttackAction();
            });
        }
    }
}