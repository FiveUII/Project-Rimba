using UnityEngine;
using ProjectRimba.Systems;

namespace ProjectRimba.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class AxeLumberjack : MonoBehaviour, IDamageable
    {
        private enum State { Patrol, Windup, Cooldown }

        [Header("Stats")]
        [SerializeField] private float maxHp = 60f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float patrolRange = 3f;

        [Header("Combat")]
        [SerializeField] private Transform playerTorso;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float throwRange = 7.5f;
        [SerializeField] private float windupDuration = 0.6f;
        [SerializeField] private float attackCooldown = 1.2f;
        [SerializeField] private GameObject axeProjectilePrefab;

        public event System.Action OnAttack;
        public event System.Action OnHurt;
        public bool IsMoving => _state == State.Patrol;
        public bool IsWindingUp => _state == State.Windup;
        public int PatrolDir => _patrolDir;

        private SpriteRenderer _sprite;
        private Color _baseColor;
        private State _state = State.Patrol;
        private Vector2 _startPos;
        private float _currentHp;
        private float _stateTimer;
        private int _patrolDir = 1;

        private void Awake()
        {
            _currentHp = maxHp;
            _startPos = transform.position;
            _sprite = GetComponentInChildren<SpriteRenderer>();
            if (_sprite != null)
            {
                _baseColor = _sprite.color;
            }
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Patrol:
                    UpdatePatrol();
                    break;
                case State.Windup:
                    _stateTimer -= Time.deltaTime;
                    if (_stateTimer <= 0f)
                    {
                        ExecuteAttack();
                    }
                    break;
                case State.Cooldown:
                    _stateTimer -= Time.deltaTime;
                    if (_stateTimer <= 0f)
                    {
                        _state = State.Patrol;
                    }
                    break;
            }
        }

        private void UpdatePatrol()
        {
            transform.Translate(Vector2.right * (_patrolDir * moveSpeed * Time.deltaTime));
            if (Mathf.Abs(transform.position.x - _startPos.x) >= patrolRange)
            {
                _patrolDir *= -1;
            }

            float detectRange = axeProjectilePrefab != null ? throwRange : attackRange;
            if (playerTorso != null && Vector2.Distance(transform.position, playerTorso.position) <= detectRange)
            {
                _patrolDir = playerTorso.position.x >= transform.position.x ? 1 : -1;
                _state = State.Windup;
                _stateTimer = windupDuration;
                if (_sprite != null) _sprite.color = Color.yellow;
            }
        }

        private void ExecuteAttack()
        {
            if (_sprite != null) _sprite.color = _baseColor;
            OnAttack?.Invoke();

            if (playerTorso != null)
            {
                float dist = Vector2.Distance(transform.position, playerTorso.position);
                Vector2 dir = ((Vector2)playerTorso.position - (Vector2)transform.position).normalized;
                if (dist <= attackRange * 1.2f)
                {
                    if (playerTorso.TryGetComponent(out IDamageable target))
                    {
                        target.TakeDamage(1f, dir);
                    }
                }
                else if (axeProjectilePrefab != null && dist <= throwRange * 1.2f)
                {
                    var go = Instantiate(axeProjectilePrefab, transform.position + Vector3.up * 0.25f, Quaternion.identity);
                    if (go.TryGetComponent(out AxeProjectile proj))
                    {
                        proj.Initialize(dir);
                    }
                }
            }

            _state = State.Cooldown;
            _stateTimer = attackCooldown;
        }

        public void TakeDamage(float amount, Vector2 hitNormal)
        {
            _currentHp -= amount;
            Debug.Log($"[AxeLumberjack] Kena pukul {amount:F1} | Sisa HP: {Mathf.Max(0f, _currentHp):F1}");

            if (_currentHp <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            if (_state == State.Windup)
            {
                _state = State.Cooldown;
                _stateTimer = 0.5f;
                if (_sprite != null) _sprite.color = _baseColor;
            }
            OnHurt?.Invoke();
        }
    }
}
