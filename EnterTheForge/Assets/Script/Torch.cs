using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamedesign.VM_Game4
{
    public class Torch : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var pc = collision.gameObject.GetComponent<PlayerController>();
                pc._isBurning = true;
            }
        }
    }
}
