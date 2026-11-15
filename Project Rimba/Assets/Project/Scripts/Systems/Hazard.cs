using UnityEngine;
using ProjectRimba.Systems;

namespace ProjectRimba.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private float knockbackForce = 10f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDamage(collision.collider, collision.GetContact(0).normal);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Vector2 normal = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
            TryDamage(other, normal);
        }

        private void TryDamage(Collider2D col, Vector2 normal)
        {
            if (col.gameObject.layer != LayerMask.NameToLayer("PlayerBody")) return;

            if (col.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(1f, normal);
            }

            if (col.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.up * knockbackForce;
            }
        }
    }
}
