using System;
using System.Collections;

using AsteroidsGame.Data.Types;
using AsteroidsGame.Enemies.Asteroid;
using AsteroidsGame.Enemies.Saucer;
using AsteroidsGame.Events;
using AsteroidsGame.Services;

using UnityEngine;

using Zenject;

using Random = UnityEngine.Random;

namespace AsteroidsGame.GameLogic
{
    public class WaveManager : IInitializable, ITickable
    {
        private enum GameState
        {
            Starting,
            InProgress
        }

        private int _AsteroidEnemies;
        private int _SaucerEnemies;

        private int _AllEnemies { get { return _AsteroidEnemies + _SaucerEnemies; } }

        private GameState _CurrentState = GameState.Starting;

        readonly Settings _Settings;
        readonly IEnemySpawner _EnemySpawner;
        readonly IScore _Score;
        readonly SignalBus _SignalBus;
        readonly CoroutineRunner _CoroutineRunner;

        readonly AsteroidEnemy.Factory _AsteroidFactory;
        readonly SaucerEnemy.Factory _BigSaucerFactory;
        readonly SaucerEnemy.Factory _SmallSaucerFactory;

        public WaveManager(Settings settings,
            IEnemySpawner enemySpawner,
            IScore score,
            SignalBus signalBus,
            CoroutineRunner coroutineRunner,
            [Inject(Id = AsteroidSize.Big)] AsteroidEnemy.Factory asteroidFactory,
            [Inject(Id = SaucerSize.Big)] SaucerEnemy.Factory bigSaucerFactory,
            [Inject(Id = SaucerSize.Small)] SaucerEnemy.Factory smallSaucerFactory)
        {
            _Settings = settings;
            _EnemySpawner = enemySpawner;
            _Score = score;
            _SignalBus = signalBus;
            _CoroutineRunner = coroutineRunner;

            _AsteroidFactory = asteroidFactory;
            _BigSaucerFactory = bigSaucerFactory;
            _SmallSaucerFactory = smallSaucerFactory;
        }

        public void Initialize()
        {
            SetupEventListeners();
            StartNewWave();
        }

        public void Tick()
        {
            CheckForWaveEnd();
        }

        private void SetupEventListeners()
        {
            _SignalBus.Subscribe<AsteroidCreatedSignal>(() => _AsteroidEnemies++);
            _SignalBus.Subscribe<SaucerCreatedSignal>(() => _SaucerEnemies++);
            _SignalBus.Subscribe<AsteroidDiedSignal>(() => _AsteroidEnemies--);
            _SignalBus.Subscribe<SaucerDiedSignal>(() => _SaucerEnemies--);
        }

        private void StartNewWave()
        {
            _CurrentState = GameState.Starting;

            _CoroutineRunner.StopCoroutine(IE_CheckForSaucerSpawn());
            _CoroutineRunner.StartCoroutine(IE_CreateNewWave());
        }

        private void CheckForWaveEnd()
        {
            if (_CurrentState == GameState.InProgress)
            {
                if (_AllEnemies <= 0)
                {
                    StartNewWave();
                }
            }
        }

        IEnumerator IE_CreateNewWave()
        {
            yield return new WaitForSeconds(_Settings.WaveDelay);

            _CurrentState = GameState.InProgress;

            _EnemySpawner.SpawnOnRandomLocation(_AsteroidFactory, AsteroidSize.Big, CalculateAsteroidsToSpawn(), true);
            _CoroutineRunner.StartCoroutine(IE_CheckForSaucerSpawn());
        }

        IEnumerator IE_CheckForSaucerSpawn()
        {
            while (_CurrentState == GameState.InProgress)
            {
                if (_AllEnemies > _Settings.SaucerSpawnEnemyThreshold || _SaucerEnemies > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    yield return new WaitForSeconds(_Settings.SaucerSpawnCheckDelay);
                    SpawnSaucer();
                }
            }
        }

        private void SpawnSaucer()
        {
            float currentSpawnChance = Random.value;

            if (_Settings.SaucerSpawnChance >= currentSpawnChance)
            {
                SaucerEnemy.Factory saucerToSpawn = _BigSaucerFactory;
                SaucerSize saucerSize = SaucerSize.Big;

                if (_Settings.SmallSaucerSpawnChance >= currentSpawnChance)
                {
                    saucerToSpawn = _SmallSaucerFactory;
                    saucerSize = SaucerSize.Small;
                }

                _EnemySpawner.SpawnOnRandomLocation(saucerToSpawn, saucerSize, 1, false);
            }
        }

        private int CalculateAsteroidsToSpawn()
        {
            int asteroidsToSpawn = _Settings.AsteroidStartingCount;
            int incrementMultiplier = _Score.GetCurrentScore / _Settings.AsteroidIncrementScoreThreshold;
            asteroidsToSpawn += _Settings.AsteroidIncrementCount * incrementMultiplier;

            return asteroidsToSpawn;
        }

        [Serializable]
        public class Settings
        {
            public int AsteroidStartingCount;
            public int AsteroidIncrementCount;
            public int AsteroidIncrementScoreThreshold;

            public float SaucerSpawnCheckDelay;
            public float SaucerSpawnChance;
            public float SmallSaucerSpawnChance;
            public int SaucerSpawnEnemyThreshold;

            public float WaveDelay;
        }
    }
}