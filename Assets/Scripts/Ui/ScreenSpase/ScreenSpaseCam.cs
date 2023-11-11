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
    RectTransform _canvasRectTrans;
    public void SetScreenPosition(ref RectTransform targetTrans, Transform worldTrans)
    {
        targetTrans.localPosition = CameraUtilsExtensions.WorldToScreenSpaceCamera(
            _worldCam,_canvasCam,_canvasRectTrans,worldTrans.position);
    }
}
