using TMPro;
using UnityEngine;

public class TextMeshWrapper : MonoBehaviour
{
    TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
    public void SetText(string text)
    {
        _text.SetText(text);
    }

    private void Reset()
    {
        _text = GetComponent<TMP_Text>();
        _text.fontSize = 40;
    }
}
