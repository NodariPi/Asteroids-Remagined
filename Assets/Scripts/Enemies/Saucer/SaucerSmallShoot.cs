using AsteroidsGame.Bullets;
using AsteroidsGame.Data;
using AsteroidsGame.PlayerShip;
using AsteroidsGame.Services;

using UnityEngine;

namespace AsteroidsGame.Enemies.Saucer
{
    public class SaucerSmallShoot : SaucerShootBase
    {
        readonly IPlayer _Player;

        public SaucerSmallShoot(SaucerData settings,
            IPlayer player,
            Transform transform,
            IObjectPooler<IBullet> bulletPooler,
            Bullet.Factory bulletFactory) : base(settings, transform, bulletPooler, bulletFactory)
        {
            _Player = player;
        }

        internal override Vector3 GetDir()
        {
            return (_Player.Position - _Transform.position).normalized;
        }
    }
}