using UnityEngine;

namespace Assets.Scripts
{
    public class BackgroundController : MonoBehaviour
    {
        public float scrollSpeed;
        public float tileSize;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSize);
            transform.position = startPosition + transform.up * newPosition;
        }
    }
}