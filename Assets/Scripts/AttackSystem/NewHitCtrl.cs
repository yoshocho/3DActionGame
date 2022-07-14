using UnityEngine;

namespace AttackSetting
{

    public class NewHitCtrl : MonoBehaviour
    {
        [SerializeField]
        Collider _collider;

        GameObject _user;
        ActionCtrl _actCtrl;

        private void Awake()
        {
            _collider = GetComponentInChildren<Collider>();
            _collider.enabled = false;
        }

        public void SetUp(ActionCtrl ctrl,GameObject owner)
        {
            _actCtrl = ctrl;
            _user = owner;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_actCtrl) return;

            if (_user.gameObject.CompareTag("Player") && other.CompareTag("Enemy"))
                _actCtrl.HitCallBack(other);
            else if(_user.gameObject.CompareTag("Enemy") && other.CompareTag("Player"))
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