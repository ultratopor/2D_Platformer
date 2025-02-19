using UnityEngine;
using Gameplay;
using Model;
using Core;
using UnityEngine.InputSystem;

namespace Mechanics
{
    public class PlayerController : KinematicObject
    {
        public AudioClip JumpAudio;
        public AudioClip RespawnAudio;
        public AudioClip OuchAudio;
        [HideInInspector] public Collider2D Collider2d;
        [HideInInspector] public AudioSource AudioSource;
        [HideInInspector] public Health PlayerHealth;
        [HideInInspector] public Animator PlayerAnimator;
        /// <summary>
        /// Максимальная скорость передвижения.
        /// </summary>
        [SerializeField] private float _maxSpeed = 2;
        /// <summary>
        /// Начальная скорость прыжка в начале прыжка.
        /// </summary>
        [SerializeField] private float _jumpTakeOffSpeed = 2;
        
        public bool ControlEnabled = true;
        public JumpState PlayerJumpState = JumpState.Grounded;
        
        private bool _stopJump;
        
        private bool _jump;
        private Vector2 _move;
        private SpriteRenderer _spriteRenderer;
        private InputSystem_Actions _input;
        private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => Collider2d.bounds;

        private void Awake()
        {
            PlayerHealth = GetComponent<Health>();
            AudioSource = GetComponent<AudioSource>();
            Collider2d = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            PlayerAnimator = GetComponent<Animator>();
            _input = new InputSystem_Actions();
            _input.Enable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.Player.Jump.performed += OnJump;
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (!ControlEnabled) return;
            if (PlayerJumpState == JumpState.Grounded)
            {
                PlayerJumpState = JumpState.PrepareToJump;
            }
            else
            {
                _stopJump = true;
                Simulation.Schedule<PlayerStopJump>().player = this;
            }
        }

        protected override void Update()
        {
            if (ControlEnabled)
            {
                _move.x = _input.Player.Move.ReadValue<Vector2>().x;
            }
            else
            {
                _move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        private void UpdateJumpState()
        {
            _jump = false;
            switch (PlayerJumpState)
            {
                case JumpState.PrepareToJump:
                    PlayerJumpState = JumpState.Jumping;
                    _jump = true;
                    _stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Simulation.Schedule<PlayerJumped>().Player = this;
                        PlayerJumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Simulation.Schedule<PlayerLanded>().player = this;
                        PlayerJumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    PlayerJumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (_jump && IsGrounded)
            {
                Velocity.y = _jumpTakeOffSpeed * _model.JumpModifier;
                _jump = false;
            }
            else if (_stopJump)
            {
                _stopJump = false;
                if (Velocity.y > 0)
                {
                    Velocity.y *= _model.JumpDeceleration;
                }
            }

            if (_move.x > 0.01f)
                _spriteRenderer.flipX = true;
            else if (_move.x < -0.01f)
                _spriteRenderer.flipX = false;

            PlayerAnimator.SetBool("grounded", IsGrounded);
            PlayerAnimator.SetFloat("velocityX", Mathf.Abs(Velocity.x) / _maxSpeed);

            TargetVelocity = _move * _maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}