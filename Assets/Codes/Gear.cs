using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    // 장비가 새롭게 추가되거나 레벨업 할 때 로직 적용 함수를 호출
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    // 타입에 따라 적절하게 로직을 적용시켜주는 함수 추가
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    // 장갑의 기능인 연사력을 올리는 함수 작성
    void RateUp()
    {
        // 플레이어로 올라가서 모든 Weapon을 가져오기
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                // 근접 무기
                case 0:
                    weapon.speed = 150 + (150 * rate);
                    break;
                // 원거리 무기
                case 1:
                    weapon.speed = 0.7f * (1f - rate);  // 값이 계속 작아진다 -> 더 빨리 발사하게 된다
                    break;
                case 2:
                    weapon.speed = 1f * (1f - rate);
                    break;
                case 3:
                    weapon.speed = 1.5f * (1f - rate);
                    break;
                case 4:
                    weapon.speed = 1f * (1f - rate);
                    break;
            }
        }
    }

    // 신발의 기능인 이동 속도을 올리는 함수 작성
    void SpeedUp()
    {
        float speed = 3;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
