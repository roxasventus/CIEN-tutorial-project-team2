using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            GameManager.instance.player.GetHit(collision.gameObject.GetComponent<Trap>().damage);
        }
    }
}
