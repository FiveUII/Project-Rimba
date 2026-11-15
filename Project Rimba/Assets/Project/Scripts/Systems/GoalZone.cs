using System;
using UnityEngine;

namespace ProjectRimba.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class GoalZone : MonoBehaviour
    {
        public event Action OnGoalReached;
        private bool _triggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggered) return;

            int layer = other.gameObject.layer;
            if (layer == LayerMask.NameToLayer("PlayerBody") || layer == LayerMask.NameToLayer("PlayerArm"))
            {
                _triggered = true;
                Debug.Log("[GoalZone] Stage Clear!");
                OnGoalReached?.Invoke();
            }
        }
    }
}
