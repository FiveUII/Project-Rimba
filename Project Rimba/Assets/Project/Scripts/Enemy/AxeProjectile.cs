using UnityEngine;
using ProjectRimba.Player;
using ProjectRimba.Systems;

namespace ProjectRimba.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class AxeProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 8.5f;
        [SerializeField] private float spinSpeed = 720f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float lifetime = 3.5f;

        private Vector2 _dir = Vector2.left;
        private float _timer;
        private int _groundLayer;

        private void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
        }

        public void Initialize(Vector2 direction)
        {
            _dir = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.left;
            _timer = lifetime;
        }

        private void Update()
        {
            transform.Translate(_dir * (speed * Time.deltaTime), Space.World);
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<OrangutanController>(out _) && other.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage, _dir);
                Destroy(gameObject);
                return;
            }

            if (other.gameObject.layer == _groundLayer)
            {
                Destroy(gameObject);
            }
        }
    }
}
