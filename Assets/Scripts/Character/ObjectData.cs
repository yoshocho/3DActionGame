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

    public void CheckType(GameObject owner)
    {
        if(Type == ObjectType.None)
        {
            var tag = owner.tag;
            if(tag == "Player") Type = ObjectType.Player;
            else if(tag == "Enemy")Type = ObjectType.Enemy;
        }
    } 
}
