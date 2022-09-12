using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultData 
{
    public ScoreData ScoreData { get; private set; }
    public TimeData TimeData { get; private set; }

    public ResultData(ScoreData score,TimeData time)
    {
        ScoreData = score;
        TimeData = time;
    }
}

public class ResultText : ChildUi,IUIEventReceiver<ResultData>
{
    [SerializeField]
    List<TextMeshWrapper> _resultTexts;

    public override void SetUp()
    {
        _resultTexts.ForEach(t => t.SetUp());
    }

    public override void Disable()
    {
        _resultTexts.ForEach(t => t.Disable());
    }
    public override void Enable()
    {
        _resultTexts.ForEach(t => t.Enable());
    }

    public void ReceiveData(ResultData data)
    {
        int start = 0;
        DOTween.To(() => start, (value) => start = value, data.ScoreData.GetScore(), 1.0f)
            .OnUpdate(() => { _resultTexts[0].SetText($"ÉXÉRÉA:{start}"); });

        TimeData time = data.TimeData;
        _resultTexts[1].SetText("éûä‘" + time.Hour.ToString() + ":" +
            time.Minite.ToString("00") + ":" +
            time.ElapsedTime.ToString("f2"));
    }
}
