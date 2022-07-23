using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ComboCountView : MonoBehaviour
{
    [SerializeField]
    TMP_Text _comboText = null;
    [SerializeField]
    int _textSize = 85;
    void Start()
    {
        _comboText.gameObject.SetActive(false);
    }
    public void SetComboText(int comboCount)
    {
        _comboText.SetText($"<size={_textSize}>{comboCount}</size>Combo");
    }

    public void TextEnabled()
    {
        _comboText.gameObject.SetActive(true);
    }

    public void TextDisEnabled()
    {
        _comboText.gameObject.SetActive(false);
    }
}
