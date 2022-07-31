/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IUIEventReceiver<T>
{
    void ReceiveData(T data);
}
