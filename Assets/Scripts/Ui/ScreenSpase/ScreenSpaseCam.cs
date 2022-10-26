using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaseCam : IScreenSpaseSet
{
    [SerializeField]
    Camera _worldCam;
    [SerializeField]
    Camera _canvasCam;
    [SerializeField]
    RectTransform _rectTrans;
    public void SetScreenPosition(ref RectTransform targetTrans, Transform worldTrans)
    {
        targetTrans.localPosition = CameraUtilsExtensions.WorldToScreenSpaceCamera(_worldCam,_canvasCam,_rectTrans,worldTrans.position);
    }
}
