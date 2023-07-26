using UnityEngine;

namespace Base.Interfaces
{
    public interface IBaseFactory<out T>
    {
        T Create(Transform parent);
    }
}