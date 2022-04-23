using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting {
    public partial class ActionCtrl : MonoBehaviour 
    {
        /// <summary>
        /// 攻撃のエフェクト再生関数
        /// </summary>
        /// <param name="data">攻撃データ</param>
        public void PlayEf(ActionData data)
        {

            if (data.Effect.CameraShake) CameraManager.ShakeCam();
            if (data.Effect.ControllerShake) { }

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
        /// アニメーションイベント用のエフェクト再生関数
        /// </summary>
        /// <param name="effect">タイプ</param>
        public void SetEf(AttackEffect effect)
        {
            switch (effect)
            {
                case AttackEffect.CameraShake:
                    CameraManager.ShakeCam();
                    break;
                case AttackEffect.ControllerShake:
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