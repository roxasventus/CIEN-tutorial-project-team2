using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // 아이템 관리에 필요한 변수들 선언
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    // 이름과 설명 텍스트 변수 추가 및 초기화
    Text textName;
    Text textDesc;


    private void Awake()
    {
        // 자식 오브젝트의 컴포넌트가 필요
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInChildren에서 두번째 값으로 가져오기 (첫번째는 자기자신)
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0]; // 텍스트는 한개 밖에 없으므로 인덱스 0을 사용하도 된다
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;

    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.MagicCircle:
                if (data.magicCircleWaits[level] != 0)
                    textDesc.text = string.Format(data.itemDesc, data.magicCircleWaits[level]);
                else
                    textDesc.text = string.Format(data.itemDesc, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }


    private void LateUpdate()
    {
        textLevel.text = "Lv." + (level + 1);
    }

    // 버튼 클릭 이벤트와 연결할 함수 추가
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    // AddComponent<T>: 런타임 시간에 게임오브젝트에 T 컴포넌트를 추가하는 함수(추가한 컴포넌트를 return도 해준다)
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount, 0);
                }
                level++;
                break;
            case ItemData.ItemType.MagicCircle:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    // AddComponent<T>: 런타임 시간에 게임오브젝트에 T 컴포넌트를 추가하는 함수(추가한 컴포넌트를 return도 해준다)
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    float nextWaitTime = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];
                    nextWaitTime += data.magicCircleWaits[level];

                    weapon.LevelUp(nextDamage, nextCount, nextWaitTime);
                }
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        // 최대 레벨 도달 시, 버튼 상호작용 불가가 되도록
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

}
