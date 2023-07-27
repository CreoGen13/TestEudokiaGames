using System;
using DG.Tweening;
using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Transform window;
        
        private ScriptableUiSettings _uiSettings;

        [Inject]
        public void Construct(ScriptableUiSettings uiSettings)
        {
            _uiSettings = uiSettings;
        }

        private void Start()
        {
            window.gameObject.SetActive(false);
        }

        public void Open()
        {
            background.enabled = true;
            window.gameObject.SetActive(true);
            background.color = new Color(0, 0, 0, 0);
            text.color = new Color(0, 0, 0, 0);
            
            Sequence sequence = DOTween.Sequence();
            sequence.Join(
                background.DOColor(
                    _uiSettings.backgroundFadedColor,
                    _uiSettings.fadeAnimationDuration));
            sequence.Join(
                text.DOColor(
                    Color.white,
                    _uiSettings.fadeAnimationDuration));
            sequence.Play();
        }
    }
}