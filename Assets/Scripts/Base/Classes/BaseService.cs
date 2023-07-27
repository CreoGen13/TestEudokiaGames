namespace Base.Classes
{
    public abstract class BaseService
    {
        public abstract void Update();

        public abstract void Stop();
        public abstract void Resume();

        public virtual void OnEnable(){}
        public virtual void OnDisable(){}
    }
}