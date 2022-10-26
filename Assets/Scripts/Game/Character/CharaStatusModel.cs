using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class CharaStatusModel
{
    [SerializeField]
    IntReactiveProperty Hp = new IntReactiveProperty(default);

    public IReadOnlyReactiveProperty<int> CurrentHp { get => Hp; private set { Hp.Value = value.Value; } }
    
    public int MaxHp { get; private set; }

    [SerializeField]
    int atk;
    public int Atk { get => atk;}


    public void SetUp() => MaxHp = Hp.Value;
    public void UpdateHp(int hp) => Hp.Value = Mathf.Min(Hp.Value = hp, MaxHp);
    
    public void UpdateMaxHp(int maxHp,bool applyCurrent = false)
    {
        MaxHp = maxHp;
        if(applyCurrent) Hp.Value = maxHp;
    }
    public int UpdateAtk { set { atk = value; } }
}
