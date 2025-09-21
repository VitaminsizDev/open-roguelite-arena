using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Game.Systems.Camera
{
    /// Gates CinemachineInputAxisController X/Y with RMB. Zoom (Z) unaffected.
    [RequireComponent(typeof(CinemachineCamera))]
    public class OrbitalFollowInputBridge : CinemachineInputAxisController
    {
        [Tooltip("Button action (Right Mouse Button)")]
        public InputActionReference rmb;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (rmb != null && rmb.action != null)
            {
                rmb.action.Enable();
                rmb.action.performed += OnRmbChanged;
                rmb.action.canceled  += OnRmbChanged;
                SetGate(rmb.action.IsPressed());
            }
        }

        protected override void OnDisable()
        {
            if (rmb != null && rmb.action != null)
            {
                rmb.action.performed -= OnRmbChanged;
                rmb.action.canceled  -= OnRmbChanged;
            }
            base.OnDisable();
        }

        private void OnRmbChanged(InputAction.CallbackContext ctx)
        {
            bool pressed = rmb != null && rmb.action != null && rmb.action.IsPressed();
            SetGate(pressed);
        }

        private void SetGate(bool pressed)
        {
           Controllers[0].Enabled = pressed; // X
           Controllers[1].Enabled = pressed; // Y
        }
    }
}
