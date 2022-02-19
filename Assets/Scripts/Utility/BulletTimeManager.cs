using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BulletTimeManager : MonoBehaviour
{
    [SerializeField] PlayerStateMachine m_player = default;
    /// <summary>バレットタイムのイベント</summary>
    private Subject<float> bulletTimeSubject = new Subject<float>();
    public IObservable<float> OnBulletTime => bulletTimeSubject;
    /// <summary>バレットタイムの終了イベント</summary>
    private Subject<Unit> bulletTimeEndSubject = new Subject<Unit>();
    public IObservable<Unit> BulletTimeEnd => bulletTimeEndSubject;
    /// <summary>ジャスト成功時のイベント</summary>
    private Subject<Unit> onJustSubject = new Subject<Unit>();
    public IObservable<Unit> JustSuccess => onJustSubject;

    public static BulletTimeManager Instance;

    [Tooltip("スローモーション時の敵のアニメーションのスピード")]
    [SerializeField] float m_slowAnimTime = 0.1f;
    [Tooltip("ジャスト回避した時のゲーム時間")]
    [SerializeField] float m_slowTimeScale = 0.1f;
    [Tooltip("スローモーションの継続時間")]
    [SerializeField] float m_slowTime = 1f;
    [Tooltip("バレットタイムの継続時間")]
    [SerializeField] float m_bulletElapsedTime = 3f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        m_player?.OnJust.Subscribe(_ => OnJust().Forget());
    }

    private void SetNormalTime()
    {
        Time.timeScale = 1f;
    }

    private async UniTask OnJust(CancellationToken token = default)
    {
        onJustSubject.OnNext(Unit.Default);
        Time.timeScale = m_slowTimeScale;
        await UniTask.Delay(TimeSpan.FromSeconds(m_slowTime),ignoreTimeScale:true);
        SetNormalTime();
        bulletTimeSubject.OnNext(m_slowAnimTime);
        await UniTask.Delay(TimeSpan.FromSeconds(m_bulletElapsedTime),ignoreTimeScale:true);
        bulletTimeEndSubject.OnNext(Unit.Default);
    }

}
