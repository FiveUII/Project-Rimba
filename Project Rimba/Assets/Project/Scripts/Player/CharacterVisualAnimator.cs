using UnityEngine;
using ProjectRimba.Enemy;

namespace ProjectRimba.Player
{
    /// <summary>
    /// Drives Animator parameters on a child Visual object for either OrangutanController or AxeLumberjack.
    /// Keeps animation transforms isolated from root physics colliders.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CharacterVisualAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int IsGrapplingHash = Animator.StringToHash("IsGrappling");
        private static readonly int IsWindingUpHash = Animator.StringToHash("IsWindingUp");
        private static readonly int IsHealingHash = Animator.StringToHash("IsHealing");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int AttackUpHash = Animator.StringToHash("AttackUp");
        private static readonly int AttackDownHash = Animator.StringToHash("AttackDown");
        private static readonly int HurtHash = Animator.StringToHash("Hurt");

        private Animator _animator;
        private OrangutanController _player;
        private PlayerHealth _playerHealth;
        private PlayerAdrenaline _playerAdrenaline;
        private Rigidbody2D _playerRb;
        private AxeLumberjack _lumberjack;
        private SpriteRenderer _sprite;
        private SpriteRenderer _healGlowSr;
        private float _healGlowTimer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _player = GetComponentInParent<OrangutanController>();
            _playerHealth = GetComponentInParent<PlayerHealth>();
            _playerAdrenaline = GetComponentInParent<PlayerAdrenaline>();
            _playerRb = GetComponentInParent<Rigidbody2D>();
            _lumberjack = GetComponentInParent<AxeLumberjack>();

            if (_player != null && _sprite != null)
            {
                var glowGo = new GameObject("HealGlow_Visual");
                glowGo.transform.SetParent(transform, false);
                _healGlowSr = glowGo.AddComponent<SpriteRenderer>();
                _healGlowSr.sortingOrder = _sprite.sortingOrder - 1;
                _healGlowSr.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_player != null) _player.OnAttack += HandlePlayerAttack;
            if (_playerHealth != null)
            {
                _playerHealth.OnDamaged += HandleHurt;
                _playerHealth.OnHealed += HandleHealed;
            }
            if (_lumberjack != null)
            {
                _lumberjack.OnAttack += HandleAttack;
                _lumberjack.OnHurt += HandleHurt;
            }
        }

        private void OnDisable()
        {
            if (_player != null) _player.OnAttack -= HandlePlayerAttack;
            if (_playerHealth != null)
            {
                _playerHealth.OnDamaged -= HandleHurt;
                _playerHealth.OnHealed -= HandleHealed;
            }
            if (_lumberjack != null)
            {
                _lumberjack.OnAttack -= HandleAttack;
                _lumberjack.OnHurt -= HandleHurt;
            }
        }

        private void Update()
        {
            if (_player != null && _playerRb != null)
            {
                _animator.SetFloat(SpeedHash, Mathf.Abs(_playerRb.linearVelocity.x));
                _animator.SetBool(IsGroundedHash, _player.IsGrounded);
                _animator.SetBool(IsGrapplingHash, _player.IsGrappling);
                _animator.SetBool(IsHealingHash, _playerAdrenaline != null && _playerAdrenaline.IsHealing);
                UpdateHealGlow();
            }
            else if (_lumberjack != null)
            {
                _animator.SetFloat(SpeedHash, _lumberjack.IsMoving ? 1f : 0f);
                _animator.SetBool(IsWindingUpHash, _lumberjack.IsWindingUp);
                if (_sprite != null)
                {
                    // spr_lumberjack faces left by default; flipX when patrolling right.
                    _sprite.flipX = _lumberjack.PatrolDir > 0;
                }
            }
        }

        private void HandlePlayerAttack(Vector2 dir)
        {
            if (dir.y > 0.5f) _animator.SetTrigger(AttackUpHash);
            else if (dir.y < -0.5f) _animator.SetTrigger(AttackDownHash);
            else _animator.SetTrigger(AttackHash);
        }

        private void HandleAttack()
        {
            _animator.SetTrigger(AttackHash);
        }

        private void HandleHurt()
        {
            _animator.SetTrigger(HurtHash);
        }

        private void HandleHealed()
        {
            _healGlowTimer = 0.6f;
        }

        private void UpdateHealGlow()
        {
            if (_healGlowTimer <= 0f) return;

            _healGlowTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(_healGlowTimer / 0.6f);
            if (_sprite != null)
            {
                _sprite.color = Color.Lerp(Color.white, new Color(0.55f, 1f, 0.6f, 1f), t * 0.85f);
            }
            if (_healGlowSr != null && _sprite != null)
            {
                _healGlowSr.enabled = t > 0f;
                _healGlowSr.sprite = _sprite.sprite;
                _healGlowSr.flipX = _sprite.flipX;
                _healGlowSr.transform.localScale = _sprite.transform.localScale * (1.14f + (1f - t) * 0.22f);
                _healGlowSr.color = new Color(0.3f, 1f, 0.45f, t * 0.78f);
            }
        }
    }
}
