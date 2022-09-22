namespace ObjectPool
{
    public interface IPoolObject
    {
        public void SetUp();
        public void Sleep();

        public event System.Action OnRelease;
    }
}