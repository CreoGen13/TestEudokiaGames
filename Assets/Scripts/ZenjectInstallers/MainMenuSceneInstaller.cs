using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class MainMenuSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuView mainMenuView;
        public override void InstallBindings()
        {
            BindMainMenu();
        }

        private void BindMainMenu()
        {
            Container
                .Bind<MainMenuPresenter>()
                .AsSingle()
                .WithArguments(new MainMenuModel(), mainMenuView);
        }
    }
}