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

        private CharacterController controller;
        private PlayerInputActions inputActions;
        private Vector2 inputVector;
        private float verticalVelocity;

        [Inject]
        private void Construct(PlayerInputActions inputActions)
        {
            this.inputActions = inputActions;
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

            if (controller.isGrounded)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity -= -Physics.gravity.y * Time.deltaTime;
            }

            move.y = verticalVelocity;
            controller.Move(move * Time.deltaTime);
        }
    }

}