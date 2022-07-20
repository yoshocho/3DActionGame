using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnEventHandler 
{
    public bool LockOn = false;
    public Transform Target = null;
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
        if (data.LockOn)
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
        _cursor.gameObject.SetActive(true);
    }
    public override void Disable()
    {
        _cursor.gameObject.SetActive(false);
    }
}
