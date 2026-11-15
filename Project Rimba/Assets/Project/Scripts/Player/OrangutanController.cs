using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectRimba.Systems;

namespace ProjectRimba.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(DistanceJoint2D))]
    public class OrangutanController : MonoBehaviour
    {
        [Header("Movement (Hollow Knight Feel)")]
        [SerializeField] private float walkSpeed = 7.5f;
        [SerializeField] private float groundAccel = 70f;
        [SerializeField] private float airAccel = 48f;
        [SerializeField] private float jumpVelocity = 15f;
        [SerializeField] private float jumpCutMultiplier = 0.5f;
        [SerializeField] private float coyoteTime = 0.12f;
        [SerializeField] private float jumpBufferTime = 0.12f;

        [Header("Dash (Left Shift)")]
        [SerializeField] private float dashSpeed = 19f;
        [SerializeField] private float dashDuration = 0.16f;
        [SerializeField] private float dashCooldown = 0.6f;

        [Header("Melee & Pogo (Left Click)")]
        [SerializeField] private float attackDamage = 25f;
        [SerializeField] private float attackRange = 1.6f;
        [SerializeField] private float attackRadius = 0.9f;
        [SerializeField] private float attackCooldown = 0.28f;
        [SerializeField] private float pogoVelocity = 14f;
        [SerializeField] private float adrenalinePerHit = 20f;
        [SerializeField] private LayerMask enemyLayers;
        [SerializeField] private LayerMask pogoableLayers;

        [Header("Grapple Swing (Right Click)")]
        [SerializeField] private float grappleRange = 3.8f;
        [SerializeField] private float pythonGrappleMultiplier = 2f;
        [SerializeField] private float swingPumpForce = 45f;
        [SerializeField] private float releaseBoost = 1.15f;
        [SerializeField] private LayerMask grabbableLayers;

        [Header("Ground Check")]
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.95f);
        [SerializeField] private float groundCheckRadius = 0.25f;

        [Header("Visuals")]
        [SerializeField] private LineRenderer grappleLine;
        [SerializeField] private SpriteRenderer slashVisual;

        public event Action<bool> OnPythonUnlockedChanged;
        public event Action OnAttack;

        public bool IsGrappling { get; private set; }
        public bool IsNearGrabbable { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool HasPythonSkill { get; private set; }
        public Collider2D CurrentGrappledCollider { get; private set; }

        private float EffectiveGrappleRange => HasPythonSkill ? grappleRange * pythonGrappleMultiplier : grappleRange;

        public void UnlockPythonSkill()
        {
            if (HasPythonSkill) return;
            HasPythonSkill = true;
            if (grappleLine != null)
            {
                grappleLine.startColor = new Color(0.25f, 0.85f, 0.4f, 1f);
                grappleLine.endColor = new Color(0.25f, 0.85f, 0.4f, 1f);
            }
            OnPythonUnlockedChanged?.Invoke(true);
        }

        private Rigidbody2D _rb;
        private DistanceJoint2D _grappleJoint;
        private PlayerAdrenaline _adrenaline;
        private Camera _mainCamera;
        private CameraFollow2D _cameraFollow;

        private readonly List<Collider2D> _hitResults = new List<Collider2D>(8);
        private ContactFilter2D _attackFilter;

        private float _moveInput;
        private float _facingDir = 1f;
        private float _defaultGravity;
        private float _coyoteCounter;
        private float _jumpBufferCounter;
        private float _dashTimer;
        private float _nextDashTime;
        private float _nextAttackTime;
        private float _slashTimer;
        private float _hitStopEndTime;
        private bool _inHitStop;
        private Vector2 _mouseWorldPos;
        private Vector2 _grapplePoint;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _grappleJoint = GetComponent<DistanceJoint2D>();
            TryGetComponent(out _adrenaline);
            _mainCamera = Camera.main;
            if (_mainCamera != null) _mainCamera.TryGetComponent(out _cameraFollow);

            if (grappleLine == null) TryGetComponent(out grappleLine);
            if (grappleLine != null) grappleLine.enabled = false;
            if (slashVisual != null) slashVisual.enabled = false;

            _defaultGravity = _rb.gravityScale;
            _grappleJoint.enabled = false;
            _grappleJoint.autoConfigureDistance = false;
            _grappleJoint.maxDistanceOnly = true;

            _attackFilter = new ContactFilter2D { useTriggers = true, useLayerMask = true };
        }

        private void Update()
        {
            if (_inHitStop && Time.unscaledTime >= _hitStopEndTime)
            {
                _inHitStop = false;
                if (Time.timeScale > 0f) Time.timeScale = 1f;
            }

            if (_slashTimer > 0f)
            {
                _slashTimer -= Time.deltaTime;
                if (_slashTimer <= 0f && slashVisual != null) slashVisual.enabled = false;
            }

            if (Keyboard.current == null || Mouse.current == null) return;

            if (_mainCamera != null)
            {
                Vector2 screenPos = Mouse.current.position.ReadValue();
                float camDepth = Mathf.Abs(_mainCamera.transform.position.z);
                _mouseWorldPos = _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, camDepth));
            }

            float input = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input += 1f;
            _moveInput = input;

            if (Mathf.Abs(_moveInput) > 0.01f)
            {
                _facingDir = Mathf.Sign(_moveInput);
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * _facingDir;
                transform.localScale = scale;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _jumpBufferCounter = jumpBufferTime;
            }
            else if (_jumpBufferCounter > 0f)
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            if (Keyboard.current.spaceKey.wasReleasedThisFrame && _rb.linearVelocity.y > 0f && !IsGrappling)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * jumpCutMultiplier);
            }

            if (Keyboard.current.leftShiftKey.wasPressedThisFrame && Time.time >= _nextDashTime && _dashTimer <= 0f)
            {
                StartDash();
            }

            if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= _nextAttackTime)
            {
                PerformAttack();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                TryStartGrapple();
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame && IsGrappling)
            {
                StopGrapple(true);
            }

            IsNearGrabbable = !IsGrappling && FindBestBranch() != null;
        }

        private void FixedUpdate()
        {
            IsGrounded = Physics2D.OverlapCircle(_rb.position + groundCheckOffset, groundCheckRadius, groundLayers) != null;
            if (IsGrounded) _coyoteCounter = coyoteTime;
            else if (_coyoteCounter > 0f) _coyoteCounter -= Time.fixedDeltaTime;

            if (_dashTimer > 0f)
            {
                _dashTimer -= Time.fixedDeltaTime;
                if (_dashTimer <= 0f)
                {
                    _rb.gravityScale = _defaultGravity;
                }
                return;
            }

            if (IsGrappling)
            {
                int wallMask = groundLayers.value & ~grabbableLayers.value;
                if (Physics2D.Linecast(_rb.position, _grapplePoint, wallMask))
                {
                    StopGrapple(false);
                    return;
                }

                if (Mathf.Abs(_moveInput) > 0.01f)
                {
                    _rb.AddForce(Vector2.right * (_moveInput * swingPumpForce));
                }
                _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, 16f);
                return;
            }

            Vector2 vel = _rb.linearVelocity;
            float accel = IsGrounded ? groundAccel : airAccel;
            vel.x = Mathf.MoveTowards(vel.x, _moveInput * walkSpeed, accel * Time.fixedDeltaTime);

            if (_jumpBufferCounter > 0f && _coyoteCounter > 0f)
            {
                vel.y = jumpVelocity;
                _jumpBufferCounter = 0f;
                _coyoteCounter = 0f;
            }

            _rb.linearVelocity = vel;
        }

        private void LateUpdate()
        {
            if (grappleLine == null) return;

            grappleLine.enabled = IsGrappling;
            if (IsGrappling)
            {
                grappleLine.SetPosition(0, _rb.position);
                grappleLine.SetPosition(1, _grapplePoint);
            }
        }

        private void StartDash()
        {
            if (IsGrappling) StopGrapple(false);

            _dashTimer = dashDuration;
            _nextDashTime = Time.time + dashCooldown;
            _rb.gravityScale = 0f;
            _rb.linearVelocity = new Vector2(_facingDir * dashSpeed, 0f);
        }

        private void PerformAttack()
        {
            _nextAttackTime = Time.time + attackCooldown;
            OnAttack?.Invoke();

            Vector2 toMouse = _mouseWorldPos - _rb.position;
            Vector2 attackDir;
            if (Mathf.Abs(toMouse.y) > Mathf.Abs(toMouse.x))
            {
                attackDir = toMouse.y >= 0f ? Vector2.up : Vector2.down;
            }
            else
            {
                attackDir = toMouse.x >= 0f ? Vector2.right : Vector2.left;
            }

            Vector2 hitOrigin = _rb.position + attackDir * attackRange;
            ShowSlashVisual(hitOrigin, attackDir);

            _attackFilter.layerMask = enemyLayers | pogoableLayers;
            int count = Physics2D.OverlapCircle(hitOrigin, attackRadius, _attackFilter, _hitResults);
            bool hitEnemy = false;
            bool pogoTriggered = false;

            for (int i = 0; i < count; i++)
            {
                Collider2D col = _hitResults[i];
                if (col == null) continue;

                int layerBit = 1 << col.gameObject.layer;
                if ((enemyLayers.value & layerBit) != 0 && col.TryGetComponent(out IDamageable target))
                {
                    target.TakeDamage(attackDamage, -attackDir);
                    hitEnemy = true;
                }

                if (attackDir == Vector2.down && !IsGrounded && ((enemyLayers.value & layerBit) != 0 || col.TryGetComponent<Hazard>(out _)))
                {
                    pogoTriggered = true;
                }
            }

            if (hitEnemy)
            {
                if (_adrenaline != null) _adrenaline.AddAdrenaline(adrenalinePerHit);
                TriggerFeedback(0.18f, 0.1f, 0.04f);
            }

            if (pogoTriggered)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, pogoVelocity);
                _nextDashTime = Time.time; // Refresh dash on pogo like Hollow Knight
                if (!hitEnemy) TriggerFeedback(0.1f, 0.06f, 0f);
            }
        }

        private void ShowSlashVisual(Vector2 origin, Vector2 dir)
        {
            if (slashVisual == null) return;
            slashVisual.transform.position = origin;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            slashVisual.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            slashVisual.enabled = true;
            _slashTimer = 0.09f;
        }

        private Collider2D FindBestBranch()
        {
            _attackFilter.layerMask = grabbableLayers;
            int count = Physics2D.OverlapCircle(_rb.position, EffectiveGrappleRange, _attackFilter, _hitResults);
            int wallMask = groundLayers.value & ~grabbableLayers.value;
            Collider2D best = null;
            float bestScore = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider2D col = _hitResults[i];
                if (col == null) continue;
                if (Physics2D.Linecast(_rb.position, col.transform.position, wallMask)) continue;

                float distToMouse = Vector2.Distance(col.transform.position, _mouseWorldPos);
                if (distToMouse < bestScore)
                {
                    bestScore = distToMouse;
                    best = col;
                }
            }
            return best;
        }

        private void TryStartGrapple()
        {
            Collider2D branch = FindBestBranch();
            if (branch == null) return;

            if (_dashTimer > 0f)
            {
                _dashTimer = 0f;
                _rb.gravityScale = _defaultGravity;
            }

            IsGrappling = true;
            CurrentGrappledCollider = branch;
            _grapplePoint = branch.transform.position;
            _grappleJoint.connectedAnchor = _grapplePoint;
            _grappleJoint.distance = Mathf.Clamp(Vector2.Distance(_rb.position, _grapplePoint), 1.5f, EffectiveGrappleRange);
            _grappleJoint.enabled = true;
        }

        public void ForceStopGrapple()
        {
            if (IsGrappling) StopGrapple(false);
        }

        private void StopGrapple(bool applyBoost)
        {
            IsGrappling = false;
            CurrentGrappledCollider = null;
            _grappleJoint.enabled = false;

            if (applyBoost && _rb.linearVelocity.sqrMagnitude > 4f)
            {
                Vector2 boosted = _rb.linearVelocity * releaseBoost;
                boosted.y += 2f;
                _rb.linearVelocity = Vector2.ClampMagnitude(boosted, 20f);
            }
        }

        private void TriggerFeedback(float shakeIntensity, float shakeDuration, float freezeDuration)
        {
            if (_cameraFollow != null) _cameraFollow.Shake(shakeIntensity, shakeDuration);

            if (freezeDuration > 0f && Time.timeScale > 0.1f)
            {
                _inHitStop = true;
                _hitStopEndTime = Time.unscaledTime + freezeDuration;
                Time.timeScale = 0.05f;
            }
        }
    }
}
