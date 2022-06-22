using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using AttackSetting;

public class CharacterBase : MonoBehaviour,IDamage
{
    StatusModel _status = new StatusModel();

    [SerializeField]
    float _moveSpeed = default;
    public IReadOnlyReactiveProperty<int> CurrentHp { get => _status.hp; protected set { _status.hp.Value = value.Value; } }
    public int MaxHp { get => _status.maxHp; private set { _status.maxHp = value; }}
    public float MoveSpeed { get => _moveSpeed;protected set { _moveSpeed = value; } }
    public int Atk { get => _status.atk; protected set { _status.atk = value;} }

    public bool IsDeath { get; protected set; } = false;

    Rigidbody _rb;
    public Rigidbody Rigidbody { get => _rb; protected set { _rb = value; } }
    
    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        _rb = GetComponent<Rigidbody>();
        _status.maxHp = _status.hp.Value;
    }

    public virtual void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        _status.hp.Value = Mathf.Max(_status.hp.Value - damage,0);
        if(_status.hp.Value == 0)
        {
            IsDeath = true;
        }
    }

    public virtual void KnockBack()
    {

    }
}
