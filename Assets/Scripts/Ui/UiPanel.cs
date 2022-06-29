using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPanel : MonoBehaviour
{
    [SerializeField]
    bool _initVisibility = true;

    [SerializeField]
    int _id;
    public int Id { get => _id; private set { _id = value; } }

    protected CanvasGroup _canvasGroup;
    protected CanvasRenderer _canvasRenderer;
    public RectTransform RectTrans{ get; protected set; }

    List<ChildUi> _childUis = new List<ChildUi>();

    public List<ChildUi> ChildUis => _childUis;

    private void Awake()
    {
        RectTrans = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasRenderer = GetComponent<CanvasRenderer>();
        if (!_initVisibility) _canvasGroup.alpha = 0.0f;
        SetUp();
    }

    protected virtual void SetUp()
    {
        ChildUi[] childUis = GetComponentsInChildren<ChildUi>();
        foreach (ChildUi childUi in childUis)
        {
            childUi.SetUp();
            _childUis.Add(childUi);
        }
    }

    public virtual void Open()
    {
        foreach(ChildUi childUi in _childUis)
        {
            childUi.Enable();
        }
        _canvasGroup.alpha = 1.0f;
    }
    public virtual void Close()
    {
        foreach (ChildUi childUi in _childUis)
        {
            childUi.Disable();
        }
        _canvasGroup.alpha = 0.0f;
    }
}
