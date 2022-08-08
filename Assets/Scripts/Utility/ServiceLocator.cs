using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private static ServiceLocator s_instance = new ServiceLocator();
    public static ServiceLocator Instance => s_instance ??= new ServiceLocator();

    private ServiceLocator()
    {

    }

    private static class Cacher<T>
    {
        public static T s_instance;
    }
    public T Resolve<T>()
    {
        return Cacher<T>.s_instance;
    }
    public void Register<T>(T instance)
    {
        Cacher<T>.s_instance = instance;
    }
}