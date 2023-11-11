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

        [Header("ダメージ数")]
        public int Damage;
        [Header("攻撃タイプ")]
        public AttackType AttackType;
        public int Id = 0;
        [Header("持続時間")]
        public float KeepTime;
        [Header("持続時間後の入力受付時間")]
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