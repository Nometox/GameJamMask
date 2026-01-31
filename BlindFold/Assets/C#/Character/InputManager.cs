using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Survive.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool BlindFold { get; private set; }
        public bool Jump { get; private set; }
        public bool Crouch { get; private set; }
        public bool Interact { get; private set; }

        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _BlindFoldAction;
        private InputAction _InteractAction;


        private void Awake()
        {
            HideCursor();
            _currentMap = PlayerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _BlindFoldAction = _currentMap.FindAction("BlindFold");
            _InteractAction = _currentMap.FindAction("Interact");


            _moveAction.performed += onMove;
            _lookAction.performed += onLook;
            _BlindFoldAction.performed += onBlindFold;
            _InteractAction.performed += onInteract;


            _moveAction.canceled += onMove;
            _lookAction.canceled += onLook;
            _BlindFoldAction.canceled += onBlindFold;
            _InteractAction.canceled += onInteract;

        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }
        private void onBlindFold(InputAction.CallbackContext context)
        {
            BlindFold = context.ReadValueAsButton();
        }
        private void onInteract(InputAction.CallbackContext context)
        {
            Interact = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
        }

    }
}