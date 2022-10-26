using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;


public class CharacterBase : MonoBehaviour,IDamage
{
    [SerializeField]
    CharaStatusModel _status = new CharaStatusModel();
    [SerializeField]
    ObjectData _data = new ObjectData();
    public CharaStatusModel Status { get => _status; protected set { _status = value;} } 
    public ObjectData Data { get => _data; protected set { _data = value;} }
    public bool IsDeath { get; protected set; } = false;

    private void Start()
    {
        SetUp();
    }

    protected virtual void SetUp()
    {
        _status.SetUp();
        if(_data.Type == ObjectType.None) _data.CheckType(gameObject);
    }

    public virtual void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        var hp = Mathf.Max(_status.CurrentHp.Value - damage, 0);
        _status.UpdateHp(hp);
        if(_status.CurrentHp.Value == 0)
        {
            IsDeath = true;
        }
    }
}
