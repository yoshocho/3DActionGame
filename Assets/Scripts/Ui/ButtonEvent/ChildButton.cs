using UnityEngine;
using UnityEngine.UI;

public abstract class ChildButton : ChildUi
{
    [SerializeField]
    protected Button _button;
    [SerializeField]
    bool _startedSelect = false;
    public override void SetUp()
    {
        if (!_button) _button = GetComponent<Button>();
        if (_startedSelect) _button.Select();
    }

    public override void Enable()
    {
        _button.enabled = true;
    }
    public override void Disable()
    {
        _button.enabled = false;
    }
}
