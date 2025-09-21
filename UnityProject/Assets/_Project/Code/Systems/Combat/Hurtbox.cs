using UnityEngine;

namespace Game.Systems.Combat
{
    public class Hurtbox : MonoBehaviour
    {
        public Transform center;
        public float radius = 0.5f;

        private ulong _lastHitId;

        public bool IsHitID(ulong id) => _lastHitId == id;
        public void MarkHit(ulong id) { _lastHitId = id; }
    }
}
