using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Gamedesign.VM_Game4
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody2D RB { get { return GetComponent<Rigidbody2D>(); } }

        [SerializeField]
        private GameObject _playerModel;

        [Space]
        [SerializeField]
        private Sprite _playerSpriteBurning;
        [SerializeField]
        private Sprite _playerSprite;

        [Space]
        [SerializeField]
        private float _moveSpeed = 10.0f;
        [SerializeField]
        private float _sprintSpeed = 15.0f;
        [SerializeField]
        private float JumpHeight = 8.5f;
        [SerializeField]
        private float CoyoteTime = 0.1f;
        [SerializeField]
        private float _stepOffset = 0.3f;
        [SerializeField]
        private float _normalisedMovement;

        [Space]
        [SerializeField]
        private float _CoyoteTimer;
        [SerializeField]
        private protected Vector3 _internalVelocity;

        public Vector3 _externalVelocity;

        [SerializeField]
        private bool _jump;
        [SerializeField]
        private bool _isGrounded;

        private bool _isSprinting;

        public bool _isBurning;

        private float _burnTimer = 10.0f;

        private void FixedUpdate()
        {
            _CoyoteTimer -= Time.deltaTime;

            Burn();

            ApplyGround();
            ApplyGravity();
            ApplyJump();

            ApplyDirection();
            ApplyMovement();

            ApplyVelocity();
        }

        private void Burn()
        {
            if (_isBurning)
            {
                _playerModel.GetComponentInChildren<SpriteRenderer>().sprite = _playerSpriteBurning;
                _burnTimer -= Time.deltaTime;

                if (_burnTimer <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void ApplyMovement()
        {
            if (_isGrounded && _isSprinting)
            {
                _internalVelocity.x = _normalisedMovement * _sprintSpeed;
            }
            else
                _internalVelocity.x = _normalisedMovement * _moveSpeed;
        }

        private void ApplyDirection()
        {
            if (_internalVelocity.x == 0)
                return;

            var playerRenderer = _playerModel.GetComponentInChildren<SpriteRenderer>();

            if (_internalVelocity.x < 0 && playerRenderer.flipX == false)
            {
                playerRenderer.flipX = true;
            }
            else if (_internalVelocity.x > 0 && playerRenderer.flipX == true)
            {
                playerRenderer.flipX = false;
            }
        }

        private void ApplyVelocity()
        {
            RB.velocity = _internalVelocity + _externalVelocity;
        }

        private void ApplyJump()
        {
            if (_jump)
            {
                _internalVelocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * JumpHeight);

                _CoyoteTimer -= CoyoteTime;
                _jump = false;
            }
        }

        private void ApplyGround()
        {
            if (_isGrounded)
            {
                _internalVelocity.y -= _stepOffset * 10;

                _CoyoteTimer = CoyoteTime;
            }
        }

        private void ApplyGravity()
        {
            if (!_isGrounded)
                _internalVelocity.y += (Physics.gravity.y * Time.deltaTime) * 2;
            else
                _internalVelocity -= Vector3.Project(_internalVelocity, Physics.gravity.normalized);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _normalisedMovement = context.ReadValue<Vector2>().x;
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && !_jump && _CoyoteTimer >= 0)
            {
                //Debug.Log("Coyotejump has started!");
                _jump = true;
            }
        }
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started)
                _isSprinting = true;

            if (context.canceled)
                _isSprinting = false;
        }

        public void OnResetLevel(InputAction.CallbackContext context)
        {
            if (context.started)
                Destroy(this.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var hitNormal = collision.GetContact(0).normal;
            hitNormal = new Vector2(Mathf.RoundToInt(hitNormal.x), Mathf.RoundToInt(hitNormal.y));
            //Debug.Log($"Hit normal data: {hitNormal}");

            Rigidbody _detectedRigidBody = collision.gameObject.GetComponent<Rigidbody>();

            if (hitNormal.y == -1.0f)
            {
                //Debug.Log("Object above player detected");
                _internalVelocity.y = -0.5f;
                _externalVelocity.y = 0.0f;
            }
            else if (_detectedRigidBody != null)
            {
                //Note: Negative Y velocity will be replaced by gravity.
                _externalVelocity = new Vector3(_detectedRigidBody.velocity.x, Mathf.Max(_detectedRigidBody.velocity.y, 0),
                    _detectedRigidBody.velocity.z);

                Debug.Log("Player is now standing on top of " + collision.gameObject.GetComponent<Rigidbody>().gameObject);
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 4)
                return;

            _isGrounded = true;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _isGrounded = false;
        }
        private void OnDestroy()
        {
            var nextCoal = FindObjectOfType<PlayerController>();

            if (nextCoal != null)
            {

            }
        }
    }
}
