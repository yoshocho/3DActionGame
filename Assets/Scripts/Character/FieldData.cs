using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
   public List<EnemyBase> _enemys { get; private set; }

    public FieldData()
    {
        _enemys = new List<EnemyBase>();
    }
    public void RegisterEnemy(EnemyBase e) => _enemys.Add(e);
    public void RemoveEnemy(EnemyBase e) => _enemys.Remove(e);
}
