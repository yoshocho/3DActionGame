using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TimeData
{
    public float ElapsedTime { get; private set; }
    public int Minite { get; private set; }
    public int Hour { get; private set; }

    public void UpdateTime()
    {
        ElapsedTime += Time.deltaTime;

        if(ElapsedTime > 60)
        {
            Minite++;
            ElapsedTime -= 60.0f;
        }
        if(Minite >= 60)
        {
            Hour++;
            Minite = 0;
        }
    }
    public void ResetTime()
    {
        ElapsedTime = 0.0f;
        Minite = 0;
        Hour = 0;
    }
}
