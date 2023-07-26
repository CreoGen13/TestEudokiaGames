using System;
using System.Collections.Generic;
using Base.Interfaces;
using Mono;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Pools.Supply
{
    public class SupplyPool : IBaseGenericEnumPool<Mono.Supply, SupplyType>
    {
        private readonly Transform _parent;
        private readonly Transform [] _supplyPoints;
        private readonly SupplyFactory _factory;
        private readonly Queue<Mono.Supply> _supplies = new Queue<Mono.Supply>();
        private readonly List<Mono.Supply> _spawnedSupplies = new List<Mono.Supply>();

        [Inject]
        public SupplyPool(SupplyFactory factory, Transform parent, Transform [] supplyPoints)
        {
            _parent = parent;
            _factory = factory;
            _supplyPoints = supplyPoints;
        }

        public void Init(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var supply = _factory.Create(_parent);
                supply.gameObject.SetActive(false);

                _supplies.Enqueue(supply);
            }
        }

        public void Clear()
        {
            foreach (var spawnedEnemy in _spawnedSupplies)
            {
                Return(spawnedEnemy);
            }
        }
        public Mono.Supply Spawn(SupplyType type)
        {
            if (_supplies.Count == 0)
                return null;
            
            var supply = _supplies.Dequeue();
            supply.gameObject.SetActive(true);
            supply.SetType(type);
            _spawnedSupplies.Add(supply);
            
            return supply;
        }
        public Mono.Supply Spawn(SupplyType type, Vector3 position)
        {
            var supply = Spawn(type);
            supply.transform.position = position;
            
            return supply;
        }
        public void SpawnRandom()
        {
            int randomType = Mathf.RoundToInt(Random.value * (Enum.GetValues(typeof(SupplyType)).Length - 1));
            var supply = Spawn((SupplyType)randomType);
            int randomPos = Mathf.RoundToInt(Random.value * (_supplyPoints.Length - 1));
            supply.transform.position = _supplyPoints[randomPos].position;
        }
        public void Return(Mono.Supply supply)
        {
            supply.gameObject.SetActive(false); ;
            _supplies.Enqueue(supply);
            _spawnedSupplies.Remove(supply);
        }
    }
}