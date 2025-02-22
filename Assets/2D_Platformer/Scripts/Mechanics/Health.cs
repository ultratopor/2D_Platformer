using UnityEngine;
using Core;
using Gameplay;

namespace Mechanics
{
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// Максимальное количество очков жизни для объекта.
        /// </summary>
        public int MaxHp = 1;

        /// <summary>
        /// Указывает, следует ли считать объект "живым".
        /// </summary>
        public bool IsAlive => _currentHp > 0;

        private int _currentHp;

        /// <summary>
        /// Добавляет одну жизнь.
        /// </summary>
        public void Increment()
        {
            _currentHp = Mathf.Clamp(_currentHp + 1, 0, MaxHp);
        }

        /// <summary>
        /// Уменьшает на одну. При достижении 0 единиц здоровья будет запущено событие HealthIsZero.
        /// </summary>
        public void Decrement()
        {
            _currentHp = Mathf.Clamp(_currentHp - 1, 0, MaxHp);
            if (_currentHp == 0)
            {
                // на случай нескольких жизней
                /*var ev = Simulation.Schedule<HealthIsZero>();
                ev.Health = this;*/
            }
        }

        /// <summary>
        /// Умертвление
        /// </summary>
        public void Die()
        {
            while (_currentHp > 0) Decrement();
        }

        private void Awake()
        {
            _currentHp = MaxHp;
        }
    }
}