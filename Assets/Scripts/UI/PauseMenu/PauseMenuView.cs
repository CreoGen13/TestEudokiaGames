using System;
using Base.Classes;
using DG.Tweening;
using Scriptables;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PauseMenu
{
    public class PauseMenuView : BaseView
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Transform window;
        [SerializeField] private Image background;

        private ScriptableUiSettings _uiSettings;
        private PauseMenuPresenter _presenter;
        private Sequence _sequence;

        [Inject]
        private void Construct(PauseMenuPresenter presenter, ScriptableUiSettings uiSettings)
        {
            _uiSettings = uiSettings;
            _presenter = presenter;
        }
        private void Start()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            continueButton.onClick.AddListener(_presenter.OnContinueButton);
            restartButton.onClick.AddListener(_presenter.OnRestartButton);
            mainMenuButton.onClick.AddListener(_presenter.OnMainMenuButton);
        }
        public void SetButtonsState(bool active)
        {
            continueButton.interactable = active;
            restartButton.interactable = active;
            mainMenuButton.interactable = active;
        }

        public void OpenMenu(Action callback)
        {
            background.enabled = true;
            background.color = new Color(0, 0, 0, 0);
            window.gameObject.SetActive(true);
            var pos = window.localPosition;
            
            window.localPosition = new Vector3(pos.x, Screen.height, pos.z);
            _sequence = DOTween.Sequence();
            _sequence.Join(
                window.DOLocalMoveY(
                    0,
                    _uiSettings.openAnimationDuration));
            _sequence.Join(
                background.DOColor(
                    _uiSettings.backgroundFadedColor,
                    _uiSettings.fadeAnimationDuration));
            _sequence.OnComplete(() =>
            {
                callback?.Invoke();
            });
            _sequence.Play();
        }
        public void CloseMenu(Action callback)
        {
            var pos = window.localPosition;
            window.localPosition = new Vector3(pos.x, 0, pos.z);
            _sequence = DOTween.Sequence();
            _sequence.Join(
                window.DOLocalMoveY(
                    Screen.height,
                    _uiSettings.closeAnimationDuration));
            _sequence.Join(
                background.DOColor(
                   new Color(0, 0, 0, 0),
                    _uiSettings.fadeAnimationDuration));
            _sequence.OnComplete(() =>
            {
                background.enabled = false;
                window.gameObject.SetActive(false);
                callback?.Invoke();
                _presenter.OnMenuClosed?.Invoke();
            });
            _sequence.Play();
        }
    }
}