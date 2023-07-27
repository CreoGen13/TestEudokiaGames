using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Project Settings", menuName = "Scriptables/Project Settings", order = 0)]
    public class ScriptableProjectSettings : ScriptableObject
    {
        [Header("Scene Names")]
        public string mainMenuSceneName;
        public string loadSceneName;
        public string gameSceneName;
        
        [Header("Pools")]
        public int poolSize;
    }
}