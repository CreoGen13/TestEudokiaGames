using Base;
using Base.Interfaces;
using Enemy;
using Mono;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Enemy
{
    public class EnemyFactory : IBaseFactory<EnemyPresenter>
    {
        private readonly DiContainer _container;
        private readonly GameObject _enemyPrefab;

        private int _count;

        [Inject]
        public EnemyFactory(GameObject enemyPrefab, DiContainer container)
        {
            _container = container;
            _enemyPrefab = enemyPrefab;
        }

        public EnemyPresenter Create(Transform parent)
        {
            _count++;
            var enemy = _container.InstantiatePrefabForComponent<EnemyView>(_enemyPrefab, parent);
            
            _container
                .Bind<EnemyPresenter>()
                .WithId(_count)
                .AsTransient()
                .WithArguments(new EnemyModel(), enemy)
                .OnInstantiated((context, instance) =>
                {
                    enemy.SetPresenter((EnemyPresenter)instance);
                })
                .NonLazy();

            return _container.ResolveId<EnemyPresenter>(_count);
        }
    }
}