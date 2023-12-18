using AsteroidsGame.Events;

using Zenject;

namespace AsteroidsGame.Services
{
    public class ScoreManager : IScore, IInitializable
    {
        public int GetCurrentScore => currentScore;
        private int currentScore = 0;

        readonly SignalBus _SignalBus;

        public ScoreManager(SignalBus signalBus)
        {
            _SignalBus = signalBus;
        }

        public void Initialize()
        {
            _SignalBus.Subscribe((AsteroidDiedSignal data) => AddScore(data.ScoreReward));
            _SignalBus.Subscribe((SaucerDiedSignal data) => AddScore(data.ScoreReward));
        }

        private void AddScore(int scoreToAdd)
        {
            currentScore += scoreToAdd;
        }
    }
}