using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems.Input
{
    public struct MoveIntent { public Vector2 dir; public bool sprint; }
    public struct AttackIntent { public bool pressed; public bool held; }

    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputDriver : MonoBehaviour
    {
        public MoveIntent Move;
        public AttackIntent Attack;

        bool _attackHeld;
        PlayerInput _playerInput;
        InputAction _sprintAction;

        private void Awake()
        {
            if (!_playerInput) _playerInput = GetComponent<PlayerInput>();
            _sprintAction = _playerInput ? _playerInput.actions["Sprint"] : null;
        }

        private void OnEnable()
        {
            if (!_playerInput) _playerInput = GetComponent<PlayerInput>();
            if (_playerInput)
                _sprintAction = _playerInput.actions?["Sprint"];
        }

        // Send Messages requires InputValue signatures:

        public void OnMove(InputValue value)
        {
            Move.dir = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            Move.sprint = value.isPressed;
        }

        public void OnAttack(InputValue value)
        {
            bool down = value.isPressed;
            Attack.pressed = down && !_attackHeld; // edge on press
            Attack.held = down;                     // level while held
            _attackHeld = down;
        }

        private void Update()
        {
            if (_sprintAction != null)
                Move.sprint = _sprintAction.IsPressed();
        }

        private void OnDisable()
        {
            Move = default;
            Attack = default;
            _attackHeld = false;
        }
    }
}
