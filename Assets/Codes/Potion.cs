using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class Potion : MonoBehaviour
{
    GameObject barrier;
    public float Heal;
    public enum PotionType { Barrier, Heal }
    public PotionType potionType;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        else
        {
            if (potionType == PotionType.Barrier)
            {
                barrier = GameManager.instance.player.transform.Find("Barrier").gameObject;
                barrier.SetActive(true);
            }
            else if (potionType == PotionType.Heal)
            {
                if (GameManager.instance.health + GameManager.instance.maxHealth * Heal > GameManager.instance.maxHealth)
                {
                    GameManager.instance.health = GameManager.instance.maxHealth;
                }
                else
                    GameManager.instance.health += GameManager.instance.maxHealth * Heal;
            }
            Destroy(gameObject);
        }
    }
}
