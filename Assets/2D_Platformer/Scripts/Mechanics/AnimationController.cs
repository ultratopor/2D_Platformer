using UnityEngine;
using Model;
using Core;

namespace Mechanics
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        /// <summary>
        /// Максимальная скорость передвижения
        /// </summary>
        public float MaxSpeed = 7;
        /// <summary>
        /// Максимальная скорость прыжка
        /// </summary>
        public float JumpTakeOffSpeed = 7;

        /// <summary>
        /// Используется для указания желаемого направления движения.
        /// </summary>
        public Vector2 Move;

        /// <summary>
        /// Состояние прыжка
        /// </summary>
        public bool Jump;

        /// <summary>
        /// Обнуление прыжка
        /// </summary>
        public bool StopJump;
        
        public Animator Animator;

        private SpriteRenderer _spriteRenderer;
        private PlatformerModel _model = Simulation.GetModel<PlatformerModel>();

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            if (Jump && IsGrounded)
            {
                Velocity.y = JumpTakeOffSpeed * _model.JumpModifier;
                Jump = false;
            }
            else if (StopJump)
            {
                StopJump = false;
                if (Velocity.y > 0)
                {
                    Velocity.y *= _model.JumpDeceleration;
                }
            }
            // поворот модели в сторону движения
            if (Move.x > 0.01f)
                _spriteRenderer.flipX = false;
            else if (Move.x < -0.01f)
                _spriteRenderer.flipX = true;

            //_animator.SetBool("grounded", IsGrounded);
            Animator.SetFloat("velocity", Mathf.Abs(Velocity.x) / MaxSpeed);

            TargetVelocity = Move * MaxSpeed;
        }
    }
}