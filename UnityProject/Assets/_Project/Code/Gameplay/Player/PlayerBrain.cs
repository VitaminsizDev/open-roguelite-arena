using UnityEngine;
using Game.Systems.Input;
using Game.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerBrain : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputDriver input;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private PlayerConfig playerConfig;

        [Header("Movement Overrides")]
        [SerializeField] private float moveSpeedOverride = 0f;
        [SerializeField] private float sprintSpeedOverride = 0f;

        private CharacterController _controller;
        private float _verticalVelocity;

        public Vector2 MoveAxes { get; private set; }
        public Vector3 PlanarVelocity { get; private set; }
        public Vector3 WorldMoveDirection { get; private set; } = Vector3.forward;
        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }
        public float NormalizedSpeed { get; private set; }
        public float CurrentSpeed { get; private set; }

        private float MoveSpeed => moveSpeedOverride > 0f
            ? moveSpeedOverride
            : (playerConfig ? playerConfig.walkSpeed : 4.5f);

        private float SprintSpeed => sprintSpeedOverride > 0f
            ? sprintSpeedOverride
            : (playerConfig ? playerConfig.walkSpeed * playerConfig.sprintMultiplier : MoveSpeed);

        private float Gravity => playerConfig ? playerConfig.gravity : -20f;
        private float GroundedGravity => playerConfig ? playerConfig.groundedGravity : -2f;
        private float RotationSharpness => playerConfig ? playerConfig.rotationSharpness : 12f;

        private void Reset()
        {
            input = GetComponent<PlayerInputDriver>();
            if (!cameraTransform && Camera.main) cameraTransform = Camera.main.transform;
            playerConfig = FindDefaultConfig();
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (!input) input = GetComponent<PlayerInputDriver>();
            if (!cameraTransform && Camera.main) cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            TickMovement(Time.deltaTime);
        }

        public void InjectCamera(Transform t)
        {
            cameraTransform = t;
        }

        private void TickMovement(float dt)
        {
            Vector2 rawInput = input ? input.Move.dir : Vector2.zero;
            rawInput = Vector2.ClampMagnitude(rawInput, 1f);
            MoveAxes = rawInput;

            bool sprintRequested = input && input.Move.sprint;
            IsSprinting = sprintRequested && rawInput.sqrMagnitude > 0.0001f;

            if (!cameraTransform && Camera.main)
            {
                cameraTransform = Camera.main.transform;
            }

            Vector3 camForward = cameraTransform ? cameraTransform.forward : transform.forward;
            Vector3 camRight = cameraTransform ? cameraTransform.right : transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            if (camForward.sqrMagnitude < 0.0001f) camForward = transform.forward;
            if (camRight.sqrMagnitude < 0.0001f) camRight = transform.right;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 desiredDir = camForward * rawInput.y + camRight * rawInput.x;
            if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

            float targetSpeed = IsSprinting ? SprintSpeed : MoveSpeed;
            PlanarVelocity = desiredDir * targetSpeed * rawInput.magnitude;
            CurrentSpeed = PlanarVelocity.magnitude;

            WorldMoveDirection = PlanarVelocity.sqrMagnitude > 0.0001f
                ? PlanarVelocity.normalized
                : camForward;

            if (rawInput.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(camForward, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, RotationSharpness * dt);
            }

            if (_controller.isGrounded)
            {
                IsGrounded = true;
                _verticalVelocity = GroundedGravity;
            }
            else
            {
                IsGrounded = false;
                _verticalVelocity += Gravity * dt;
            }

            Vector3 motion = PlanarVelocity + Vector3.up * _verticalVelocity;
            _controller.Move(motion * dt);

            float baseSpeed = Mathf.Max(MoveSpeed, 0.01f);
            NormalizedSpeed = Mathf.Max(0f, CurrentSpeed / baseSpeed);
        }

        private PlayerConfig FindDefaultConfig()
        {
            #if UNITY_EDITOR
            if (playerConfig) return playerConfig;
            string[] guids = AssetDatabase.FindAssets("t:PlayerConfig");
            if (guids != null && guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<PlayerConfig>(path);
            }
            #endif
            return playerConfig;
        }
    }
}
