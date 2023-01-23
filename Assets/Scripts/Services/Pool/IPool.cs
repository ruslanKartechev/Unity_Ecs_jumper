namespace Services.Pool
{
    public interface IPool<T>
    {
        T GetItem();
        void Return(T target);
        void CollectAllBack();

    }
}