using UnityEngine;
using Mechanics;
using Unity.Cinemachine;

namespace Model
{
    /// <summary>
    /// Основная модель, содержащая необходимые данные для реализации игры в стиле платформера.
    /// Этот класс должен содержать только данные и методы, которые работают с этими данными.
    /// Он инициализируется данными в классе GameController.
    /// </summary>
    [System.Serializable]
    public class PlatformerModel
    {
 
        public CinemachineCamera virtualCamera;

        public PlayerController player;

        public Transform spawnPoint;

        /// <summary>
        /// Усилитель прыжка
        /// </summary>
        public float jumpModifier = 1.5f;

        /// <summary>
        /// Модификатор падения после прыжка
        /// </summary>
        public float jumpDeceleration = 0.5f;

    }
}