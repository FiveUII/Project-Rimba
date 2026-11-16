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

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _player = GetComponentInParent<OrangutanController>();
            _playerHealth = GetComponentInParent<PlayerHealth>();
            _playerAdrenaline = GetComponentInParent<PlayerAdrenaline>();
            _playerRb = GetComponentInParent<Rigidbody2D>();
            _lumberjack = GetComponentInParent<AxeLumberjack>();
        }

        private void OnEnable()
        {
            if (_player != null) _player.OnAttack += HandlePlayerAttack;
            if (_playerHealth != null) _playerHealth.OnDamaged += HandleHurt;
            if (_lumberjack != null)
            {
                _lumberjack.OnAttack += HandleAttack;
                _lumberjack.OnHurt += HandleHurt;
            }
        }

        private void OnDisable()
        {
            if (_player != null) _player.OnAttack -= HandlePlayerAttack;
            if (_playerHealth != null) _playerHealth.OnDamaged -= HandleHurt;
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
    }
}
