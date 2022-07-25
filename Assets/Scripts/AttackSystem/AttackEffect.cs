using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AttackSetting {

    /// <summary>
    /// 攻撃のエフェクトのタイプ
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
        /// アニメーションイベント用のエフェクト再生関数
        /// </summary>
        /// <param name="effect">タイプ</param>
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
                Debug.LogWarning(string.Format("指定されたエフェクトを設定していません{0}", effect));
                return;
            }
            ef.SetUp(gameObject);
            ef.SetEffect();
        }
    }
}