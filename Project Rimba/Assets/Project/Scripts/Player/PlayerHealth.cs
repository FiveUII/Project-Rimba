using System;
using UnityEngine;
using ProjectRimba.Systems;

namespace ProjectRimba.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxSegments = 5;
        [SerializeField] private float invincibilityDuration = 1.2f;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public int CurrentSegments { get; private set; }
        public int MaxSegments => maxSegments;

        private float _invincibleUntil;

        private void Awake()
        {
            CurrentSegments = maxSegments;
        }

        public void TakeDamage(float amount, Vector2 hitNormal)
        {
            if (CurrentSegments <= 0 || Time.time < _invincibleUntil) return;

            CurrentSegments = Mathf.Max(0, CurrentSegments - 1);
            _invincibleUntil = Time.time + invincibilityDuration;

            Debug.Log($"[PlayerHealth] Kena serangan! Sisa Daun: {CurrentSegments}/{maxSegments}");
            OnHealthChanged?.Invoke(CurrentSegments, maxSegments);

            if (CurrentSegments == 0)
            {
                Debug.Log("[PlayerHealth] Orang Utan tumbang!");
                OnDied?.Invoke();
            }
        }

        public void Heal(int segments = 1)
        {
            if (CurrentSegments <= 0 || CurrentSegments >= maxSegments) return;

            CurrentSegments = Mathf.Min(maxSegments, CurrentSegments + segments);
            Debug.Log($"[PlayerHealth] Heal! Sisa Daun: {CurrentSegments}/{maxSegments}");
            OnHealthChanged?.Invoke(CurrentSegments, maxSegments);
        }
    }
}
