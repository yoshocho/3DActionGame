using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    List<UiPanel> _uiList;

    UiPanel _currentUi;
    int _prevUiIndex;

    public void SetUp()
    {


        _uiList = new List<UiPanel>();

    }

    public void ReceiveData<T>(T arg)
    {
        foreach (var parent in _uiList)
        {
            var ui = parent.ChildUis.OfType<IUIEventReceiver<T>>();
            ui.FirstOrDefault().ReceiveData(arg);
            break;
        }
    }

    public void ReceiveData<T>(string parentName, T arg)
    {
        var parent = _uiList
            .FirstOrDefault(p => p.name == parentName)
            .ChildUis.OfType<IUIEventReceiver<T>>().FirstOrDefault();
        parent.ReceiveData(arg);
    }

    public void RequestOpen(string path)
    {
        var ui = _uiList.FirstOrDefault(u => u.Path == path);
        ui?.Open();
    }

    public void RequestClose(string path)
    {
        var ui = _uiList.FirstOrDefault(u => u.Path == path);
        ui?.Close();
    }
}

