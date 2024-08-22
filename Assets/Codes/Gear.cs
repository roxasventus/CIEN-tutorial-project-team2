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

    // ��� ���Ӱ� �߰��ǰų� ������ �� �� ���� ���� �Լ��� ȣ��
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    // Ÿ�Կ� ���� �����ϰ� ������ ��������ִ� �Լ� �߰�
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

    // �尩�� ����� ������� �ø��� �Լ� �ۼ�
    void RateUp()
    {
        // �÷��̾�� �ö󰡼� ��� Weapon�� ��������
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                // ���� ����
                case 0:
                    weapon.speed = 150 + (150 * rate);
                    break;
                // ���Ÿ� ����
                case 1:
                    weapon.speed = 0.7f * (1f - rate);  // ���� ��� �۾����� -> �� ���� �߻��ϰ� �ȴ�
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

    // �Ź��� ����� �̵� �ӵ��� �ø��� �Լ� �ۼ�
    void SpeedUp()
    {
        float speed = 3;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
