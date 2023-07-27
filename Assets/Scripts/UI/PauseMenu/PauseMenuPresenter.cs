using System;
using Base.Classes;
using Infrastructure.Utils;
using Scriptables;
using UniRx;
using Zenject;

namespace UI.PauseMenu
{
    public class PauseMenuPresenter : BaseGenericPresenter<PauseMenuModel, PauseMenuView>
    {
        public Action OnMenuClosed;
        
        private readonly ScriptableProjectSettings _projectSettings;

        private IDisposable _validationSubscription;

        [Inject]
        public PauseMenuPresenter(PauseMenuModel model, PauseMenuView view, ScriptableProjectSettings projectSettings)
            : base(model, view)
        {
            _projectSettings = projectSettings;
            
            Reset();
            Init();
        }

        protected sealed override void Init()
        {
            _validationSubscription = Model.Observe()
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

        public void OpenMenu()
        {
            Model.IsValidating = true;
            Model.Update();
            
            View.OpenMenu(() =>
            {
                Model.IsValidating = false;
                Model.Update();
            });
        }
        
        #region Buttons

        public void OnContinueButton()
        {
            Model.IsValidating = true;
            Model.Update();

            View.CloseMenu(() =>
            {
                Model.IsValidating = false;
                Model.Update();
            });
        }
        public async void OnRestartButton()
        {
            Model.IsValidating = true;
            Model.Update();
            
            await ExtraOperationsUtil.LoadScene(_projectSettings.gameSceneName, _projectSettings.loadSceneName);
        }
        public async void OnMainMenuButton()
        {
            Model.IsValidating = true;
            Model.Update();
            
            await ExtraOperationsUtil.LoadScene(_projectSettings.mainMenuSceneName, _projectSettings.loadSceneName);
        }

        #endregion
    }
}