using System.Threading;
using Base.Classes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;
        
        private MainMenuPresenter _presenter;
        
        [Inject]
        private void Construct(MainMenuPresenter presenter)
        {
            _presenter = presenter;
        }
        private void Start()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            startButton.onClick.AddListener(_presenter.OnStartGameButton);
            exitButton.onClick.AddListener(_presenter.OnExitGameButton);
        }

        public void SetButtonsState(bool active)
        {
            startButton.interactable = active;
            exitButton.interactable = active;
        }
    }
}
