using System;

using AsteroidsGame.Data;

using UnityEngine;

using Zenject;

using Random = UnityEngine.Random;

namespace AsteroidsGame.Enemies.Asteroid
{
    public class AsteroidMovement : IInitializable, ITickable
    {
        private Vector3 _Direction = Vector3.zero;
        private float _Speed = 0f;

        readonly Settings _Settings;
        readonly Transform _Transform;

        public AsteroidMovement(AsteroidData asteroidData,
            Transform transform)
        {
            _Settings = asteroidData.MovementSettings;
            _Transform = transform;
        }

        public void Initialize()
        {
            Setup();
        }

        public void Tick()
        {
            Move();
        }

        private void Setup()
        {
            _Direction = Random.insideUnitCircle.normalized;
            _Speed = Random.Range(_Settings.MinSpeed, _Settings.MaxSpeed);
        }

        private void Move()
        {
            _Transform.position += _Direction * _Speed * Time.deltaTime;
        }

        [Serializable]
        public class Settings
        {
            public float MinSpeed;
            public float MaxSpeed;
        }
    }
}