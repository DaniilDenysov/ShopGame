using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Player.Camera
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform body;
        [SerializeField, Range(0.1f, 1000f)] private float sensitivityX = 2f;
        [SerializeField, Range(0.1f, 1000f)] private float sensitivityY = 1f;
        [SerializeField, Range(-89f, 0f)] private float minY = -35f;
        [SerializeField, Range(0f, 89f)] private float maxY = 60f;

        private PlayerInputActions inputActions;
        private Vector2 lookInput;
        private float yaw;
        private float pitch;

        [Inject]
        private void Construct(PlayerInputActions inputActions)
        {
            this.inputActions = inputActions;
        }

        private void OnEnable()
        {
            inputActions.Player.Look.performed += OnLookPerformed;
            inputActions.Player.Look.canceled += OnLookCanceled;
        }

        private void OnDisable()
        {
            inputActions.Player.Look.performed -= OnLookPerformed;
            inputActions.Player.Look.canceled -= OnLookCanceled;
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        private void OnLookCanceled(InputAction.CallbackContext context)
        {
            lookInput = Vector2.zero;
        }

        private void Update()
        {
            yaw += lookInput.x * sensitivityX * Time.deltaTime;
            pitch -= lookInput.y * sensitivityY * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minY, maxY);

            body.rotation = Quaternion.Euler(0f, yaw, 0f);
            transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}
