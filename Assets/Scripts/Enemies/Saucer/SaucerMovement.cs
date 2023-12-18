using System;

using AsteroidsGame.Data;

using UnityEngine;

using Zenject;

using Random = UnityEngine.Random;

namespace AsteroidsGame.Enemies.Saucer
{
    public class SaucerMovement : IInitializable, ITickable
    {
        private float _Timer = 0f;
        private Vector3 _Direction = Vector3.zero;
        private float _Speed = 0f;

        private bool CanChangeDir { get { return _Timer <= 0f; } }

        readonly Settings _Settings;
        readonly Transform _Transform;

        public SaucerMovement(SaucerData saucerData,
            Transform transform)
        {
            _Settings = saucerData.MovementSettings;
            _Transform = transform;
        }

        public void Initialize()
        {
            SetDirection();
        }

        public void Tick()
        {
            Move();
            CheckForDirectionChange();
        }

        private void Move()
        {
            _Transform.position += _Direction * _Speed * Time.deltaTime;
        }

        private void SetDirection()
        {
            _Timer = _Settings.ChangeDirectionInterval;
            _Direction = Random.insideUnitCircle.normalized;
            _Speed = Random.Range(_Settings.MinSpeed, _Settings.MaxSpeed);
        }

        private void CheckForDirectionChange()
        {
            if (CanChangeDir)
                SetDirection();
            else
                _Timer -= Time.deltaTime;
        }

        [Serializable]
        public class Settings
        {
            public float MinSpeed;
            public float MaxSpeed;
            public float ChangeDirectionInterval;
        }
    }
}