using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using AttackSetting;

public class CharacterBase : MonoBehaviour,IDamage
{
    [SerializeField]
    public CharaStatusModel Status { get; protected set; } = new CharaStatusModel();

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
        Status.SetUp();
    }

    public virtual void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        var hp = Mathf.Max(Status.CurrentHp.Value - damage, 0);
        Status.UpdateHp(hp);
        if(Status.CurrentHp.Value == 0)
        {
            IsDeath = true;
        }
    }
}
