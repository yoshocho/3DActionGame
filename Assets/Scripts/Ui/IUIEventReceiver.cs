/// <summary>
/// Uiがデータを受取る用のインターフェース
/// </summary>
/// <typeparam name="T">データ</typeparam>
public interface IUIEventReceiver<T>
{
    void ReceiveData(T data);
}
