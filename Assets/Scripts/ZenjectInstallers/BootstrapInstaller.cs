using Infrastructure.Services.Input;
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
            BindInputService();
        }
        
        private void BindInputService()
        {
            Container
                .Bind<InputService>()
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