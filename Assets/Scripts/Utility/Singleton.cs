using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<TOwer> where TOwer : Singleton<TOwer>, new()
{
    private static TOwer s_instance = new TOwer();
    public static TOwer Instance  => s_instance;
    
    protected Singleton()
    {
        Debug.Assert(s_instance == null);
    }

    public virtual void Release() => s_instance = null;

}
