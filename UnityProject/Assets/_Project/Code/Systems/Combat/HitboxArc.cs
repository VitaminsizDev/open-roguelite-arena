using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Combat
{
    public static class HitboxArc
    {
        private static readonly Collider[] _buf = new Collider[64];

        /// <summary>
        /// Finds hurtboxes within an arc sector.
        /// </summary>
        public static int Query(Vector3 origin, Vector3 facing, float reach, float coneDeg, int layerMask, List<Hurtbox> results)
        {
            results.Clear();
            int n = Physics.OverlapSphereNonAlloc(origin, reach, _buf, layerMask);
            float half = coneDeg * 0.5f;

            for (int i = 0; i < n; i++)
            {
                var hb = _buf[i].GetComponent<Hurtbox>();
                if (!hb) continue;

                Vector3 to = (hb.center ? hb.center.position : hb.transform.position) - origin;
                float dist = to.magnitude;
                if (dist > reach) continue;

                float ang = Vector3.Angle(facing, to.normalized);
                if (ang <= half) results.Add(hb);
            }
            return results.Count;
        }
    }
}
