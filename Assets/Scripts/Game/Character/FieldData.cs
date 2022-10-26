using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldData
{
   public List<EnemyBase> Enemys { get; private set; }

    public FieldData()
    {
        Enemys = new List<EnemyBase>();
    }
    public void RegisterEnemy(EnemyBase e) => Enemys.Add(e);
    public void RemoveEnemy(EnemyBase e) => Enemys.Remove(e);
}
