using Base;
using Mono;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Enemy
{
    public class EnemyFactory : IBaseFactory<UnitEnemy>
    {
        private readonly DiContainer _container;
        private readonly GameObject _enemyPrefab;

        public EnemyFactory(GameObject enemyPrefab, DiContainer container)
        {
            _container = container;
            _enemyPrefab = enemyPrefab;
        }

        public UnitEnemy Create(Transform parent)
        {
            var enemy = _container.InstantiatePrefabForComponent<UnitEnemy>(_enemyPrefab);
            enemy.transform.SetParent(parent);
            return enemy;
        }
    }
}