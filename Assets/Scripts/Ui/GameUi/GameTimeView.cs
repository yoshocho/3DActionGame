using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeView : ChildUi
{
    TextMeshWrapper _text;
    public override void SetUp()
    {
        _text = GetComponent<TextMeshWrapper>();
        _text.SetText("00:00:00");
    }

    private void Update()
    {
        if (GameManager.Instance.GameTime == null) return;

        var time = GameManager.Instance.GameTime;
        _text.SetText(time.Hour.ToString() + ":" + time.Minite.ToString("00")
            + ":" + time.ElapsedTime.ToString("f2"));

    }

    public override void Enable()
    {
        _text.Enable();
    }
    public override void Disable()
    {
        _text.Disable();
    }
}
