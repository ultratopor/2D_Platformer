using UnityEngine;
using Model;
using Core;
using UnityEngine.InputSystem;

namespace Platformer.UI
{
    /// <summary>
    /// Переключатель меню
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        [SerializeField] private MainUIController _mainMenu;

        /// <summary>
        /// Список канвасов на сцене, кроме главного меню
        /// </summary>
        [SerializeField] private Canvas[] _gamePlayCanvases;

        private readonly PlatformerModel _model = Simulation.GetModel<PlatformerModel>();
        private bool _showMainCanvas = false;

        private void OnEnable()
        {
            SwitchMainMenu(_showMainCanvas);
            _model.Player.Input.Player.MenuPlay.performed += OnEcsPressed;
        }

        private void OnEcsPressed(InputAction.CallbackContext obj)
        {
            ToggleMainMenu(_showMainCanvas);
        }

        private void OnDisable()
        {
            _model.Player.Input.Player.MenuPlay.performed -= OnEcsPressed;
        }

        /// <summary>
        /// Вкл/выкл гав меню, если внутренне состояние отличается от внешнего
        /// </summary>
        /// <param name="show">Булка, указывающая на вкл/выкл</param>
        private void ToggleMainMenu(bool show)
        {
            if (_showMainCanvas != show)
            {
                SwitchMainMenu(show);
            }
        }
    
        /// <summary>
        /// Если всё правда, то переключаем глав меню и другие канвасы
        /// </summary>
        /// <param name="show">Булка переключения</param>
        private void SwitchMainMenu(bool show)
        {
            if (show)
            {
                Time.timeScale = 0;
                _mainMenu.gameObject.SetActive(true);
                foreach (var i in _gamePlayCanvases) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                _mainMenu.gameObject.SetActive(false);
                foreach (var i in _gamePlayCanvases) i.gameObject.SetActive(true);
            }
            _showMainCanvas = show;
        }
    }
}