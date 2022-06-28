using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    List<UiPanel> _uiList = new List<UiPanel>();

    UiPanel _currentUi;
    int _prevUiIndex;
    public void SetUp()
    {

    }

    public static void Open(int id)
    {
        if(id == Instance._prevUiIndex)Instance._uiList[id].Close();
        Instance._prevUiIndex = id;
        Instance._currentUi = Instance._uiList[id];
        Instance._currentUi.Open();
    }
}