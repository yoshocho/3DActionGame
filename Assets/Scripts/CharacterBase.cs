using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using AttackSetting;

public class CharacterBase : MonoBehaviour,IDamage
{
    [SerializeField]
    CharaStatusModel _status = new CharaStatusModel();
    public CharaStatusModel Status { get => _status; protected set { _status = value;} } 
    [SerializeField]
    float _moveSpeed = default;
    public float MoveSpeed { get => _moveSpeed;protected set { _moveSpeed = value; } }
    public bool IsDeath { get; protected set; } = false;

    Rigidbody _rb;
    public Rigidbody RB { get => _rb; protected set { _rb = value; } }
    
    private void Awake()
    {
        SetUp();
    }

    protected virtual void SetUp()
    {
        _rb = GetComponent<Rigidbody>();
        _status.SetUp();
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
