using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamedesign.VM_Game4
{
    public class Forge : MonoBehaviour
    {
        public Sprite ActiveForge;
        public GameObject Endscreen;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && collision.GetComponent<PlayerController>()._isBurning)
            {
                this.GetComponent<SpriteRenderer>().sprite = ActiveForge;
                Destroy(collision.gameObject);

                Endscreen.SetActive(true);
            }
        }
    }
}
