using System;
using System.Threading;
using Base.Classes;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using Scriptables;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Application = UnityEngine.Device.Application;

namespace UI.MainMenu
{
    public class MainMenuPresenter : BaseGenericPresenter<MainMenuModel, MainMenuView>
    {
        private readonly ScriptableProjectSettings _projectSettings;
        
        private IDisposable _validatingSubscription;

        [Inject]
        public MainMenuPresenter(MainMenuModel model, MainMenuView view, ScriptableProjectSettings projectSettings)
            : base(model, view)
        {
            _projectSettings = projectSettings;
            Reset();
            Init();
        }
        protected sealed override void Init()
        {
            _validatingSubscription = Model.Observe()
                .Select(model => model.IsValidating)
                .DistinctUntilChanged(state => state.GetHashCode())
                .Subscribe(
                    value =>
                    {
                        View.SetButtonsState(!value);
                    })
                .AddTo(Disposable);
        }
        public sealed override void Reset()
        {
            Model.IsValidating = false;
            Model.Update();
        }

        #region Buttons

        public async void OnStartGameButton()
        {
            Model.IsValidating = true;
            Model.Update();

            await ExtraOperationsUtil.LoadScene(_projectSettings.gameSceneName, _projectSettings.loadSceneName);
        }
        public void OnExitGameButton()
        {
            Model.IsValidating = true;
            Model.Update();
            
            Application.Quit();
        }

        #endregion
    }
}