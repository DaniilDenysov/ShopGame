using ShopGame.Player.Hunger;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Range(1f, 10000f)] private float movementSpeed = 5f;
        [SerializeField, Range(1f, 10000f)] private float jumpForce = 5f;
        [SerializeField] private PlayerHunger playerHunger;

        private CharacterController controller;
        private PlayerInputActions inputActions;
        private Vector2 inputVector;
        private float verticalVelocity;

        private bool jumpRequested;
        private bool wasGrounded;

        [Inject]
        private void Construct(PlayerInputActions inputActions)
        {
            this.inputActions = inputActions;
        }

        private void OnEnable()
        {
            inputActions.Player.Jump.performed += OnJump;
        }

        private void OnDisable()
        {
            inputActions.Player.Jump.performed -= OnJump;
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (controller.isGrounded)
            {
                playerHunger.MovementMultiplier = 3;
                jumpRequested = true;
            }
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            inputVector = inputActions.Player.Move.ReadValue<Vector2>();
            Vector3 move = new Vector3(inputVector.x, 0f, inputVector.y);
            move = transform.TransformDirection(move) * movementSpeed;

            bool isGrounded = controller.isGrounded;

            if (isGrounded && !wasGrounded)
            {
                verticalVelocity = -0.5f;
            }

            if (isGrounded && jumpRequested)
            {
                verticalVelocity = jumpForce;
                playerHunger.MovementMultiplier = 1;
                jumpRequested = false;
            }

            if (!isGrounded)
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
            if (!jumpRequested && move.magnitude > 0)
            {
                playerHunger.MovementMultiplier = 2;
            }
            else
            {
                playerHunger.MovementMultiplier = 1;
            }
            move.y = verticalVelocity;
            controller.Move(move * Time.deltaTime);
            wasGrounded = isGrounded;
        }
    }
}
