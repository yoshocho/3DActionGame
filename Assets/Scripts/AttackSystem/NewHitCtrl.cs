using UnityEngine;

namespace AttackSetting
{

    public class NewHitCtrl : MonoBehaviour
    {
        [SerializeField]
        Collider _collider;
        ActionCtrl _actCtrl;

        private void Awake()
        {
            _collider = GetComponentInChildren<Collider>();
            _collider.enabled = false;
        }

        public void SetUp(ActionCtrl ctrl)
        {
            _actCtrl = ctrl;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_actCtrl) return;

            _actCtrl.HitCallBack(other);
        }

        public void TriggerEnable()
        {
            _collider.enabled = true;
        }
        public void TriggerDisable()
        {
            _collider.enabled = false;
        }
    }
}