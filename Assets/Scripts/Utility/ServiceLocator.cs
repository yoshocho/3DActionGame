using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    #region
    //private static ServiceLocator s_instance = new ServiceLocator();
    //public static ServiceLocator Instance => s_instance ??= new ServiceLocator();

    //private ServiceLocator()
    //{

    //}
    #endregion

    //public static bool IsValid => ;
    private static class Cacher<T>
    {
        public static T s_instance;
    }
    public static T GetInstance<T>()
    {
        return Cacher<T>.s_instance;
    }
    public static void Register<T>(T instance)
    {
        Cacher<T>.s_instance = instance;
    }

}