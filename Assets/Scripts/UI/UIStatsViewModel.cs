using AsteroidsGame.PlayerShip;
using AsteroidsGame.Services;

using Zenject;

namespace AsteroidsGame.UI
{
    public class UIStatsViewModel : IFixedTickable
    {
        private UIStatsView _View;
        private IScore _Score;
        private IPlayer _Player;

        public UIStatsViewModel(UIStatsView view,
            IScore score,
            IPlayer player)
        {
            _View = view;
            _Score = score;
            _Player = player;
        }

        public void FixedTick()
        {
            UpdateHealth();
            UpdateScore();
        }

        private void UpdateScore()
        {
            _View.m_TxtScoreCounter.text = _Score.GetCurrentScore.ToString();
        }

        private void UpdateHealth()
        {
            _View.m_TxtHealthCounter.text = _Player.Health.ToString();
        }
    }
}