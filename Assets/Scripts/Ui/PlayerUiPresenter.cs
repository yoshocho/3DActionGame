﻿using System;
using UniRx;
using UnityEngine;

public class PlayerUiPresenter : MonoBehaviour
{
    [SerializeField]
    Player _player = default;
    [SerializeField]
    HpView _view = default;
    [SerializeField]
    ComboCountView _comboView = default;
    [SerializeField]
    float _chainTime = 1.5f;
    void Start()
    {
        _player.MaxHp
            .Subscribe(maxhp => _view.SetUp(maxhp))
            .AddTo(this);

        _player.OnDamage
            .Subscribe(hp => _view.ApllyHpBar(hp))
            .AddTo(this);

        _player.OnCombo
            .Scan(0, (cc, _) => cc + 1)
            .Do(_ => _comboView.TextEnabled())
            .Do(cc => _comboView.SetComboText(cc))
            .Throttle(TimeSpan.FromSeconds(_chainTime))
            .FirstOrDefault()
            .RepeatUntilDestroy(this)
            .Do(_ => _comboView.TextDisEnabled())
            .Subscribe()
            .AddTo(this);
    }
}
