public class GameTimeView : ChildUi
{
    TextMeshWrapper _text;
    TimeData _time;
    public override void SetUp()
    {
        _text = GetComponent<TextMeshWrapper>();
        _text.SetText("00:00:00");
        _time = GameManager.Instance.GameTime;
    }

    private void Update()
    {
        _text.SetText(_time.Hour.ToString() + ":" +
            _time.Minite.ToString("00") + ":" +
            _time.ElapsedTime.ToString("f2"));
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
