using System;
using Base;
using Base.Classes;
using Base.Interfaces;
using Scriptables;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

namespace Enemy
{
    public class EnemyView : BaseView, IProceduralView
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Slider slider;
        
        private EnemyPresenter _presenter;
        private ScriptableGameSettings _gameSettings;
        private Camera _mainCamera;
        
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public EnemyPresenter Presenter => _presenter;

        [Inject]
        private void Construct(ScriptableGameSettings gameSettings, Camera mainCamera)
        {
            _mainCamera = mainCamera;
            _gameSettings = gameSettings;
            canvas.worldCamera = mainCamera;
        }
        public void SetPresenter(BasePresenter presenter)
        {
            _presenter = (EnemyPresenter)presenter;
        }

        public void SetHealth(float healthPercent)
        {
            if(!float.IsNaN(healthPercent))
                slider.SetValueWithoutNotify(healthPercent);
        }

        private void Update()
        {
            var difference = canvas.transform.position - _mainCamera.transform.position;
            canvas.transform.rotation = Quaternion.LookRotation(difference, _mainCamera.transform.up);
            
            _presenter.Update();
        }
        private void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}