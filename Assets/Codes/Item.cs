using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // ������ ������ �ʿ��� ������ ����
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    // �̸��� ���� �ؽ�Ʈ ���� �߰� �� �ʱ�ȭ
    Text textName;
    Text textDesc;


    private void Awake()
    {
        // �ڽ� ������Ʈ�� ������Ʈ�� �ʿ�
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInChildren���� �ι�° ������ �������� (ù��°�� �ڱ��ڽ�)
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0]; // �ؽ�Ʈ�� �Ѱ� �ۿ� �����Ƿ� �ε��� 0�� ����ϵ� �ȴ�
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

    // ��ư Ŭ�� �̺�Ʈ�� ������ �Լ� �߰�
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    // AddComponent<T>: ��Ÿ�� �ð��� ���ӿ�����Ʈ�� T ������Ʈ�� �߰��ϴ� �Լ�(�߰��� ������Ʈ�� return�� ���ش�)
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
                    // AddComponent<T>: ��Ÿ�� �ð��� ���ӿ�����Ʈ�� T ������Ʈ�� �߰��ϴ� �Լ�(�߰��� ������Ʈ�� return�� ���ش�)
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

        // �ִ� ���� ���� ��, ��ư ��ȣ�ۿ� �Ұ��� �ǵ���
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

}