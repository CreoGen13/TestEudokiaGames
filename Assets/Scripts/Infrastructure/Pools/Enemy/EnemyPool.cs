using System;
using System.Collections.Generic;
using Base.Interfaces;
using Enemy;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Pools.Enemy
{
    public class EnemyPool : IBaseGenericPool<EnemyPresenter>
    {
        public Action<EnemyPresenter> OnDestinationReached;
        public Action OnDeath;
        
        public Action<int> OnCountChanged;
        
        private readonly Transform _parent;
        private readonly Transform [] _enemyPoints;
        private readonly EnemyFactory _factory;
        private readonly Queue<EnemyPresenter> _enemies = new Queue<EnemyPresenter>();
        private readonly List<EnemyPresenter> _spawnedEnemies = new List<EnemyPresenter>();

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
                var enemyPresenter = _factory.Create(_parent);
                enemyPresenter.SetActive(false);
                enemyPresenter.OnDestinationReached += () =>
                {
                    OnDestinationReached?.Invoke(enemyPresenter);
                };
                enemyPresenter.OnDeath += () =>
                {
                    OnDeath?.Invoke();
                    Return(enemyPresenter);
                };

                _enemies.Enqueue(enemyPresenter);
            }
        }

        public int Clear()
        {
            var count = _spawnedEnemies.Count;
            foreach (var spawnedEnemy in _spawnedEnemies.ToArray())
            {
                Return(spawnedEnemy);
            }
            OnCountChanged?.Invoke(_spawnedEnemies.Count);

            return count;
        }
        public EnemyPresenter Spawn()
        {
            var enemyPresenter = _enemies.Dequeue();
            enemyPresenter.SetActive(true);
            _spawnedEnemies.Add(enemyPresenter);
            enemyPresenter.OnDestinationReached?.Invoke();
            OnCountChanged?.Invoke(_spawnedEnemies.Count);
            
            return enemyPresenter;
        }
        public EnemyPresenter Spawn(Vector3 position)
        {
            var enemyPresenter = Spawn();
            enemyPresenter.SetPosition(position);
            
            return enemyPresenter;
        }
        public void SpawnRandom()
        {
            var enemyPresenter = Spawn();
            int random = Mathf.RoundToInt(Random.value * (_enemyPoints.Length - 1));
            enemyPresenter.SetPosition(_enemyPoints[random].position);
        }
        public void Return(EnemyPresenter enemy)
        {
            enemy.SetActive(false);
            enemy.Reset();
            _enemies.Enqueue(enemy);
            _spawnedEnemies.Remove(enemy);
            OnCountChanged?.Invoke(_spawnedEnemies.Count);
        }

        public void SetStats(float health, float speed)
        {
            foreach (var enemy in _enemies)
            {
                enemy.SetStats(health, speed);
            }
        }
    }
}