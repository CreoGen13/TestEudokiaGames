using Vector3 = UnityEngine.Vector3;

namespace Base.Interfaces
{
    public interface IBaseGenericPool <T> : IBasePool<T>
    {
        public T Spawn();
        public T Spawn(Vector3 position);
    }
}