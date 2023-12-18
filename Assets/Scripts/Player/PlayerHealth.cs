using System;

using AsteroidsGame.Bullets;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Enemies;
using AsteroidsGame.Events;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.PlayerShip
{
    public class PlayerHealth : IPlayerHealth, IInitializable
    {
        public int CurrentHealth => _CurrentHealth;
        private int _CurrentHealth;

        readonly Settings _Settings;
        readonly IPlayerVisibility _PlayerVisibility;
        readonly SignalBus _SignalBus;

        public PlayerHealth(Settings settings,
            IPlayerVisibility playerVisibility,
            SignalBus signalBus)
        {
            _Settings = settings;
            _PlayerVisibility = playerVisibility;
            _SignalBus = signalBus;
        }

        public void Initialize()
        {
            _CurrentHealth = _Settings.Health;
        }

        public void CheckForDamage(Collider2D col)
        {
            IBullet bullet = col.GetComponent<IBullet>();

            if (bullet != null)
            {
                if (bullet.OriginType != EnemyTypes.Player)
                {
                    OnDamageReceived();
                    bullet.OnHit();
                }
            }
            else if (col.GetComponent<IEnemy>() != null)
            {
                OnDamageReceived();
            }
        }

        private void OnDamageReceived()
        {
            if (_PlayerVisibility.IsDisabled)
                return;

            _SignalBus.Fire<PlayerDiedSignal>();

            _CurrentHealth -= 1;
            CheckForDeath();
        }

        private void CheckForDeath()
        {
            if (_CurrentHealth <= 0)
                OnDeath();
            else
                _PlayerVisibility.OnSoftDeath();
        }

        private void OnDeath()
        {
            _SignalBus.Fire<GameOverSignal>();
            _PlayerVisibility.OnDeath();
        }

        [Serializable]
        public class Settings
        {
            public int Health;
        }
    }
}