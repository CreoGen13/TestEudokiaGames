using System;
using UnityEngine;

namespace Base.Interfaces
{
    public interface IBaseGenericEnumPool<T, in TE> : IBasePool<T> where TE : Enum
    {
        public T Spawn(TE type);
        public T Spawn(TE type, Vector3 position);
    }
}