using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum AttackType
    {
        Weak,
        Heavy,
        Airial,
        Launch,
        Counter,
    }

    public enum WeaponType
    {
        Hand,
        LightSword,
        HeavySword,
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
        public float Duration = 0.1f;
        [Range(-0.5f, 2.0f)]
        public float Speed = 1.0f;
    }
    [System.Serializable]
    public class ControllerEf
    {
        public Vector2 ShakeVec;
        public float Dur;
    }

    [System.Serializable]
    public class AtkEffect
    {
        public GameObject HitEff;
        public bool CameraShake;
        public ControllerEf ControllerEf;
        public CamZoom ZoomSet;
    }

    [CreateAssetMenu(fileName = "ActionData", menuName = "ScriptableObjects/ActionData")]
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

    [System.Serializable]
    public class ComboData
    {
        public WeaponType WeaponType;
        public int ComboCount;
        public List<ActionData> ActionDatas = new List<ActionData>();
    }

}