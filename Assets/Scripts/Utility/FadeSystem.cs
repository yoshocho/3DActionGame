using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.UI;

public class FadeSystem : SingleMonoBehaviour<FadeSystem>
{
    [SerializeField] Image m_fadePanel = default;

    [SerializeField] CanvasGroup m_canvasGroup = default;

    protected override void ForcedRun()
    {
        base.ForcedRun();

        var canvasObj = new GameObject("Canvas");
        canvasObj.transform.SetParent(transform);

        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        var imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform);
        var rectTrans = imageObj.AddComponent<RectTransform>();
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.offsetMin = Vector2.zero;
        rectTrans.offsetMax = Vector2.zero;

        m_fadePanel = imageObj.AddComponent<Image>();
        m_fadePanel.color = Color.black;
        m_canvasGroup = imageObj.AddComponent<CanvasGroup>();
        m_canvasGroup.blocksRaycasts = false;
        DontDestroyOnLoad(go);
    }

    public void FadeAsync(float fadeTime) => Fade(fadeTime,this.GetCancellationTokenOnDestroy()).Forget();
    public async UniTask Fade(float fadeTime,CancellationToken token = default)
    {
        float timer = 0;
        Color panelColor = m_fadePanel.color;
        float alpha = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            alpha += Time.deltaTime / fadeTime;
            panelColor.a = alpha;
            m_fadePanel.color = panelColor;
            await UniTask.WaitForEndOfFrame(token);
        }
    }
}
