using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(CharacterController))]
public class CharacterBase : MonoBehaviour
{
    StatusModel m_status = new StatusModel();

    [SerializeField]
    float m_moveSpeed = default;
    [SerializeField]
    float m_ratateSpeed = default;
    [SerializeField]
    int m_atk = default;

    public IReadOnlyReactiveProperty<int> Hp { get => m_status.hp; protected set { m_status.hp.Value = value.Value; } }
    public int MaxHp { get => m_status.maxHp; private set { m_status.maxHp = value; }}
    public float MoveSpeed { get => m_moveSpeed;protected set { m_moveSpeed = value; } }
    public float RotateSpeed { get => m_ratateSpeed; protected set { m_ratateSpeed = value; } }
    public int Atk { get => m_atk; protected set { m_atk = value;} }

    public bool IsDeath { get; protected set; } = false;

    CharacterController m_controller;

    public CharacterController Controller { get => m_controller; protected set { m_controller = value; } }

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        m_controller = GetComponent<CharacterController>();
        m_status.maxHp = m_status.hp.Value;
    }
}
