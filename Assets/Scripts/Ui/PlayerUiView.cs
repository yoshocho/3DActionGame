using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiView : MonoBehaviour
{
    [SerializeField] Slider m_hpBar = default;
    private int m_maxHp;
    public void ApllyHpBar(int hp)
    {
        m_hpBar.value = hp / (float)m_maxHp;
        
    }
    public void MaxHpChange(int maxHp)
    {
        m_maxHp = maxHp;
    }
}