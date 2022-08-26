using UnityEngine;
using UnityEngine.UI;

public class TextWrapper : MonoBehaviour
{
    Text _text;

    public void SetUp()
    {
        _text = GetComponent<Text>();
    }
    public void SetText(string text)
    {
        _text.text = text;
    }

    private void Reset()
    {
        _text = GetComponent<Text>();
        _text.horizontalOverflow = HorizontalWrapMode.Overflow;
        _text.verticalOverflow = VerticalWrapMode.Overflow;
        _text.fontSize = 40;
    }
}
