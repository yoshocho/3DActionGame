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
    public class ControllerEf
    {
        [Header("�R���g���[���[�U���̏c���l")]
        public Vector2 ShakeVec;
        [Header("�R���g���[���[�U���̎�������")]
        public float Duration;
    }

    [System.Serializable]
    public class AtkEffect
    {
        [Header("�J�����h�炷����")]
        public Vector3 CameraShakeVec;
        public ControllerEf ControllerEf;
        public CamZoom ZoomSet;
    }

    [CreateAssetMenu(fileName = "ActionData", menuName = "ScriptableObjects/ActionData")]
    public class ActionData : ScriptableObject
    {
        public AnimClip AnimSet;
        public AtkEffect Effect;
        [SerializeReference, SubclassSelector]
        public List<IHitEvent> HitEvents;
        [SerializeReference, SubclassSelector]
        public List<IAttackEffect> AttackEffects;
        [Header("�_���[�W��")]
        public int Damage;
        public AttackType AttackType;
        public int Id = 0;
        [Header("��������")]
        public float KeepTime;
        [Header("�������Ԍ�̓��͎�t����")]
        public float ReceiveTime;
    }

    [System.Serializable]
    public class AttackData
    {
        public WeaponType WeaponType;
        public AttackType AttackType;
        public List<ActionData> ActionDatas = new List<ActionData>();
    }
}