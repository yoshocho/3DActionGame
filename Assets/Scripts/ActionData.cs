using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum AttackType
    {
        Light,
        Heavy,
        Airial,
        Launch,
        Counter,
    }

    public enum WeaponType
    {
        HeavySword,
        LightSword,
        Hand,
    }

    public enum CamZoom 
    {
        Default,
        In,
        Out,
    }

    [System.Serializable]
    public class KnockBackPower
    {
        public float ForwardPower;
        public float UpPower;
        public float DownPower;
    }

    [System.Serializable]
    public class AnimationSetting
    {
        [SerializeField,Header("アニメーション")]
        public AnimationClip Clip;
        [Range(0, 0.5f),Header("ブレンド時間")]
        public float Duration;
        [Range(-0.5f, 2.0f)]
        public float Speed;
    }

    [System.Serializable]
    public class AtkEffect 
    {
        public GameObject HitEff;
        public bool CameraShake;
        public bool ControllerShake;
        public CamZoom ZoomSet;
    }

    [CreateAssetMenu(menuName = "ActionData")]
    public class ActionData : ScriptableObject
    {
        public AnimationSetting AnimSet;
        public AtkEffect Effect;
        public AttackType AttackType;
        //public int Id;
        public float HitStopPower;
        public int Damage;
        [Header("持続時間")]
        public float KeepTime;
        [Header("持続時間後の入力受付時間")]
        public float ReceiveTime;
        public KnockBackPower KnockPower;
    }
}