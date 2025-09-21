using UnityEngine;

namespace Game.Data
{
    public enum WeaponClass { OneHand, TwoHand }

    [CreateAssetMenu(menuName = "Game/Config/AttackConfig")]
    public class AttackConfig : ScriptableObject
    {
        public WeaponClass weaponClass = WeaponClass.OneHand;
        public float damage = 10f;
        public float windup = 0.12f;
        public float active = 0.10f;
        public float recovery = 0.28f;
        [Range(30f, 180f)] public float coneDegrees = 100f;
        public float reach = 2.2f;

        [Header("Movement Modifiers During Attack")]
        [Tooltip("Speed multiplier during windup phase (0=locked, 1=normal)")]
        public float windupMoveMultiplier = 0.6f;
        [Tooltip("Speed multiplier during active phase (0=locked, 1=normal)")]
        public float activeMoveMultiplier = 0.0f;
        [Tooltip("Speed multiplier during recovery phase (0=locked, 1=normal)")]
        public float recoveryMoveMultiplier = 0.8f;

        [Tooltip("Lock facing during windup so movement input won't rotate character")]
        public bool lockFacingDuringWindup = true;
        [Tooltip("Lock facing during active so movement input won't rotate character")]
        public bool lockFacingDuringActive = true;
    }
}
