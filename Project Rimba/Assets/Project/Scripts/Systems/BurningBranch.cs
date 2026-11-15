using UnityEngine;
using ProjectRimba.Player;

namespace ProjectRimba.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class BurningBranch : MonoBehaviour
    {
        [SerializeField] private float burnLimit = 1.5f;

        private SpriteRenderer _sprite;
        private Collider2D _col;
        private OrangutanController _player;
        private Color _baseColor;
        private float _burnTimer;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            if (TryGetComponent(out _sprite))
            {
                _baseColor = _sprite.color;
            }
            _player = Object.FindAnyObjectByType<OrangutanController>();
        }

        private void Update()
        {
            bool isGrappled = _player != null && _player.IsGrappling && _player.CurrentGrappledCollider == _col;

            if (!isGrappled)
            {
                if (_burnTimer > 0f)
                {
                    _burnTimer = Mathf.Max(0f, _burnTimer - Time.deltaTime);
                    UpdateVisual();
                }
                return;
            }

            _burnTimer += Time.deltaTime;
            UpdateVisual();

            if (_burnTimer >= burnLimit)
            {
                _burnTimer = 0f;
                _player.ForceStopGrapple();
                UpdateVisual();

                if (_player.TryGetComponent(out PlayerHealth health))
                {
                    health.TakeDamage(1f, Vector2.down);
                }
            }
        }

        private void UpdateVisual()
        {
            if (_sprite == null) return;
            _sprite.color = Color.Lerp(_baseColor, Color.red, Mathf.Clamp01(_burnTimer / burnLimit));
        }
    }
}
