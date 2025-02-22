using Core;
using Gameplay;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class MushroomBooster : MonoBehaviour
    {
        [SerializeField] private float _timeToWaitAnimations;
        public float Boost = 10f;
        private Collider2D _collider;
        private Animator _animator;
        public Bounds Bounds => _collider.bounds;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Simulation.Schedule<PlayerBoostedJump>().Booster = this;
            }
        }

        public void SetBoostAnimation()
        {
            _animator.SetTrigger("boost");
        }
    }
}