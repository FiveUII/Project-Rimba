using UnityEngine;
using ProjectRimba.Systems;

namespace ProjectRimba.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class DummyTarget : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHp = 100f;
        private float _currentHp;

        private void Awake()
        {
            _currentHp = maxHp;
        }

        public void TakeDamage(float amount, Vector2 hitNormal)
        {
            _currentHp -= amount;
            Debug.Log($"[DummyTarget] Hit! Damage: {amount:F1} | Sisa HP: {Mathf.Max(0f, _currentHp):F1}");

            if (_currentHp <= 0f)
            {
                Debug.Log("[DummyTarget] Hancur! Reset HP untuk latihan.");
                _currentHp = maxHp;
            }
        }
    }
}
