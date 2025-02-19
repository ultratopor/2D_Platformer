using UnityEngine;
using Core;
using Model;

namespace Mechanics
{
    /// <summary>
    /// Доисторический синглтон
    /// </summary>
    public class GameController : MonoBehaviour
    {
        private static GameController Instance { get; set; }

        /*Это поле модели является общедоступным, поэтому его можно изменить в инспекторе.
        Ссылка на самом деле поступает из InstanceRegister и используется совместно в симуляции и событиях.
        Unity будет выполнять десериализацию по этой общей ссылке при загрузке сцены,
        что позволяет удобно настраивать модель в инспекторе.*/
        public PlatformerModel Model = Simulation.GetModel<PlatformerModel>();

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
    }
}