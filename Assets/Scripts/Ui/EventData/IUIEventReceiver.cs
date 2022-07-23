using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIEventReceiver<T>
{
    void ReceiveData(T data);
}
