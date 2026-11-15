using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectRimba.Player
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerAdrenaline : MonoBehaviour
    {
        [SerializeField] private OrangutanController playerController;
        [SerializeField] private float maxAdrenaline = 100f;
        [SerializeField] private float healCost = 40f;
        [SerializeField] private float healHoldDuration = 1.5f;

        public event Action<float, float> OnAdrenalineChanged;
        public event Action<float> OnHealProgress;

        public float CurrentAdrenaline { get; private set; }
        public float MaxAdrenaline => maxAdrenaline;

        private PlayerHealth _health;
        private float _healTimer;

        private void Awake()
        {
            _health = GetComponent<PlayerHealth>();
            CurrentAdrenaline = 0f;
        }

        private void Update()
        {
            bool healHeld = (Keyboard.current != null && Keyboard.current.eKey.isPressed)
                         || (Mouse.current != null && Mouse.current.middleButton.isPressed);

            bool canHeal = healHeld
                        && (playerController == null || playerController.IsGrounded || playerController.IsGrappling)
                        && CurrentAdrenaline >= healCost
                        && _health.CurrentSegments < _health.MaxSegments;

            if (!canHeal)
            {
                if (_healTimer > 0f)
                {
                    _healTimer = 0f;
                    OnHealProgress?.Invoke(0f);
                }
                return;
            }

            _healTimer += Time.deltaTime;
            OnHealProgress?.Invoke(Mathf.Clamp01(_healTimer / healHoldDuration));

            if (_healTimer >= healHoldDuration)
            {
                _healTimer = 0f;
                CurrentAdrenaline = Mathf.Max(0f, CurrentAdrenaline - healCost);
                _health.Heal(1);
                OnAdrenalineChanged?.Invoke(CurrentAdrenaline, maxAdrenaline);
                OnHealProgress?.Invoke(0f);
            }
        }

        public void AddAdrenaline(float amount)
        {
            if (amount <= 0f) return;

            CurrentAdrenaline = Mathf.Min(maxAdrenaline, CurrentAdrenaline + amount);
            OnAdrenalineChanged?.Invoke(CurrentAdrenaline, maxAdrenaline);
        }
    }
}
