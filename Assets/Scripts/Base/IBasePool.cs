using Vector3 = UnityEngine.Vector3;

namespace Base
{
    public interface IBasePool <T>
    {
        public void Init(int count);
        public T Spawn(Vector3 position);
        public void Return(T poolMember);
    }
}