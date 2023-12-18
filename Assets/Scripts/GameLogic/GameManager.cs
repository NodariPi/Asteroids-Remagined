using System;
using System.Collections;

using AsteroidsGame.Events;
using AsteroidsGame.Services;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

namespace AsteroidsGame.GameLogic
{
    public class GameManager : IInitializable
    {
        readonly Settings _Settings;
        readonly CoroutineRunner _CoroutineRunner;
        readonly SignalBus _SignalBus;

        public GameManager(Settings settings,
            CoroutineRunner coroutineRunner,
            SignalBus signalBus)
        {
            _Settings = settings;
            _CoroutineRunner = coroutineRunner;
            _SignalBus = signalBus;
        }

        public void Initialize()
        {
            _SignalBus.Subscribe<GameOverSignal>(OnGameOver);
        }

        private void OnGameOver()
        {
            _CoroutineRunner.StartCoroutine(IE_GameOver());
        }

        IEnumerator IE_GameOver()
        {
            yield return new WaitForSeconds(_Settings.GameOverRestartDelay);

            EndGame();
        }

        private void EndGame()
        {
            SceneManager.LoadScene(0);
        }

        [Serializable]
        public class Settings
        {
            public float GameOverRestartDelay;
        }
    }
}