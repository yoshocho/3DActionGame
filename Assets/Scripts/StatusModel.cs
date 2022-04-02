using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class StatusModel
{
    [SerializeField]
    public IntReactiveProperty hp = new IntReactiveProperty(default);
    public int maxHp = default;
}
