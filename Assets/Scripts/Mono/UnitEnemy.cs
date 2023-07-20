using Scriptables;
using UnityEngine;
using Zenject;

namespace Mono
{
    public class UnitEnemy : MonoBehaviour
    {
        private string _enemyTag;
        private float _lifeTime;
        
        [Inject]
        private void Construct(ScriptableGameSettings gameSettings)
        {
            _enemyTag = gameSettings.enemyTag;
            _lifeTime = gameSettings.projectileLifeTime;
        }

        public void Reset()
        {
        }
    }
}