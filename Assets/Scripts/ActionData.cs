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
        [SerializeField,Header("�A�j���[�V����")]
        public AnimationClip Clip;
        [Range(0, 0.5f),Header("�u�����h����")]
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
        [Header("��������")]
        public float KeepTime;
        [Header("�������Ԍ�̓��͎�t����")]
        public float ReceiveTime;
        public KnockBackPower KnockPower;
    }
}