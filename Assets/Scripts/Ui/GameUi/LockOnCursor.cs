using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct LockOnEventHandler
{
    public bool IsLockOn;
    public Transform Target;
    public LockOnEventHandler(bool lockOn, Transform target = null)
    {
        IsLockOn = lockOn;
        Target = target;
    }
}

public class LockOnCursor : ChildUi, IUIEventReceiver<LockOnEventHandler>
{
    [SerializeField]
    Image _cursor;
    RectTransform _tarns;
    Transform _target;
    public override void SetUp()
    {
        _cursor = GetComponent<Image>();
        _cursor.enabled = false;

        _tarns = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (_target == null)
        {
            Disable();
            return;
        }


        _tarns.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.position);
    }
    public void ReceiveData(LockOnEventHandler data)
    {
        if (data.IsLockOn)
        {
            _target = data.Target;
            Enable();
        }
        else
        {
            _target = null;
            Disable();
        }
    }
    public override void Enable()
    {
        _cursor.enabled = true;
    }
    public override void Disable()
    {
        _cursor.enabled = false;
    }
}
