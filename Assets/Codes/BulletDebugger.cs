using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDebugger : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // debug bullet's name
            Debug.Log(collision.gameObject.name);
        }
    }
}
