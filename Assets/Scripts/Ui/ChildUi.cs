using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChildUi : MonoBehaviour
{
    public abstract void SetUp();

    public virtual void Enable() { }

    public virtual void Disable() { }
}
