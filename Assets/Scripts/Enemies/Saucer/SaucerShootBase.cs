using System;

using AsteroidsGame.Bullets;
using AsteroidsGame.Data;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Services;

using UnityEngine;

using Zenject;

using Random = UnityEngine.Random;

namespace AsteroidsGame.Enemies.Saucer
{
    public class SaucerShootBase : IInitializable, ITickable
    {
        private float _FireCooldownRemaining = 0f;
        private bool CanShoot { get { return _FireCooldownRemaining <= 0f; } }

        readonly Settings _Settings;
        readonly IObjectPooler<IBullet> _BulletPooler;
        readonly Bullet.Factory _BulletFactory;

        protected readonly Transform _Transform;

        public SaucerShootBase(SaucerData saucerData,
            Transform transform,
            IObjectPooler<IBullet> bulletPooler,
            Bullet.Factory bulletFactory)
        {
            _Settings = saucerData.ShootSettings;
            _Transform = transform;
            _BulletPooler = bulletPooler;
            _BulletFactory = bulletFactory;
        }

        public void Initialize()
        {
            _FireCooldownRemaining = _Settings.FireCooldown;
        }

        public void Tick()
        {
            CheckForShoot();
            UpdateCooldown();
        }

        private void CheckForShoot()
        {
            if (CanShoot)
            {
                Shoot();
                _FireCooldownRemaining = _Settings.FireCooldown;
            }
        }

        private void UpdateCooldown()
        {
            if (_FireCooldownRemaining > 0f)
            {
                _FireCooldownRemaining -= Time.deltaTime;
            }
        }

        private void Shoot()
        {
            IBullet bullet;

            if (!_BulletPooler.GetObject(out bullet))
                bullet = _BulletFactory.Create();

            Vector3 direction = GetDir();
            Vector3 pos = direction * _Settings.CircleRadius;
            pos += _Transform.position;

            Quaternion rot = Quaternion.Euler(0f, 0f, GetRotationAngle(direction));

            bullet.SetupBullet(pos, rot, _Settings.BulletSpeed, _Settings.BulletLifetime, _Settings.EnemyType);
        }

        internal virtual Vector3 GetDir()
        {
            return Random.insideUnitCircle.normalized;
        }

        private float GetRotationAngle(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            return -angle;
        }

        [Serializable]
        public class Settings
        {
            public float CircleRadius;
            public float FireCooldown;
            public float BulletSpeed;
            public float BulletLifetime;
            public EnemyTypes EnemyType;
        }
    }
}