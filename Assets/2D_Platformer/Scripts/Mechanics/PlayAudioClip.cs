using UnityEngine;

namespace Mechanics
{
    public class PlayAudioClip : StateMachineBehaviour
    {
        /// <summary>
        /// В какой момент анимации будет воспроизводиться клип
        /// </summary>
        [SerializeField] private float _normalizeTime = 0.5f;

        /// <summary>
        /// Зацикливание клипа
        /// </summary>
        [SerializeField] private float _modulus = 0f;

        [SerializeField] private AudioClip _clip;

        private float _lastT = -1f;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var nt = stateInfo.normalizedTime;
            if (_modulus > 0f) nt %= _modulus;
            if (nt >= _normalizeTime && _lastT < _normalizeTime)
                AudioSource.PlayClipAtPoint(_clip, animator.transform.position);
            _lastT = nt;
        }
    }
}