using UnityEngine;
using ProjectRimba.Player;
using ProjectRimba.Systems;

namespace ProjectRimba.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class AnimalCage : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHp = 50f;
        private float _currentHp;

        private void Awake()
        {
            _currentHp = maxHp;
        }

        public void TakeDamage(float amount, Vector2 hitNormal)
        {
            _currentHp -= amount;
            Debug.Log($"[AnimalCage] Kandang retak! Sisa HP: {Mathf.Max(0f, _currentHp):F0}");

            if (_currentHp <= 0f)
            {
                var player = Object.FindAnyObjectByType<OrangutanController>();
                if (player != null)
                {
                    player.UnlockPythonSkill();
                }
                Debug.Log("[AnimalCage] Ular Piton dibebaskan! Jangkauan Grapple 2x!");
                Destroy(gameObject);
            }
        }
    }
}
