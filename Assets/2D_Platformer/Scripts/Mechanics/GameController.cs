using UnityEngine;
using Core;
using Model;

namespace Mechanics
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        /*Это поле модели является общедоступным, поэтому его можно изменить в инспекторе.
        Ссылка на самом деле поступает из InstanceRegister и используется совместно в симуляции и событиях.
        Unity будет выполнять десериализацию по этой общей ссылке при загрузке сцены,
        что позволяет удобно настраивать модель в инспекторе.*/
        public PlatformerModel Model = Simulation.GetModel<PlatformerModel>();

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
    }
}