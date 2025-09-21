using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Config/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement Speeds")]
        public float walkSpeed = 4.2f;
        public float sprintMultiplier = 1.35f;

        [Header("Character Physics")]
        public float rotationSharpness = 12f;
        public float gravity = -20f;
        public float groundedGravity = -2f;

        [Header("Stats")]
        public float baseDefense = 5f;
    }
}
