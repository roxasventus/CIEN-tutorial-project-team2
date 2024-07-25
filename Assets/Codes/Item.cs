using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Accessory accessory;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDescription;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDescription = texts[2];

        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Level." + (level+1);
        textDescription.text = data.itemDescription;
    }

    public void OnClick()
    {
        switch (data.type)
        {
            case ItemData.ItemType.Weapon:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data); 
                }
                else
                {
                    float nextDamage = data.damages[level];
                    int nextCount = data.counts[level];
                    float nextMagicCircleWait = data.magicCircleWait[level];

                    weapon.LevelUp(nextDamage, nextCount, nextMagicCircleWait);
                }
                break;
                
            case ItemData.ItemType.Accessory:
                if (level == 0)
                {
                    GameObject newAccessory = new GameObject();
                    accessory = newAccessory.AddComponent<Accessory>();
                    accessory.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    accessory.LevelUp(nextRate);
                }
        
                break;
        }

        level++;

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
