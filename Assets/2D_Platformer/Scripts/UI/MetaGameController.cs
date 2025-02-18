using UnityEngine;
using Mechanics;

namespace Platformer.UI
{
    /// <summary>
    /// Переключатель меню
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// Главное меню
        /// </summary>
        [SerializeField] private MainUIController _mainMenu;

        /// <summary>
        /// Список канвасов на сцене, кроме главного меню
        /// </summary>
        [SerializeField] private Canvas[] _gamePlayCanvases;

        [SerializeField] private GameController _gameController;

        private bool showMainCanvas = false;

        private void OnEnable()
        {
            SwitchMainMenu(showMainCanvas);
        }

        /// <summary>
        /// Вкл/выкл гав меню, если внутренне состояние отличается от внешнего
        /// </summary>
        /// <param name="show">Булка, указывающая на вкл/выкл</param>
        private void ToggleMainMenu(bool show)
        {
            if (showMainCanvas != show)
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
            this.showMainCanvas = show;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Menu"))
            {
                ToggleMainMenu(show: !showMainCanvas);
            }
        }

    }
}