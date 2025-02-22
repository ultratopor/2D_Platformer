using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanics
{
    public class RestartScene : MonoBehaviour
    {
        // переделать
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}