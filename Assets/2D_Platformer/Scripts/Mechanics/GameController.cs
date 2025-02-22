using UnityEngine;
using Core;
using Model;

namespace Mechanics
{
    public class GameController : MonoBehaviour
    {
        private static GameController Instance { get; set; }

        /* Экземпляр модели здесь поселился, чтоб прокинуть данные через инспектор.*/
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