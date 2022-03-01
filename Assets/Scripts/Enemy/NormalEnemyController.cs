using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

public class NormalEnemyController : EnemyBase
{      
    //[SerializeField] GameObject m_targetPos = default;
    [SerializeField] float m_slowAngularSpeed = 3f;
    [SerializeField] float m_distace = default;
    [SerializeField] float m_chaseRange = 6f;
    [SerializeField] float m_attackRange = 2f;
    [SerializeField] int m_damage = 100;
    [SerializeField] BoxCollider m_enemyCollider = default;
    [SerializeField] Collider m_attackTrigger = default;
    [SerializeField] float m_attackCoolTime = 0.7f;
    float m_attackCurrentTime = 0f;
    [SerializeField] float m_rotateSpeed = 10f;
    [SerializeField] float m_enemyDeathTime = 1.5f;
    [SerializeField] GameObject m_hitEf = default;
    ConditionType m_currentState = ConditionType.Chase;

    protected override void StartSet()
    {
        base.StartSet();
        m_agent.speed = m_moveSpeed;
        m_attackTrigger.enabled = false;
        m_attackCurrentTime = m_attackCoolTime;
    }
    
    void Update()
    {
        
        m_distace = Vector3.Distance(transform.position, m_target);
        switch (m_currentState)
        {
            case ConditionType.Idel:
                break;
            case ConditionType.Wait:
                Wait();
                break;
            case ConditionType.Chase:
                Chase();
                break;
            case ConditionType.Attack:
                Attack();
                break;
            case ConditionType.Death:
                Death();
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if (m_anim)
        {
            m_anim.SetFloat("speed", m_agent.velocity.magnitude);
        }
    }
    private void OnTriggerEnter(Collider other)
    {  
        var target = other.gameObject.GetComponent<IDamage>();

        if (target != null && other.CompareTag("Player"))
        {
            target.AddDamage(m_damage);
        }
    }
    public void Wait()
    {
        m_agent.speed = 0f;
        if (!m_isSlow)
        {
            m_attackCurrentTime -= Time.deltaTime;
        }
        if (m_attackCurrentTime < 0f)
        {
            if (m_distace < m_attackRange)
            {
                m_attackCurrentTime = m_attackCoolTime;
                ChangeState(ConditionType.Attack);
            }
            else
            {
                m_attackCurrentTime = m_attackCoolTime;
                ChangeState(ConditionType.Chase);

            }
        }
    }
    public override void Chase()
    {
        base.Chase();
        if (m_distace < m_chaseRange)
        {
            if (m_distace < m_attackRange)
            {
                ChangeState(ConditionType.Attack);
            }
            else
            {
                ChangeState(ConditionType.Chase);
            }
        }
    }
    public override void Attack()
    {
        Vector3 targetPos = m_target;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        m_agent.speed = 0;     
        m_anim.SetTrigger("AttackTrigger");
        ChangeState(ConditionType.Wait);
    }
    public override void SlowDown(float slowTime)
    {
        base.SlowDown(slowTime);
    }
    public override void SetNormalTime()
    {
        base.SetNormalTime();
    }

    public override void AddDamage(int damage)
    {
        AttackEnd();
        Vector3 targetPos = m_target;
        targetPos.y = transform.position.y;
        Instantiate(m_hitEf, this.transform.position += new Vector3(0,0.5f,0),Quaternion.identity);
        transform.LookAt(targetPos);
        m_anim.Play("damage");
        base.AddDamage(damage);
        if (m_currentHp.Value <= 0f)
        {
            ChangeState(ConditionType.Death);
        }
    }
    public void Death()
    {      
        m_agent.speed = 0f;
        m_anim.Play("Death");
        m_enemyCollider.enabled = false;
        Destroy(this.gameObject, m_enemyDeathTime);
    }

    void ChangeState(ConditionType state)
    {
        m_currentState = state;
    }

    void StartAttack()
    {
        m_attackTrigger.enabled = true;
    }

    void AttackEnd()
    {
        m_attackTrigger.enabled = false;
    }
}
