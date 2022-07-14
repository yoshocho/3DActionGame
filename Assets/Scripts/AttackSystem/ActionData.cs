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
        [Header("コントローラー振動の縦横値")]
        public Vector2 ShakeVec;
        [Header("コントローラー振動の持続時間")]
        public float Duration;
    }

    [System.Serializable]
    public class AtkEffect
    {
        [Header("カメらを揺らす強さ")]
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
        [Header("ダメージ数")]
        public int Damage;
        public AttackType AttackType;
        public int Id = 0;
        [Header("持続時間")]
        public float KeepTime;
        [Header("持続時間後の入力受付時間")]
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