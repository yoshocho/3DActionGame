using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AttackSetting {

    /// <summary>
    /// �U���̃G�t�F�N�g�̃^�C�v
    /// </summary>
    public enum AttackEffect
    {
        CameraShake,
        ControllerShake,
        SetEffect,
    }

    public partial class ActionCtrl : MonoBehaviour 
    {
        /// <summary>
        /// �A�j���[�V�����C�x���g�p�̃G�t�F�N�g�Đ��֐�
        /// </summary>
        /// <param name="effect">�^�C�v</param>
        public void SetEf(AttackEffect effect)
        {
            IAttackEffect ef = null;
            switch (effect)
            {
                case AttackEffect.CameraShake:
                    ef = CurrentAction.AttackEffects.FirstOrDefault(e => e is CameraShake);
                    break;
                case AttackEffect.ControllerShake:
                    //ef = CurrentAction.AttackEffects.OfType<ControllerShake>().First();
                    ef = CurrentAction.AttackEffects.FirstOrDefault(e => e is ControllerShake);
                    break;
                case AttackEffect.SetEffect:
                    ef = CurrentAction.AttackEffects.FirstOrDefault(e => e is SetAttackEffect);
                    break;
                default:
                    break;
            }
            if (ef == null)
            {
                Debug.LogWarning(string.Format("�w�肳�ꂽ�G�t�F�N�g��ݒ肵�Ă��܂���{0}", effect));
                return;
            }
            ef.SetUp(gameObject);
            ef.SetEffect();
        }
    }
}