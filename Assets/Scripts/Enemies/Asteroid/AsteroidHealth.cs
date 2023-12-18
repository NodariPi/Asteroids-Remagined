using System;

using AsteroidsGame.Bullets;
using AsteroidsGame.Data;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Events;
using AsteroidsGame.PlayerShip;
using AsteroidsGame.Services;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.Enemies.Asteroid
{
    public class AsteroidHealth : IEnemyHealth, IInitializable
    {
        readonly Settings _Settings;
        readonly IEnemy _Asteroid;
        readonly Transform _Transform;
        readonly IEnemySpawner _Spawner;
        readonly SignalBus _SignalBus;
        readonly DiContainer _Container;

        public AsteroidHealth(AsteroidData asteroidData,
            IEnemy asteroid,
            Transform transform,
            IEnemySpawner spawner,
            SignalBus signalBus,
            DiContainer container)
        {
            _Settings = asteroidData.HealthSettings;
            _Asteroid = asteroid;
            _Transform = transform;
            _Spawner = spawner;
            _SignalBus = signalBus;
            _Container = container;
        }

        public void Initialize()
        {
            _SignalBus.Fire<AsteroidCreatedSignal>();
        }

        public void CheckForDamage(Collider2D col)
        {
            IBullet bullet = col.GetComponent<IBullet>();

            if (bullet != null)
            {
                OnDeath();
                bullet.OnHit();
            }
            else if (col.GetComponent<IPlayer>() != null)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            _SignalBus.Fire(new AsteroidDiedSignal(_Settings.ScoreReward));

            SpawnSmallerAsteroids();
            _Asteroid.Destroy();
        }

        private void SpawnSmallerAsteroids()
        {
            AsteroidEnemy.Factory _AsteroidFactory = _Container.ResolveId<AsteroidEnemy.Factory>(_Settings.AsteroidSizeToSpawn);

            if (_AsteroidFactory != null)
            {
                _Spawner.SpawnInsideCircle(_AsteroidFactory, _Settings.AsteroidSizeToSpawn, _Transform.position, _Settings.AsteroidsToSpawnAmount, true);
            }
        }

        [Serializable]
        public class Settings
        {
            public AsteroidSize AsteroidSizeToSpawn;
            public int AsteroidsToSpawnAmount;
            public int ScoreReward;
        }
    }
}