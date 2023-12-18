using System;

using AsteroidsGame.Input;

using UnityEngine;

using Zenject;

namespace AsteroidsGame.PlayerShip
{
    public class PlayerWarpDrive : ITickable
    {
        private float _CooldownRemaining = 0f;

        private bool CanWarp { get { return _CooldownRemaining <= 0f && !_PlayerVisibility.IsDisabled; } }

        readonly Settings _Settings;
        readonly IInputManager _InputManager;
        readonly IPlayerVisibility _PlayerVisibility;
        readonly Camera _Camera;
        readonly Transform _PlayerTransform;

        public PlayerWarpDrive(Settings settings,
            IInputManager input,
            IPlayerVisibility playerVisibility,
            Camera camera,
            Transform playerTransform)
        {
            _Settings = settings;
            _InputManager = input;
            _PlayerVisibility = playerVisibility;
            _Camera = camera;
            _PlayerTransform = playerTransform;
        }

        public void Tick()
        {
            TryWarp();
            UpdateCooldown();
        }

        private void TryWarp()
        {
            if (_InputManager.GetWarpDriveButton())
            {
                if (CanWarp)
                {
                    Warp();
                    _CooldownRemaining = _Settings.WarpCooldown;
                }
            }
        }

        private void Warp()
        {
            Vector3 _Target = _Camera.ScreenToWorldPoint(_InputManager.GetAimingPosition());
            _Target.z = 0;

            _PlayerTransform.position = _Target;
        }

        private void UpdateCooldown()
        {
            if (!CanWarp)
            {
                _CooldownRemaining -= Time.deltaTime;
            }
        }

        [Serializable]
        public class Settings
        {
            public float WarpCooldown;
        }
    }
}