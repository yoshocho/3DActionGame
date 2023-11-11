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
    [SerializeReference, SubclassSelector]
    IScreenSpaseSet _screenSpase;
    [SerializeField]
    Image _cursorImage;
    RectTransform _tarns;
    Transform _target;
    public override void SetUp()
    {
        _cursorImage = GetComponent<Image>();
        _cursorImage.enabled = false;

        _tarns = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (_target == null)
        {
            Disable();
            return;
        }

        _screenSpase.SetScreenPosition(ref _tarns,_target);
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
        _cursorImage.enabled = true;
    }
    public override void Disable()
    {
        _cursorImage.enabled = false;
    }
}
