/// <summary>
/// Ui���f�[�^������p�̃C���^�[�t�F�[�X
/// </summary>
/// <typeparam name="T">�f�[�^</typeparam>
public interface IUIEventReceiver<T>
{
    void ReceiveData(T data);
}
