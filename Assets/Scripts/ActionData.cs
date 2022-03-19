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

    [System.Serializable]
    public class KnockBackPower
    {
        public float ForwardPower;
        public float UpPower;
    }

    [System.Serializable]
    public class AnimationSetting
    {
        [SerializeField]
        public AnimationClip Clip;
        [Range(0, 0.5f)]
        public float Duration;
        [Range(-0.5f, 2.0f)]
        public float Speed;
    }

    [CreateAssetMenu(menuName = "ActionData")]
    public class ActionData : ScriptableObject
    {
        public AnimationSetting AnimSet;
        public AttackType AttackType;
        public int Id;
        public float HitStopPower;
        public int Damage;
        public float KeepTime;
        public float ReceiveTime;
        public KnockBackPower KnockPower;
    }
}