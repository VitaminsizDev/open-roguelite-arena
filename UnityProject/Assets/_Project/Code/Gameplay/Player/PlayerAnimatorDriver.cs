using UnityEngine;

namespace Game.Gameplay.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerAnimatorDriver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerBrain brain;
        [SerializeField] private Animator animator;

        [Header("Parameter Names")]
        [SerializeField] private string speedParam = "MoveSpeed";
        [SerializeField] private string forwardParam = "MoveForward";
        [SerializeField] private string strafeParam = "MoveRight";

        [Header("Damping (seconds)")]
        [SerializeField, Min(0f)] private float speedDampTime = 0.05f;
        [SerializeField, Min(0f)] private float directionalDampTime = 0.05f;

        private int _speedHash = -1;
        private int _forwardHash = -1;
        private int _strafeHash = -1;

        private bool _hasSpeedParam;
        private bool _hasForwardParam;
        private bool _hasStrafeParam;

        private void Reset()
        {
            brain = GetComponent<PlayerBrain>();
            animator = GetComponent<Animator>();
            CacheParameterHashes();
        }

        private void OnValidate()
        {
            if (!brain) brain = GetComponent<PlayerBrain>();
            if (!animator) animator = GetComponent<Animator>();
            CacheParameterHashes();
        }

        private void Awake()
        {
            if (!animator) animator = GetComponent<Animator>();
            if (!brain) brain = GetComponent<PlayerBrain>();
            if (!animator)
            {
                enabled = false;
                return;
            }
            CacheParameterHashes();
        }

        private void Update()
        {
            if (!brain || !animator)
            {
                enabled = false;
                return;
            }

            float dt = Time.deltaTime;

            if (_hasSpeedParam)
                animator.SetFloat(_speedHash, brain.NormalizedSpeed, speedDampTime, dt);

            Vector2 axes = brain.MoveAxes;
            if (_hasForwardParam)
                animator.SetFloat(_forwardHash, Mathf.Clamp(axes.y, -1f, 1f), directionalDampTime, dt);
            if (_hasStrafeParam)
                animator.SetFloat(_strafeHash, Mathf.Clamp(axes.x, -1f, 1f), directionalDampTime, dt);
        }

        private void CacheParameterHashes()
        {
            (_hasSpeedParam, _speedHash) = ResolveHash(speedParam);
            (_hasForwardParam, _forwardHash) = ResolveHash(forwardParam);
            (_hasStrafeParam, _strafeHash) = ResolveHash(strafeParam);
        }

        private static (bool valid, int hash) ResolveHash(string param)
        {
            if (string.IsNullOrWhiteSpace(param)) return (false, 0);
            return (true, Animator.StringToHash(param));
        }
    }
}
