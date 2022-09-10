using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamedesign.VM_Game4
{
    public class Pillar : MonoBehaviour
    {
        public Sprite ActivePillar;
        public GameObject[] Objects;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && collision.GetComponent<PlayerController>()._isBurning)
            {
                this.GetComponent<SpriteRenderer>().sprite = ActivePillar;
                Destroy(collision.gameObject);

                foreach (var item in Objects)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}
