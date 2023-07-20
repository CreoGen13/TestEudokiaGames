using System;
using System.Collections.Generic;
using Base;
using Mono;
using UnityEngine;
using Zenject;

namespace Infrastructure.Pools.Enemy
{
    public class EnemyPool : IBasePool<UnitEnemy>
    {
        private readonly Transform _parent;
        private readonly Transform [] _enemyPoints;
        private readonly EnemyFactory _factory;
        private readonly Queue<UnitEnemy> _projectiles = new Queue<UnitEnemy>();

        [Inject]
        public EnemyPool(EnemyFactory factory, Transform parent, Transform [] enemyPoints)
        {
            _parent = parent;
            _factory = factory;
            _enemyPoints = enemyPoints;
        }

        public void Init(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var projectile = _factory.Create(_parent);
                projectile.gameObject.SetActive(false);

                _projectiles.Enqueue(projectile);
            }
        }

        public UnitEnemy Spawn(Vector3 position)
        {
            var projectile = _projectiles.Dequeue();
            projectile.gameObject.SetActive(true);
            projectile.transform.position = position;
            return projectile;
        }

        public void Return(UnitEnemy projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.Reset();
            _projectiles.Enqueue(projectile);
        }

        public void SpawnAtPoints()
        {
            foreach (var point in _enemyPoints)
            {
                Spawn(point.position);
            }
        }
    }
}