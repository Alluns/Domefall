using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
