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
    public float duration;

    float timer;
    Player player;

    private void Awake()
    {
        // GetComponentInParent �Լ��� �θ��� ������Ʈ ��������
        player = GetComponentInParent<Player>();
    }

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
            case 1:
                timer += Time.deltaTime;

                // speed ���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻� ���� ����
                // speed ���� ����ӵ��� �ǹ�: ���� ���� ���� �߻�
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
            case 2:
                
                break;

        }

        // .. Test Code ..
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
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
        switch (id)
        {
            case 0:
                speed = 150; 
                Batch();
                break;
            case 1:
                speed = 0.3f;
                break;
            case 2:
                speed = 0.3f;
                Zoning(Vector3.zero);
                break;
            default:
                for (int index = 0; index < count; index++)
                {
                    Vector3 targetPos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
                    Zoning(targetPos);
                }

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


            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        // FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
    }

    // area attack : bullet remains in the target area for a certain period of time
    void Zoning(Vector3 targetPos)
    {
        Transform bullet;

        if (index < transform.childCount)            
        {
            bullet = transform.GetChild(index);
        }
        else
        {
            bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.parent = transform; 
        }

        bullet.localPosition = targetPos;

        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        
    }
}
