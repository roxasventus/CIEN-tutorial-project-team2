using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : MonoBehaviour
{
    public int id;
    public float shotRate;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Accessory " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        this.id = data.itemId;
        shotRate = data.damages[0];
        ApplyAccessory();
    }

    public void LevelUp(float nextRate)
    {
        this.shotRate = nextRate;
        ApplyAccessory();
    }

    void ApplyAccessory()
    {
        switch (id)
        {
            case 7: // item id 7: Rate Up
                RateUp();
                break;
            case 8: // item id 8: Moving Speed Up
                MovingSpeedUp();
                break;
        }
    }

    // Attack Speed Up 공격 주기 단축
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (shotRate * 150);
                    break;
                default:
                    weapon.speed = 0.5f*(1f - shotRate);
                    break;
            }
        }
    }

    // Moving Speed Up 이동 속도 증가
    void MovingSpeedUp(){
        float speed = 3;
        GameManager.instance.player.speed = speed + (shotRate * speed);
    }
}
