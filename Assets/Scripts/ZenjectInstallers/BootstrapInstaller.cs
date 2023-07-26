using Infrastructure.Services;
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
        public override void InstallBindings()
        {
            BindScriptables();
            BindeServices();
        }
        
        private void BindeServices()
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
        }
    }
}