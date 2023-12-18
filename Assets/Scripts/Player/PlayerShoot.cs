using System;

using AsteroidsGame.Bullets;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Input;
using AsteroidsGame.Services;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.PlayerShip
{
    public class PlayerShoot : ITickable
    {
        private float _FireCooldownRemaining = 0f;
        private bool CanShoot { get { return _FireCooldownRemaining <= 0f; } }

        readonly Settings _Settings;
        readonly IInputManager _InputManager;
        readonly IPlayerVisibility _PlayerVisibility;
        readonly IObjectPooler<IBullet> _BulletPooler;
        readonly Bullet.Factory _BulletFactory;
        readonly Transform[] _FirePoints;

        public PlayerShoot(Settings settings,
            IInputManager input,
            IPlayerVisibility playerVisibility,
            IObjectPooler<IBullet> pooler,
            Bullet.Factory bulletFactory,
            Transform[] firePoints)
        {
            _Settings = settings;
            _InputManager = input;
            _PlayerVisibility = playerVisibility;
            _BulletPooler = pooler;
            _BulletFactory = bulletFactory;
            _FirePoints = firePoints;
        }

        public void Tick()
        {
            CheckForShoot();
            UpdateCooldown();
        }

        private void CheckForShoot()
        {
            if (_PlayerVisibility.IsDisabled)
                return;

            if (CanShoot)
            {
                if (_InputManager.GetFireButton())
                {
                    Shoot();

                    _FireCooldownRemaining = _Settings.FireCooldown;
                }
            }
        }

        private void Shoot()
        {
            foreach (Transform point in _FirePoints)
            {
                IBullet bullet;

                if (!_BulletPooler.GetObject(out bullet))
                    bullet = _BulletFactory.Create();

                bullet.SetupBullet(point.position, point.rotation, _Settings.BulletSpeed, _Settings.BulletLifetime, _Settings.EnemyType);
            }
        }

        private void UpdateCooldown()
        {
            if (_FireCooldownRemaining > 0f)
            {
                _FireCooldownRemaining -= Time.deltaTime;
            }
        }

        [Serializable]
        public class Settings
        {
            public float FireCooldown;
            public float BulletSpeed;
            public float BulletLifetime;
            public EnemyTypes EnemyType;
        }
    }
}