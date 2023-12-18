using AsteroidsGame.Data.Types;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.Enemies.Asteroid
{
    public class AsteroidEnemy : MonoBehaviour, IEnemy
    {
        private IEnemyHealth _Health;

        [Inject]
        private void Construct(IEnemyHealth health)
        {
            _Health = health;
        }

        public void Setup(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _Health.CheckForDamage(collision);
        }

        public class Factory : PlaceholderFactory<AsteroidSize, AsteroidEnemy>
        {

        }
    }
}