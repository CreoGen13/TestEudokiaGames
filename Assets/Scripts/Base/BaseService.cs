namespace Base
{
    public abstract class BaseService
    {
        public abstract void Update();

        public virtual void OnEnable(){}
        public virtual void OnDisable(){}
    }
}