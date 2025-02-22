using UnityEngine;
using Mechanics;
using Platformer.UI;
using Unity.Cinemachine;

namespace Model
{
    /// <summary>
    /// Основная модель, содержащая необходимые данные для реализации игры.
    /// Он инициализируется данными в классе GameController.
    /// </summary>
    [System.Serializable]
    public class PlatformerModel
    {
        public CinemachineCamera VirtualCamera;
        public PlayerController Player;
        public Transform SpawnPoint;
        public MetaGameController MetaGameController;
        public MainUIController UIController;

        /// <summary>
        /// Усилитель прыжка
        /// </summary>
        public float JumpModifier = 1.5f;

        /// <summary>
        /// Модификатор падения после прыжка
        /// </summary>
        public float JumpDeceleration = 0.5f;

    }
}