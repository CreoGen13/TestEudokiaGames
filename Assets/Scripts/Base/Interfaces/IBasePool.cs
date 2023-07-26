namespace Base.Interfaces
{
    public interface IBasePool<in T>
    {
        public void Init(int count);
        public void Return(T poolMember);
    }
}