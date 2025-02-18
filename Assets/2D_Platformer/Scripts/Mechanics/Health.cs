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
        public int maxHP = 1;

        /// <summary>
        /// Указывает, следует ли считать объект "живым".
        /// </summary>
        public bool IsAlive => currentHP > 0;

        int currentHP;

        /// <summary>
        /// Увеличьте HP объекта.
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
        }

        /// <summary>
        /// Уменьшите количество единиц здоровья объекта. При достижении 0 единиц здоровья будет запущено событие HealthIsZero.
        /// </summary>
        public void Decrement()
        {
            currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
            if (currentHP == 0)
            {
                var ev = Simulation.Schedule<HealthIsZero>();
                ev.health = this;
            }
        }

        /// <summary>
        /// Уменьшайте HP объекта до тех пор, пока HP не достигнет 0.
        /// </summary>
        public void Die()
        {
            while (currentHP > 0) Decrement();
        }

        void Awake()
        {
            currentHP = maxHP;
        }
    }
}