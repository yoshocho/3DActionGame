using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    None,
    Player,
    Enemy,
}
[System.Serializable]
public class ObjectData
{
    [SerializeField]
    public ObjectType Type;
    [SerializeField]
    public string Name = string.Empty;
}
