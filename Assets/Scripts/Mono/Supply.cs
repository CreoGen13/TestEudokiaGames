using System;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Mono
{
    public class Supply : MonoBehaviour
    {
        [SerializeField] private GameObject killMesh;
        [SerializeField] private GameObject stopMesh;

        private ScriptableGameSettings _gameSettings;

        public SupplyType SupplyType { get; private set; }

        [Inject]
        private void Construct(ScriptableGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void SetType(SupplyType supplyType)
        {
            SupplyType = supplyType;
            
            switch (supplyType)
            {
                case SupplyType.KillAll:
                {
                    killMesh.SetActive(true);
                    stopMesh.SetActive(false);
                    break;
                }
                case SupplyType.StopSpawn:
                {
                    killMesh.SetActive(false);
                    stopMesh.SetActive(true);
                    break;
                }
            }
        }

        public void Update()
        {
            transform.rotation *= Quaternion.Euler(_gameSettings.supplyRotation);
        }
    }

    public enum SupplyType
    {
        KillAll,
        StopSpawn,
    }
}