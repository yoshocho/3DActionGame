using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{

    public class NewHitCtrl : MonoBehaviour
    {
        Collider m_collider;
        ActionCtrl m_actCtrl;

        private void Awake()
        {
            m_collider = GetComponentInChildren<Collider>();
            m_collider.enabled = false;
        }

        public void SetUp(ActionCtrl ctrl)
        {
            m_actCtrl = ctrl;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_actCtrl) return;

            m_actCtrl.HitCallBack(other);
        }
    }
}