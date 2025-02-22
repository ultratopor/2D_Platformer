using UnityEngine;

namespace Platformer.UI
{
    /// <summary>
    /// Переключатель канвасов
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _panels;
        /// <summary>
        /// Выключает все ненужные канвасы, включает нужный
        /// </summary>
        /// <param name="index">Индекс нужного канваса</param>
        public void SetActivePanel(int index)
        {
            for (var i = 0; i < _panels.Length; i++)
            {
                var active = i == index;
                if (_panels[i].activeSelf != active) _panels[i].SetActive(active);
            }
        }

        private void OnEnable()
        {
            SetActivePanel(0);
        }
    }
}