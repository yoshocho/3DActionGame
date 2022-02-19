using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AttackAssistController : MonoBehaviour
{
    [SerializeField] float m_targetRange = 4f;
    [SerializeField] float m_detectInterval = 1f;

    GameObject m_lockOnTarget = default;
    GameObject m_target = default;
    float m_timar;

    public GameObject LockOnTarget => m_lockOnTarget;
    public GameObject Target => m_target;
    
    public GameObject ShootTarget { get; private set; }

    void Update()
    {
        m_timar += Time.deltaTime;

        if (m_timar > m_detectInterval)
        {
            m_timar = 0;

            GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemyArray)
            {
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
                if (distance < m_targetRange)
                {
                    if (m_target == null || distance < Vector3.Distance(this.transform.position, m_target.transform.position))
                    {
                        m_target = enemy;
                    }
                }
            }
        }

        if (m_target)
        {
            //攻撃範囲かどうか
            if (m_targetRange < Vector3.Distance(this.transform.position, m_target.transform.position))
            {
                m_target = null;
            }
            else
            {
                Debug.DrawLine(this.gameObject.transform.position, m_target.transform.position, Color.blue);
            }
        }
    }

    //public GameObject GetTargetEnemy()
    //{
    //    var enemys = GameObject.FindGameObjectsWithTag("Enemy")
    //        .Where(enemy => enemy.GetComponent<EnemyBase>().IsVisible is true);

       
    //}
}
