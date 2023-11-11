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
        None,
        LightSword,
        HeavySword,
    }

    [CreateAssetMenu(fileName = "ActionData", menuName = "ScriptableObjects/ActionData")]
    public class ActionData : ScriptableObject
    {
        public AnimClip AnimSet;
        [SerializeReference, SubclassSelector]
        public List<IHitEvent> HitEvents;
        [SerializeReference, SubclassSelector]
        public List<IAttackEffect> AttackEffects;
        [SerializeReference, SubclassSelector]
        List<IAttackAction> _attackAction = new List<IAttackAction>();
        public List<IAttackAction> AttackAction => _attackAction;
        [SerializeReference,SubclassSelector]
        List<IHitEventTask> _hitTasks = new List<IHitEventTask>();
        public List<IHitEventTask> HitTasks => _hitTasks;

        [Header("�_���[�W��")]
        public int Damage;
        [Header("�U���^�C�v")]
        public AttackType AttackType;
        public int Id = 0;
        [Header("��������")]
        public float KeepTime;
        [Header("�������Ԍ�̓��͎�t����")]
        public float ReceiveTime;
    }

    [System.Serializable]
    public class AttackList
    {
        public WeaponType WeaponType;
        public AttackType AttackType;
        public List<ActionData> ActionDatas = new List<ActionData>();

        public ActionData this[int index]
        {
            get => ActionDatas[index];
        }
    }
}