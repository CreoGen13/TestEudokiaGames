using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Project Settings", menuName = "Scriptables/Project Settings", order = 0)]
    public class ScriptableProjectSettings : ScriptableObject
    {
        [Header("Pools")]
        public int poolSize;
    }
}