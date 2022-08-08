using UnityEngine.UI;
using UniRx;
using UnityEngine;
using System;

public class SceneChangeButton : ChildButton
{
    [SerializeField]
    string _sceneName = string.Empty;

    const float WaitTime = 1.0f;
    public override void SetUp()
    {
        base.SetUp();

        _button.OnClickAsObservable()
            .Where(_ => _sceneName.Length >= 0)
            .ThrottleFirst(TimeSpan.FromSeconds(WaitTime))
            .Subscribe(_ => SceneChanger.Instance.FadeScene(_sceneName))
            .AddTo(this);
    }
}
