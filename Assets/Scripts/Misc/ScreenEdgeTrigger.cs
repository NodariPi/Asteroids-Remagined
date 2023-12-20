using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace AsteroidsGame
{
    public class ScreenEdgeTrigger : MonoBehaviour
    {
        private static List<GameObject> _BlacklistedObjs = new List<GameObject>();

        [SerializeField] private float _BlacklistDuration = 0.2f;

        private Camera _Camera;

        public void Setup(Camera camera)
        {
            _Camera = camera;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_BlacklistedObjs.Contains(other.gameObject))
                return;

            WrapObject(other.gameObject);
        }

        void WrapObject(GameObject obj)
        {
            if (!this.gameObject.activeInHierarchy || _Camera == null)
                return;

            if (obj != null)
            {
                Vector3 objPosition = obj.transform.position;
                Vector3 viewportPosition = _Camera.WorldToViewportPoint(objPosition);

                if (IsOutsideViewport(viewportPosition))
                {
                    _BlacklistedObjs.Add(obj);
                    StartCoroutine(UpdateBlacklist(obj));

                    Vector3 newPosition = objPosition;

                    if (viewportPosition.x > 1 || viewportPosition.x < 0)
                    {
                        newPosition.x = -newPosition.x;
                    }

                    if (viewportPosition.y > 1 || viewportPosition.y < 0)
                    {
                        newPosition.y = -newPosition.y;
                    }

                    obj.transform.position = newPosition;
                }
            }
        }

        bool IsOutsideViewport(Vector3 viewportPosition)
        {
            return (viewportPosition.x > 1 || viewportPosition.x < 0 || viewportPosition.y > 1 || viewportPosition.y < 0);
        }

        IEnumerator UpdateBlacklist(GameObject obj)
        {
            yield return new WaitForSeconds(_BlacklistDuration);
            _BlacklistedObjs.Remove(obj);
        }
    }
}