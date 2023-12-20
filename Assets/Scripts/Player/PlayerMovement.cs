using System;

using AsteroidsGame.InputHandler;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.PlayerShip
{
    public class PlayerMovement : ITickable, IFixedTickable
    {
        readonly Settings _Settings;
        readonly IInputManager _InputManager;
        readonly IPlayerVisibility _PlayerVisibility;
        readonly Camera _Camera;
        readonly Rigidbody2D _RBody;
        readonly Transform _PlayerTransform;

        private PlayerMovement(Settings settings,
            IInputManager input,
            IPlayerVisibility playerVisibility,
            Camera camera,
            Rigidbody2D rbody,
            Transform playerTransform)
        {
            _Settings = settings;
            _InputManager = input;
            _PlayerVisibility = playerVisibility;
            _Camera = camera;
            _RBody = rbody;
            _PlayerTransform = playerTransform;
        }

        public void Tick()
        {
            Rotate();
        }

        public void FixedTick()
        {
            Move();
            CapSpeed();
        }

        public void Rotate()
        {
            if (_PlayerVisibility.IsDisabled)
            {
                return;
            };

            Vector3 targetPos = _Camera.ScreenToWorldPoint(_InputManager.GetAimingPosition());
            targetPos.z = 0f;

            if (Vector3.Distance(_PlayerTransform.position, targetPos) > _Settings.RotationDeadzone)
            {
                _PlayerTransform.rotation = Quaternion.LookRotation(_PlayerTransform.forward, targetPos - _PlayerTransform.position);
            }
        }

        public void Move()
        {
            if (_PlayerVisibility.IsDisabled)
            {
                return;
            };

            if (_InputManager.GetThrottle() > _Settings.ThrottleDeadzone)
            {
                _RBody.AddForce(_PlayerTransform.up * _Settings.Acceleration * _InputManager.GetThrottle());
            }
            else
            {
                _RBody.velocity -= _RBody.velocity.normalized * _Settings.Deceleration * Time.deltaTime;
            }
        }

        public void CapSpeed()
        {
            if (_PlayerVisibility.IsDisabled)
            {
                _RBody.velocity = Vector3.zero;
                return;
            }

            float sqrMaxSpeed = _Settings.MaxSpeed * _Settings.MaxSpeed;
            float sqrMinSpeed = _Settings.MinSpeed * _Settings.MinSpeed;

            if (_RBody.velocity.sqrMagnitude > sqrMaxSpeed)
            {
                _RBody.velocity = _RBody.velocity.normalized * _Settings.MaxSpeed;
            }
            else if (_RBody.velocity.sqrMagnitude < sqrMinSpeed)
            {
                _RBody.velocity = _RBody.velocity.normalized * _Settings.MinSpeed;
            }
        }

        public void ApplyMaxSpeed()
        {
            _RBody.velocity = _PlayerTransform.up * _Settings.MaxSpeed;
        }

        [Serializable]
        public class Settings
        {
            public float MaxSpeed;
            public float MinSpeed;
            public float Acceleration;
            public float Deceleration;
            public float ThrottleDeadzone;
            public float RotationDeadzone;
        }
    }
}