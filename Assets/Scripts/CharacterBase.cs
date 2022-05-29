using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using AttackSetting;

[RequireComponent(typeof(Rigidbody))]
public class CharacterBase : MonoBehaviour,IDamage
{
    StatusModel m_status = new StatusModel();

    [SerializeField]
    float m_moveSpeed = default;
    public IReadOnlyReactiveProperty<int> CurrentHp { get => m_status.hp; protected set { m_status.hp.Value = value.Value; } }
    public int MaxHp { get => m_status.maxHp; private set { m_status.maxHp = value; }}
    public float MoveSpeed { get => m_moveSpeed;protected set { m_moveSpeed = value; } }
    public int Atk { get => m_status.atk; protected set { m_status.atk = value;} }

    public bool IsDeath { get; protected set; } = false;

    Rigidbody m_rb;

    public Rigidbody Rigidbody { get => m_rb; protected set { m_rb = value; } }

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_status.maxHp = m_status.hp.Value;
    }

    public virtual void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        m_status.hp.Value = Mathf.Max(m_status.hp.Value - damage,0);
        if(m_status.hp.Value == 0)
        {
            IsDeath = true;
        }
    }

    public virtual void KnockBack()
    {

    }
}
