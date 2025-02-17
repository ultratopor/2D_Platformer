using UnityEngine;
using Model;
using Core;
using System.Collections;
using System.Collections.Generic;

namespace Mechanics
{
    /// <summary>
    /// AnimationController объединяет физику и анимацию. Обычно он используется для простой анимации врагов.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        /// <summary>
        /// Максимальная скорость передвижения
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Максимальная скорость прыжка
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// Используется для указания желаемого направления движения.
        /// </summary>
        public Vector2 move;

        /// <summary>
        /// Состояние прыжка
        /// </summary>
        public bool jump;

        /// <summary>
        /// Установите значение true, чтобы обнулить текущую скорость прыжка.
        /// </summary>
        public bool stopJump;

        SpriteRenderer spriteRenderer;
        Animator animator;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }
    }
}