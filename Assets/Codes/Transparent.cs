using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;

public class Transparnet : MonoBehaviour
{
    public float amount;

    SpriteRenderer sprite;
    Player player;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = GameManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (player.transform.position.y < transform.position.y) //when player is in front of the pillar
            return;

        Color temp = sprite.color;
        temp.a = amount;
        sprite.color = temp;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if(sprite.color.a < 1f)
        {
            Color temp = sprite.color;
            temp.a = 1f;
            sprite.color = temp;
        }
    }
}
