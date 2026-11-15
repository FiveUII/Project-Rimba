using UnityEngine;

namespace ProjectRimba.Systems
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -10f);
        [SerializeField] private float smoothTime = 0.15f;

        private Vector3 _velocity;
        private float _shakeTimer;
        private float _shakeIntensity;

        public void Shake(float intensity = 0.18f, float duration = 0.12f)
        {
            _shakeIntensity = Mathf.Max(_shakeIntensity, intensity);
            _shakeTimer = Mathf.Max(_shakeTimer, duration);
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.position + offset;
            Vector3 smoothed = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);

            if (_shakeTimer > 0f)
            {
                _shakeTimer -= Time.unscaledDeltaTime;
                Vector2 jitter = Random.insideUnitCircle * _shakeIntensity;
                smoothed.x += jitter.x;
                smoothed.y += jitter.y;
                if (_shakeTimer <= 0f) _shakeIntensity = 0f;
            }

            transform.position = smoothed;
        }
    }
}
