using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System.Collections.Generic;
using System;
using System.Collections;

public enum NormalEnemyState
{
    Idel,
    Wait,
    Chase,
    Attack,
    Death
}
[RequireComponent(typeof(Rigidbody), (typeof(Animator)))]
[RequireComponent(typeof (NavMeshAgent))]
public class EnemyBase : MonoBehaviour, IDamage
{
    enum StateType
    {
        Grounded,
        InAir,
    }
    [SerializeField]
    protected IntReactiveProperty m_currentHp = new IntReactiveProperty(1000);
    public IReadOnlyReactiveProperty<int> Hp => m_currentHp;

    [SerializeField]
    protected float m_moveSpeed = 3.5f;
    [SerializeField]
    protected Vector3 m_target = default;

    /// <summary>通常のアニメーションスピード</summary>
    protected float m_normalAnimSpeed;
    protected bool m_isSlow = false;


    public bool IsVisible { get; private set; } = default;

    #region cash
    protected CharacterController m_controller;
    protected Rigidbody m_rb = default;
    protected Animator m_anim = default;
    protected NavMeshAgent m_agent = default;
    #endregion


    void Start()
    {
        StartSet();
        BulletTimeSet();
    }

    //private void Update()
    //{
        
    //}

    void CheckState()
    {

    }

    protected virtual void StartSet()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_normalAnimSpeed = m_anim.speed;
        m_target = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {
        IsVisible = false;

    }

    private void OnWillRenderObject()
    {
        IsVisible = true;
    }

    void BulletTimeSet()
    {
        var instance = BulletTimeManager.Instance;
        instance
            .OnBulletTime
            .Subscribe(time => SlowDown(time))
            .AddTo(this);
        instance
            .BulletTimeEnd
            .Subscribe(_ => SetNormalTime())
            .AddTo(this);
    }
    public virtual void Chase()
    {
        if (!m_isSlow)
        {
            m_agent.speed = m_moveSpeed;
        }
        m_agent.destination = m_target;
    }

    public virtual void EnemyDeath()
    {
        Debug.Log("敵死亡");
        Destroy(this.gameObject);
    }

    public virtual void Attack()
    {
        Debug.Log("攻撃");
    }

    public virtual void AddDamage(int damage)
    {
        m_currentHp.Value -= damage;
    }

    public virtual void SlowDown(float slowTime)
    {
        m_anim.speed = slowTime;
        m_agent.speed = 0f;
        m_isSlow = true;
    }
    public virtual void SetNormalTime()
    {
        m_anim.speed = m_normalAnimSpeed;
        m_agent.speed = m_moveSpeed;
        m_isSlow = false;
    }
}

