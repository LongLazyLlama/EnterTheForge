using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gamedesign.VM_Game2
{
    public class PauzeMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pauzemenu;

        public void OnPauze(InputAction.CallbackContext context)
        {
            if (context.performed && !_pauzemenu.activeSelf)
            {
                _pauzemenu.SetActive(true);
                Cursor.visible = true;
            }
            else if(context.performed && _pauzemenu.activeSelf)
            {
                _pauzemenu.SetActive(false);
                Cursor.visible = false;
            }
        }

        public void Continue()
        {
            _pauzemenu.SetActive(false);
            Cursor.visible = false;
        }
    }
}
