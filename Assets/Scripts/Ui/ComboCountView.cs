using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using TMPro;
[RequireComponent(typeof(Animator))]
public class ComboCountView : MonoBehaviour
{
    [SerializeField] TMP_Text m_comboText = null;
    void Start()
    {
        m_comboText.gameObject.SetActive(false);
        #region
        /*
        m_player.OnCombo
            .Scan(0, (cc, _) => cc + 1)
            .Do(_ => m_comboText.gameObject.SetActive(true))
            .Do(cc => m_comboText.SetText($"<size=400>{cc.ToString()}</size>Combo"))
            .Throttle(TimeSpan.FromSeconds(1.5f))
            .FirstOrDefault()
            .RepeatUntilDestroy(this)
            .Do(_ => m_comboText.gameObject.SetActive(false))
            .Subscribe();
        */
        #endregion
    }
    public void SetComboText(int comboCount)
    {
        m_comboText.SetText($"<size=90>{comboCount}</size>Combo");
    }

    public void TextEnabled()
    {
        m_comboText.gameObject.SetActive(true);
    }

    public void TextDisEnabled()
    {
        m_comboText.gameObject.SetActive(false);
    }
}
