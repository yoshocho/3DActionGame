using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeSystem : SingleMonoBehaviour<FadeSystem>
{
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

        _fadePanel = imageObj.AddComponent<Image>();
        _fadePanel.color = Color.black;
        _canvasGroup = imageObj.AddComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnAwake()
    {
        if (!_fadePanel) _fadePanel = GetComponentInChildren<Image>();
        if (!_canvasGroup) _canvasGroup = GetComponentInChildren<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] Image _fadePanel = default;

    [SerializeField] CanvasGroup _canvasGroup = default;

    public const float DefaultFadeTime = 1.0f;

    float _startValue = 0.0f;
    float _endValue = 0.0f;
    float _elapedTime = 0.0f;
    float _fadeTime = 0.0f;

    Action _onFadeEnd;
    bool _isFade;
    private void Update()
    {
        if (!_isFade) return;
        _elapedTime += Time.deltaTime;
        var rate = _elapedTime / _fadeTime;
        _canvasGroup.alpha = Mathf.Lerp(_startValue, _endValue, rate);

        if (rate > 1.0f)
        {
            _onFadeEnd?.Invoke();
            _onFadeEnd = null;
            _isFade = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    private void StartFade(float startValue, float endValue, float fadeTime, Action onFadeEnd = null)
    {
        if (_isFade) return;
        _fadePanel.gameObject.SetActive(true);
        _fadeTime = fadeTime;
        _startValue = startValue;
        _endValue = endValue;
        _onFadeEnd = onFadeEnd;
        _elapedTime = 0.0f;
        _isFade = true;
    }

    public static void FadeIn(Action onFadeEnd = null)
    {
        Instance.StartFade(1.0f, 0.0f, DefaultFadeTime, onFadeEnd);
    }
    public static void FadeIn(float fadeTime = DefaultFadeTime, Action onFadeEnd = null)
    {
        Instance.StartFade(1.0f, 0.0f, fadeTime, onFadeEnd);
    }
    public static void FadeOut(Action onFadeEnd = null)
    {
        Instance.StartFade(0.0f, 1.0f, DefaultFadeTime, onFadeEnd);
    }
    public static void FadeOut(float fadeTime = DefaultFadeTime, Action onFadeEnd = null)
    {
        Instance.StartFade(0.0f, 1.0f, fadeTime, onFadeEnd);
    }
}
