using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SceneChanger : Singleton<SceneChanger>
{
    [SerializeField,Tooltip("シーン遷移時のロードゲージ")]
    Slider m_loadGage = default;
    [SerializeField, Tooltip("シーン遷移のキャンバス")]
    Canvas m_loadCanvas = default;


    public void LoadSceneOrFade(string sceneName)
    {
        LoadSceneAsync(sceneName,this.GetCancellationTokenOnDestroy()).Forget();
    }

    protected override void AddOption(GameObject go)
    {
        base.AddOption(go);
        DontDestroyOnLoad(go);
    }

    /// <summary>
    /// 待機可能なシーン遷移関数
    ///＊必ずキャンセル処理をして使う
    /// </summary>
    /// <param name="sceneName">シーンの名前</param>
    /// <param name="token">キャンセルトークン</param>
    /// <returns></returns>
    public async UniTask LoadSceneAsync(string sceneName, CancellationToken token = default)
    {
        if (sceneName.Length <= 0) { Debug.LogWarning("シーンを指定してください"); return; }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        //m_loadCanvas.gameObject.SetActive(true);

        await FadeSystem.Instance.Fade(1.0f);

        while (true)
        {
            await UniTask.Yield();
            //m_loadGage.value = asyncLoad.progress;

            if (asyncLoad.progress >= 0.9f)
            {
                //m_loadGage.value = 1f;

                asyncLoad.allowSceneActivation = true;
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                break;
            }
        }
    }
}
