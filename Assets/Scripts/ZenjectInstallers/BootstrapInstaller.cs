using Infrastructure.Services.Input;
using Infrastructure.Services.Timer;
using Scriptables;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private ScriptableProjectSettings projectSettings;
        [SerializeField] private ScriptableUiSettings uiSettings;
        public override void InstallBindings()
        {
            BindScriptables();
            BindServices();
        }
        
        private void BindServices()
        {
            Container
                .Bind<InputService>()
                .AsSingle();
            
            Container
                .Bind<TimeService>()
                .AsSingle();
        }
        private void BindScriptables()
        {
            Container
                .Bind<ScriptableProjectSettings>()
                .FromInstance(projectSettings)
                .AsSingle();
            
            Container
                .Bind<ScriptableUiSettings>()
                .FromInstance(uiSettings)
                .AsSingle();
        }
    }
}