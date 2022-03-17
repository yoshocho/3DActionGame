using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public partial class ActionCtrl : MonoBehaviour
    {
        [SerializeField]
        List<Attack> m_attacks = new List<Attack>();

        [SerializeField]
        AnimationCtrl m_animCtrl;


        float m_durationTime = 0.0f;
        float m_keepTime = 0.0f;

        public const string AttackClipName = "Attack";

        void Start()
        {
            if(!m_animCtrl)m_animCtrl = GetComponentInChildren<AnimationCtrl>();

        }
        void Update()
        {

        }

        public void RequestAction(AttackType attackType,int num)
        {

        }

        void SetAction(Attack attack)
        {


            m_animCtrl.ChangeClip(AttackClipName,attack.AnimSet.Clip);
            m_animCtrl.Play(AttackClipName, attack.AnimSet.Duration);
        }

        public void HitCallBack(GameObject target)
        {

        }

        public void ResetCombo()
        {
            TriggerOnDisable();

        }

        private void TriggerOnEnable()
        {
            
        }
        private void TriggerOnDisable()
        {
            
        }
    }
}