using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamedesign.VM_Game4
{
    public class ChangeScene : MonoBehaviour
    {
        public void loadScene(int sceneIndex)
            => SceneManager.LoadScene(sceneIndex);

        public void QuitGame()
            => Application.Quit();

        public void ReloadCurrentScene()
            => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
