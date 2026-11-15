using UnityEngine;

namespace ProjectRimba.Systems
{
    public interface IDamageable
    {
        void TakeDamage(float amount, Vector2 hitNormal);
    }
}
