using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting {
    public partial class ActionCtrl : MonoBehaviour 
    {
        public void SetEf(ActionData data)
        {

            if (data.Effect.CameraShake) CameraManager.ShakeCam();
            if (data.Effect.ControllerShake) { }

            switch (data.Effect.ZoomSet)
            {
                case CamZoom.Default:
                    break;
                case CamZoom.In:
                    CameraManager.ZoomIn();
                    break;
                case CamZoom.Out:
                    CameraManager.ZoomOut();
                    break;
                default:
                    break;
            }
        }
    }
}