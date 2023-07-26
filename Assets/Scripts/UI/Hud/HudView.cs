using Base.Classes;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Hud
{
    public class HudView : BaseView
    {
        [Header("Stats")]
        [SerializeField] public TextMeshProUGUI damageStat;
        [SerializeField] public TextMeshProUGUI reloadStat;
        [SerializeField] public TextMeshProUGUI enemyHealthStat;
        [SerializeField] public TextMeshProUGUI enemySpeedStat;
        [SerializeField] public TextMeshProUGUI enemySpawnStat;
        [SerializeField] public TextMeshProUGUI supplySpawnStat;
        [SerializeField] public TextMeshProUGUI upgradeStat;
        
        [Header("Info")]
        [SerializeField] public TextMeshProUGUI enemiesLeft;
        [SerializeField] public TextMeshProUGUI points;
        
        private HudPresenter _hudPresenter;

        [Inject]
        private void Construct(HudPresenter hudPresenter)
        {
            _hudPresenter = hudPresenter;
        }
        private void OnDestroy()
        {
            _hudPresenter.Dispose();
        }
    }
}