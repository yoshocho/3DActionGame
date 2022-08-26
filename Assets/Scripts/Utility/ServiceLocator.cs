public static class ServiceLocator<T> where T : class
{
    public static bool IsValid => Instance != null;

    public static T Instance { get; private set; }

    public static void Register(T instance)
    {
        Instance = instance;
    }
    public static void Remove(T instance)
    {
        if (Instance == instance)
        {
            Instance = null;
        }
    }

}