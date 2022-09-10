using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamedesign.VM_Game2
{
    public class DeathZone : MonoBehaviour
    {
        [SerializeField]
        private int _sceneIndex;

        private void OnCollisionStay2D(Collision2D collision)
        {
            //Debug.Log("wall hit");
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}