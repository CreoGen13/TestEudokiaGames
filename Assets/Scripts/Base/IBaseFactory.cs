using UnityEngine;

namespace Base
{
    public interface IBaseFactory<T>
    {
        T Create(Transform parent);
    }
}