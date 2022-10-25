using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtilsExtensions
{
    public static Vector3 WorldToScreenSpaceCamera
         (Camera worldCamera,
        Camera canvasCamera,
        RectTransform canvasRectTransform,
        Vector3 worldPosition)
    {
        var screenPoint = RectTransformUtility.WorldToScreenPoint
       (
           cam: worldCamera,
           worldPoint: worldPosition
       );

        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect: canvasRectTransform,
            screenPoint: screenPoint,
            cam: canvasCamera,
            localPoint: out var localPoint
        );
        return localPoint;
    }
}
