using UnityEngine;
using Gameplay;
using Model;
using Core;
using UnityEngine.Serialization;

namespace Mechanics
{
    public class PlayerController : KinematicObject
{
    public AudioClip JumpAudio;
    public AudioClip RespawnAudio;
    public AudioClip OuchAudio;
    public Collider2D Collider2d;
    public AudioSource AudioSource;
    public Health Health;
    public Animator Animator;
    
    public bool ControlEnabled = true;
    public JumpState PlayerJumpState = JumpState.Grounded;
    /// <summary>
    /// Максимальная скорость передвижения.
    /// </summary>
    private float _maxSpeed = 7;
    /// <summary>
    /// Начальная скорость прыжка в начале прыжка.
    /// </summary>
    private float _jumpTakeOffSpeed = 7;

    private bool _stopJump;
    

    private bool _jump;
    private Vector2 _move;
    private SpriteRenderer _spriteRenderer;
    private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

    public Bounds Bounds => Collider2d.bounds;

    private void Awake()
    {
        Health = GetComponent<Health>();
        AudioSource = GetComponent<AudioSource>();
        Collider2d = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (ControlEnabled)
        {
            _move.x = Input.GetAxis("Horizontal");
            if (PlayerJumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                PlayerJumpState = JumpState.PrepareToJump;
            else if (Input.GetButtonUp("Jump"))
            {
                _stopJump = true;
                Simulation.Schedule<PlayerStopJump>().player = this;
            }
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
                    Simulation.Schedule<PlayerJumped>().player = this;
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
            velocity.y = _jumpTakeOffSpeed * _model.jumpModifier;
            _jump = false;
        }
        else if (_stopJump)
        {
            _stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * _model.jumpDeceleration;
            }
        }

        if (_move.x > 0.01f)
            _spriteRenderer.flipX = false;
        else if (_move.x < -0.01f)
            _spriteRenderer.flipX = true;

        Animator.SetBool("grounded", IsGrounded);
        Animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / _maxSpeed);

        targetVelocity = _move * _maxSpeed;
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