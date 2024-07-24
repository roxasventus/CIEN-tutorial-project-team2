using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool noSpawn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("NoSpawn"))
            return;
        
        noSpawn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("NoSpawn"))
            return;

        noSpawn = false;
    }
}
