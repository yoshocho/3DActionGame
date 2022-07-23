using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerHPEventHandler
{
    public int hp;
    public int maxHp;
}

public class PlayerHPUI : ChildUi, IUIEventReceiver<PlayerHPEventHandler>
{
    [SerializeField]
    Slider _hpBar = null;
    public void ApllyHpBar(int hp,int maxHp)
    {
        _hpBar.value = hp / (float)maxHp;
    }
    public override void SetUp()
    {
        _hpBar = GetComponent<Slider>();
        _hpBar.value = 1;
    }
    public void ReceiveData(PlayerHPEventHandler data)
    {
        Debug.Log(data);
        ApllyHpBar(data.hp,data.maxHp);
    }
}
