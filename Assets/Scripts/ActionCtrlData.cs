using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum AttackType
    {
        Light,
        Heavy,
        Launch,
        Counter,
        Airial,
    }

    public enum WeaponType
    {
        HeavySword,
        LightSword,
        Hand,
    }

    public partial class ActionCtrl : MonoBehaviour
    {

        [System.Serializable]
        public class KnockBackPower
        {
            public float ForwardPower;
            public float UpPower;
        }

        [System.Serializable]
        public class AnimationSetting
        {
            public AnimationClip Clip;
            public float Duration;
            public float Speed;
        }

        [CreateAssetMenu(menuName ="AttackData")]
        public class Attack : ScriptableObject
        {
            public AnimationSetting AnimSet;
            public AttackType AttackType;
            public float HitStopPower;
            public int Damage;
            public float KeepTime;
            public float ReceiveTime;
            public KnockBackPower KnockPower;
        }
    }
}