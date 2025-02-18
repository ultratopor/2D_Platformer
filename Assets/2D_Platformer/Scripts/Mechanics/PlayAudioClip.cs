using UnityEngine;

namespace Mechanics
{
    public class PlayAudioClip : StateMachineBehaviour
    {
        /// <summary>
        /// Точка в нормализованном времени, в которую должен воспроизводиться клип.
        /// </summary>
        public float t = 0.5f;

        /// <summary>
        /// Если значение больше нуля, то нормализованное время будет равно (нормализованноеВремя % модуль). 
        /// Это используется для повторения аудиоклипа при зацикливании состояния анимации.
        /// </summary>
        public float modulus = 0f;

        /// <summary>
        /// Аудиоклип, который будет воспроизводиться.
        /// </summary>
        public AudioClip clip;

        float last_t = -1f;

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var nt = stateInfo.normalizedTime;
            if (modulus > 0f) nt %= modulus;
            if (nt >= t && last_t < t)
                AudioSource.PlayClipAtPoint(clip, animator.transform.position);
            last_t = nt;
        }
    }
}