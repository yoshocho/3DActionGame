using UnityEngine;

public interface ITargetable
{
    bool Targetable { get; }
    Transform TargetTransform { get; }
}
