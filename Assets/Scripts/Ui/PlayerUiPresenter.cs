using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerUiPresenter : MonoBehaviour
{
    [SerializeField]
    PlayerStateMachine m_player = default;
    [SerializeField]
    PlayerUiView m_view = default;
    [SerializeField]
    ComboCountView m_comboView= default;
    [SerializeField]
    ActionControl m_actionControl = default;

    [SerializeField] float m_chainTime = 1.5f;
    void Start()
    {
        m_player.MaxHp
            .Subscribe(maxhp => m_view.MaxHpChange(maxhp))
            .AddTo(this);

        m_player.OnDamage
            .Subscribe(hp => m_view.ApllyHpBar(hp))
            .AddTo(this);

        m_actionControl.OnCombo
            .Scan(0, (cc, _) => cc + 1)
            .Do(_ => m_comboView.TextEnabled())
            .Do(cc => m_comboView.SetComboText(cc))
            .Throttle(TimeSpan.FromSeconds(m_chainTime))
            .FirstOrDefault()
            .RepeatUntilDestroy(this)
            .Do(_ => m_comboView.TextDisEnabled())
            .Subscribe()
            .AddTo(this);
    }
}
