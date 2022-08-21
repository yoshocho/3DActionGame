using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UiManager
{

    private static UiManager s_instance = new UiManager();
    private UiManager()
    {

    }
    public static UiManager Instance => s_instance ??= new UiManager();
    

    List<UiPanel> _uiList;

    UiPanel _currentUi;
    int _prevUiIndex;

    const string _canvasName = "MasterCanvas";
    public Canvas MasterCanvas { get; private set; }

    public void SetUp()
    {
        MasterCanvas = GameObject.Find(_canvasName).GetComponent<Canvas>();
        UiPanel[] panels = MasterCanvas.GetComponentsInChildren<UiPanel>();

        _uiList = new List<UiPanel>();

        foreach (UiPanel panel in panels)
        {
            panel.SetUp();
            _uiList.Add(panel);
        }
    }

    public void ReceiveData<T>(T arg)
    {
        foreach (var parent in _uiList)
        {
            var ui = parent.ChildUis.OfType<IUIEventReceiver<T>>();
            if (ui != null)
            {
                ui.FirstOrDefault().ReceiveData(arg);
                break;
            }
        }
    }

    public void ReceiveData<T>(string parentName, T arg)
    {
        var ui = _uiList.FirstOrDefault(p => p.Path == parentName)
            .ChildUis.OfType<IUIEventReceiver<T>>().FirstOrDefault();

        if(ui == null)
        {
            Debug.LogWarning(string.Format("Žw’è‚³‚ê‚½UI‚ÍŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½{0},{1}",parentName,arg));
            return;
        }
        ui.ReceiveData(arg);
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

