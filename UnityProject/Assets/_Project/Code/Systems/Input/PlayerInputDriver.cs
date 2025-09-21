using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems.Input
{
    public struct MoveIntent { public Vector2 dir; public bool sprint; }
    public struct AttackIntent { public bool pressed; public bool held; }

    public class PlayerInputDriver : MonoBehaviour
    {
        public MoveIntent Move;
        public AttackIntent Attack;

        bool _attackHeld;

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
    }
}
