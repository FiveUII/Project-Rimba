using UnityEngine;
using ProjectRimba.Player;
using ProjectRimba.Systems;

namespace ProjectRimba.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class AnimalCage : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHp = 50f;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private Sprite crackedSprite;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private GameObject pythonVisual;

        private float _currentHp;
        private bool _opened;
        private SpriteRenderer _sr;
        private Collider2D _col;

        private void Awake()
        {
            _currentHp = maxHp;
            _sr = GetComponent<SpriteRenderer>();
            _col = GetComponent<Collider2D>();
            if (_sr != null && closedSprite != null) _sr.sprite = closedSprite;
            if (pythonVisual != null) pythonVisual.SetActive(false);
        }

        public void TakeDamage(float amount, Vector2 hitNormal)
        {
            if (_opened) return;

            _currentHp -= amount;
            Debug.Log($"[AnimalCage] Kandang retak! Sisa HP: {Mathf.Max(0f, _currentHp):F0}");

            if (_currentHp > 0f)
            {
                if (_sr != null && crackedSprite != null) _sr.sprite = crackedSprite;
                return;
            }

            _opened = true;
            if (_col != null) _col.enabled = false;
            if (_sr != null && openSprite != null) _sr.sprite = openSprite;
            if (pythonVisual != null) pythonVisual.SetActive(true);

            var player = Object.FindAnyObjectByType<OrangutanController>();
            if (player != null)
            {
                player.UnlockPythonSkill();
            }
            Debug.Log("[AnimalCage] Ular Piton dibebaskan! Jangkauan Grapple 2x!");
        }
    }
}
