using System;

using AsteroidsGame.Enemies;
using AsteroidsGame.PlayerShip;

using UnityEngine;

using Zenject;

using Random = UnityEngine.Random;

namespace AsteroidsGame.Services
{
    public class EnemySpawner : IEnemySpawner
    {
        readonly Settings _Settings;
        readonly IPlayer _Player;
        readonly Camera _Camera;

        public EnemySpawner(Settings settings,
            IPlayer player,
            Camera camera)
        {
            _Settings = settings;
            _Player = player;
            _Camera = camera;
        }

        public void SpawnOnRandomLocation<U, T>(PlaceholderFactory<U, T> factory, U data, int amount, bool randomDir)
        {
            for (int i = 0; i < amount; i++)
            {
                DoSpawn(factory, data, GetRandomPointOnScreen(), randomDir ? GetRandomDirection() : Quaternion.identity);
            }
        }

        public void SpawnInsideCircle<U, T>(PlaceholderFactory<U, T> factory, U data, Vector3 _Position, int amount, bool randomDir)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 pos = _Position;
                pos += Random.insideUnitCircle * _Settings.AsteroidSpawnRadius;

                DoSpawn(factory, data, pos, randomDir ? GetRandomDirection() : Quaternion.identity);
            }
        }

        private void DoSpawn<U, T>(PlaceholderFactory<U, T> factory, U data, Vector3 pos, Quaternion rot)
        {
            IEnemy _Enemy = factory.Create(data) as IEnemy;
            _Enemy.Setup(pos, rot);
        }

        private Vector2 GetRandomPointOnScreen()
        {
            Vector2 minScreenBounds = _Camera.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 maxScreenBounds = _Camera.ViewportToWorldPoint(new Vector2(1, 1));

            Vector2 randomPoint = new Vector3(
                Random.Range(minScreenBounds.x, maxScreenBounds.x),
                Random.Range(minScreenBounds.y, maxScreenBounds.y));

            //Avoid player
            if (Vector3.Distance(randomPoint, _Player.Position) < _Settings.MinDistanceFromPlayer)
            {
                randomPoint += (randomPoint - (Vector2)_Player.Position).normalized * _Settings.MinDistanceFromPlayer;
            }

            return randomPoint;
        }

        private Quaternion GetRandomDirection()
        {
            float randomAngle = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);

            return randomRotation;
        }

        [Serializable]
        public class Settings
        {
            public float MinDistanceFromPlayer;
            public float AsteroidSpawnRadius;
        }
    }
}