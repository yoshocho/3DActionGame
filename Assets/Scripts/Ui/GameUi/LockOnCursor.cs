using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnEventHandler
{
    public Transform Target = null;
}

public class LockOnCursor : ChildUi,IUIEventReceiver<Transform>
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
        if (!_target) return;
        _tarns.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.position);
    }
    public void ReceiveData(Transform data)
    {
        if (data)
        {
            _target = data;
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
