using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Transparnet : MonoBehaviour
{
    public float amount;

    SpriteRenderer[] sprites;
    Tilemap tilemap;
    Player player;

    private void Awake()
    {
        if(transform.tag == "Foreground")
        {
            tilemap = GetComponent<Tilemap>();
        }
        else
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
        }
        player = GameManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        /*
        if (player.transform.position.y < transform.position.y) //when player is in front of the pillar
            return;
        */

        if(transform.tag == "Foreground")
        {
            Color temp = tilemap.color;
            temp.a = amount;
            tilemap.color = temp;
        }
        else
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                Color temp = sprite.color;
                temp.a = amount;
                sprite.color = temp;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if(transform.tag == "Foreground")
        {
            Color temp = tilemap.color;
            temp.a = 1f;
            tilemap.color = temp;
        }
        else
        {
            foreach(SpriteRenderer sprite in sprites)
            {
                Color temp = sprite.color;
                temp.a = 1f;
                sprite.color = temp;
            }
        }
    }
}
