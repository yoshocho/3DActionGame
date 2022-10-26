using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaseOverlay : IScreenSpaseSet
{
    public void SetScreenPosition(ref RectTransform targetTrans, Transform worldTrans)
    {
        targetTrans.position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldTrans.position);
    }
}
