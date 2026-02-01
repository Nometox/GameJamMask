using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Survive.Manager;

namespace Survive.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        public Camera playerCamera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        [SerializeField] private float AirResistance = 0.8f;


        public AudioSource walkSound;

        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;
        private bool _grounded = true;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private int _groundHash;
        private int _fallingHash;
        private float _xRotation;

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        private Vector2 _currentVelocity;

        [Header("Interaction")]
        [SerializeField] private Vector3 interactionRayPoint = default;
        [SerializeField] private float interactionDistance = default;
        [SerializeField] LayerMask interactionLayer = default;
        private Interactable currentInteractble;
        private void Start()
        {
            CursorShow(false);

            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();
            playerCamera = GetComponentInChildren<Camera>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
            _groundHash = Animator.StringToHash("Grounded");
            _fallingHash = Animator.StringToHash("Falling");

        }

        private void FixedUpdate()
        {
            Move();
        }
        void Update()
        {
            HandleInteractionCheck();
            HandleInteractionInput();
        }
        private void LateUpdate()
        {
            CamMovements();
        }

        public void CursorShow(bool On)
        {
            Cursor.lockState = On ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = On;
        }
        private void HandleInteractionCheck()
        {
            if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.gameObject.layer == 7 && (currentInteractble == null || hit.collider.gameObject.GetInstanceID() != currentInteractble.GetInstanceID()))
                {
                    hit.collider.TryGetComponent(out currentInteractble);

                    if (currentInteractble)
                    {
                        currentInteractble.OnFocus();
                    }
                }
            }
            else if (currentInteractble)
            {
                currentInteractble.OnLoseFocus();
                currentInteractble = null;
            }
        }

        private void HandleInteractionInput()
        {
            if (_inputManager.Interact && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
            {
                currentInteractble.OnInteract();
            }
        }
        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _walkSpeed;
            if (_inputManager.Move == Vector2.zero)
            {
                targetSpeed = 0;
                GameManager.Instance.SetPlayerMoving(false);

                if (walkSound.isPlaying)
                {
                    walkSound.Stop();
                }
            }
            else
            {
                targetSpeed = _walkSpeed;
                GameManager.Instance.SetPlayerMoving(true);
                if (!walkSound.isPlaying)
                {
                    walkSound.Play();
                }
            }

            if (_grounded)
            {

                _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
                _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

                var xVelDifference = _currentVelocity.x - _playerRigidbody.linearVelocity.x;
                var zVelDifference = _currentVelocity.y - _playerRigidbody.linearVelocity.z;

                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);
            }
            else
            {
                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x * AirResistance, 0, _currentVelocity.y * AirResistance)), ForceMode.VelocityChange);
            }


            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CamMovements()
        {
            if (!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;


            _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
        }


        private void SetAnimationGrounding()
        {
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundHash, _grounded);
        }
    }
}