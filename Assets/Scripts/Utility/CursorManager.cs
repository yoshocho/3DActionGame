using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager
{
    public static void CursorCtrl(bool visible, CursorLockMode lockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockMode;
    }
}
