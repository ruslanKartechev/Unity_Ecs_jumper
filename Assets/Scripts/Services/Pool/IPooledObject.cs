namespace Services.Pool
{
    public interface IPooledObject<T>
    {
        void Init(IPool<T> pool);
        void CollectBack();
        
    }
}