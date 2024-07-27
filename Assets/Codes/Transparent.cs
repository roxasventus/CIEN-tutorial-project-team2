using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;

public class Transparnet : MonoBehaviour
{
    public float amount;

    SpriteRenderer[] sprites;
    Player player;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        player = GameManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (player.transform.position.y < transform.position.y) //when player is in front of the pillar
            return;


        foreach(SpriteRenderer sprite in sprites)
        {
            Color temp = sprite.color;
            temp.a = amount;
            sprite.color = temp;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (sprites[0].color.a < 1f)
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
