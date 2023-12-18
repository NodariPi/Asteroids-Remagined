using AsteroidsGame.Data.Types;
using AsteroidsGame.Services;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.Bullets
{
    public class Bullet : MonoBehaviour, IBullet
    {
        public EnemyTypes OriginType => _Origin;
        private EnemyTypes _Origin;

        private IObjectPooler<IBullet> _BulletPooler;
        private float _Speed = 0.0f;

        [Inject]
        private void Construct(IObjectPooler<IBullet> pooler)
        {
            _BulletPooler = pooler;
        }

        private void Update()
        {
            MoveForward();
        }

        private void MoveForward()
        {
            transform.position += transform.up * _Speed * Time.deltaTime;
        }

        public void SetupBullet(Vector3 pos, Quaternion rot, float speed, float timeToLive, EnemyTypes originType)
        {
            transform.position = pos;
            transform.rotation = rot;

            _Speed = speed;
            _Origin = originType;

            gameObject.SetActive(true);

            Invoke(nameof(OnHit), timeToLive);
        }

        public void OnHit()
        {
            if (IsInvoking(nameof(OnHit)))
            {
                CancelInvoke(nameof(OnHit));
            }

            _BulletPooler.AddObjectToAvailable(this);
            this.gameObject.SetActive(false);
        }

        public class Factory : PlaceholderFactory<IBullet>
        {

        }
    }
}