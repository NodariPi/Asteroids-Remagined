using System;

using AsteroidsGame.Bullets;
using AsteroidsGame.Data;
using AsteroidsGame.Data.Types;
using AsteroidsGame.Events;
using AsteroidsGame.PlayerShip;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.Enemies.Saucer
{
    public class SaucerHealth : IEnemyHealth, IInitializable
    {
        readonly Settings _Settings;
        readonly IEnemy _Saucer;
        readonly SignalBus _SignalBus;

        public SaucerHealth(SaucerData saucerData,
            IEnemy saucer,
            SignalBus signalBus)
        {
            _Settings = saucerData.HealthSettings;
            _Saucer = saucer;
            _SignalBus = signalBus;
        }

        public void Initialize()
        {
            _SignalBus.Fire<SaucerCreatedSignal>();
        }

        public void CheckForDamage(Collider2D col)
        {
            IBullet bullet = col.GetComponent<IBullet>();

            if (bullet != null)
            {
                if (bullet.OriginType != EnemyTypes.Saucer)
                {
                    OnDeath();
                    bullet.OnHit();
                }
            }
            else if (col.GetComponent<IPlayer>() != null)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            _SignalBus.Fire(new SaucerDiedSignal(_Settings.ScoreReward));
            _Saucer.Destroy();
        }

        [Serializable]
        public class Settings
        {
            public int ScoreReward;
        }
    }
}