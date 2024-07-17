using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ���� ID, ������ ID, ������, ����, �ӵ�
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ID�� ���� ������ �и�
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back�� �ð����
                break;
            default:
                break;
        }

        // .. Test Code ..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    public void Init()
    {
        // ���� ID�� ���� ������ �и�
        switch (id)
        {
            case 0:
                speed = 150; // ����� �ð����
                Batch();
                break;
            default:
                break;
        }
    }

    // ������ ���⸦ ��ġ�ϴ� �Լ�
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {

            // ������ ������Ʈ�� Transform�� ���������� ����
            Transform bullet;



            // ���� ������Ʈ�� ���� Ȱ���ϰ� ���ڶ� ���� Ǯ������ ��������
            if (index < transform.childCount)             // �ڽ��� �ڽ� ������Ʈ ���� Ȯ���� childCount �Ӽ�����
            {
                // index�� ���� childCount ���� ����� GetChild �Լ��� ��������
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // �� ������ źȯ�� �θ�� PoolManager�̴�. �� ������ źȯ�� �÷��̾ ���󰡾� �ϹǷ� �θ� �÷��̾��� �ڽ� ������Ʈ�� Weapon 0�� �ٲ�� �Ѵ�. 
                bullet.parent = transform; // parent �Ӽ��� ���� �θ� ����
            }

            // źȯ�� ��ġ�� ȸ�� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ������ źȯ�� ������ ��ġ�� ��ġ
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            // Translate �Լ��� �ڽ��� �������� �̵�, �̵� ������ Space.World ��������
            bullet.Translate(bullet.up * 1.5f, Space.World);


            bullet.GetComponent<Bullet>().Init(damage, -1); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
        }
    }
}
