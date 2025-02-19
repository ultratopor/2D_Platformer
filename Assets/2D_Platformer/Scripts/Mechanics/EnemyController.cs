using UnityEngine;
using Gameplay;
using Core;
using UnityEngine.Serialization;

namespace Mechanics
{
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath Path;
        public AudioClip Ouch;
        [HideInInspector] public AnimationController Control;
        [HideInInspector] public Collider2D Collider;
        [HideInInspector] public AudioSource Audio;

        private Mover _mover;
        private SpriteRenderer _spriteRenderer;

        public Bounds Bounds => Collider.bounds;

        private void Awake()
        {
            Control = GetComponent<AnimationController>();
            Collider = GetComponent<Collider2D>();
            Audio = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            if(Path is null) Debug.LogError("EnemyController: Path is null");
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            var ev = Simulation.Schedule<PlayerEnemyCollision>();
            ev.Player = player;
            ev.Enemy = this;
        }

        private void Update()
        {
            _mover ??= Path.CreateMover(Control.MaxSpeed * 0.5f);
            Control.Move.x = Mathf.Clamp(_mover.Position.x - transform.position.x, -1, 1);
        }

    }
}