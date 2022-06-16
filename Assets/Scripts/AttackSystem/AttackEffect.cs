using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting {
    public partial class ActionCtrl : MonoBehaviour 
    {
        /// <summary>
        /// �U���̃G�t�F�N�g�Đ��֐�
        /// </summary>
        /// <param name="data">�U���f�[�^</param>
        public void PlayEf(ActionData data)
        {

            if (data.Effect.CameraShakeVec != Vector3.zero) CameraManager.ShakeCam();
            if (data.Effect.ControllerEf.ShakeVec != Vector2.zero)
                EffectManager.Instance.ControllerShake(data.Effect.ControllerEf.ShakeVec, data.Effect.ControllerEf.Duration);

            switch (data.Effect.ZoomSet)
            {
                case CamZoom.Default:
                    break;
                case CamZoom.In:
                    CameraManager.ZoomIn();
                    break;
                case CamZoom.Out:
                    CameraManager.ZoomOut();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// �A�j���[�V�����C�x���g�p�̃G�t�F�N�g�Đ��֐�
        /// </summary>
        /// <param name="effect">�^�C�v</param>
        public void SetEf(AttackEffect effect)
        {
            switch (effect)
            {
                case AttackEffect.CameraShake:
                    CameraManager.ShakeCam(CurrentAction.Effect.CameraShakeVec);
                    break;
                case AttackEffect.ControllerShake:
                    var ctlEf = CurrentAction.Effect.ControllerEf;
                    EffectManager.Instance.ControllerShake(ctlEf.ShakeVec,ctlEf.Duration);
                    break;
                case AttackEffect.ZoomIn:
                    CameraManager.ZoomIn();
                    break;
                case AttackEffect.ZoomOut:
                    CameraManager.ZoomOut();
                    break;
                case AttackEffect.SetEffect:

                    break;
                default:
                    break;
            }
        }
    }
}