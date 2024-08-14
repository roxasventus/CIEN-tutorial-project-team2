using System;
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
    GameObject hit;

    private void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.Log(GameManager.instance);
            return;
        }
        hit = GameManager.instance.hitEffect;
    }

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
            AudioManager.instance.PlaySfx(AudioManager.Sfx.potion);

            //Instantiate(hit, transform);
            StartCoroutine(ItemGet(hit));
            //Destroy(gameObject);
        }
    }
    
    IEnumerator ItemGet(GameObject hit)
    {
        Instantiate(hit, transform);
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
    
}
