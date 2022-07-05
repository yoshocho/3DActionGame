using UnityEngine;
using UnityEngine.UI;

public abstract class ChildButton : ChildUi
{
    [SerializeField]
    protected Button _button;
    public override void SetUp()
    {
        if (!_button) _button = GetComponent<Button>();
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
