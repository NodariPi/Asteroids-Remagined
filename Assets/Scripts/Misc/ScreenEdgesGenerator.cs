using System;

using UnityEngine;

using Zenject;

namespace AsteroidsGame
{
    public class ScreenEdgesGenerator : IInitializable
    {
        readonly Settings _Settings;
        readonly Camera _Camera;

        public ScreenEdgesGenerator(Settings settings,
            Camera camera)
        {
            _Settings = settings;
            _Camera = camera;
        }

        public void Initialize()
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 screenToWorld = Camera.main.ScreenToWorldPoint(screenSize);

            CreateCollider(new Vector2(screenToWorld.x + _Settings.DistanceFromViewport, 0), new Vector2(_Settings.ColliderWidth, screenToWorld.y * _Settings.SizeMultiplier));
            CreateCollider(new Vector2(-screenToWorld.x - _Settings.DistanceFromViewport, 0), new Vector2(_Settings.ColliderWidth, screenToWorld.y * _Settings.SizeMultiplier));
            CreateCollider(new Vector2(0, screenToWorld.y + _Settings.DistanceFromViewport), new Vector2(screenToWorld.x * _Settings.SizeMultiplier, _Settings.ColliderWidth));
            CreateCollider(new Vector2(0, -screenToWorld.y - _Settings.DistanceFromViewport), new Vector2(screenToWorld.x * _Settings.SizeMultiplier, _Settings.ColliderWidth));
        }

        private void CreateCollider(Vector2 position, Vector2 size)
        {
            GameObject colliderObject = new GameObject("ScreenEdgeCollider");
            colliderObject.layer = LayerMask.NameToLayer("Boundary");
            colliderObject.transform.position = position;

            BoxCollider2D collider = colliderObject.AddComponent<BoxCollider2D>();
            collider.size = size;
            collider.isTrigger = true;

            colliderObject.AddComponent<ScreenEdgeTrigger>().Setup(_Camera);

            Rigidbody2D rBody = colliderObject.AddComponent<Rigidbody2D>();
            rBody.isKinematic = true;
            rBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        [Serializable]
        public class Settings
        {
            public float ColliderWidth;
            public float SizeMultiplier;
            public float DistanceFromViewport;
        }
    }
}