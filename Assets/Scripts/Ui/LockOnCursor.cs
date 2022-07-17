using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnEventHandler 
{
    public bool lockOn = false;
    public Transform target = null;
}

public class LockOnCursor : ChildUi,IUIEventReceiver<LockOnEventHandler>
{
    [SerializeField]
    Image _cursor;

    Transform _target;

    private void Update()
    {

    }

    public override void SetUp()
    {

    }
    public void ReceiveData(LockOnEventHandler data)
    {
        if (data.lockOn)
        {
            _target = data.target;
        }
        else
        {
            _target = null;
        }
    }
    public override void Enable()
    {

    }
    public override void Disable()
    {

    }
}
