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
<<<<<<< Updated upstream
    public float duration;
=======
    // ������ ���� �ð�
    public float magicCircleWait;

    Vector3 dir;
>>>>>>> Stashed changes

    float timer;
    Player player;

    private void Awake()
    {
        // GetComponentInParent �Լ��� �θ��� ������Ʈ ��������
<<<<<<< Updated upstream
        player = GetComponentInParent<Player>();
=======
        player = GameManager.instance.player;
>>>>>>> Stashed changes
    }

    // Modified by Jaewon

    // private void Start()
    // {
    //     Init();
    // }

    // Update is called once per frame
    void Update()
    {
        // ���� ID�� ���� ������ �и�
        switch (id)
        {
<<<<<<< Updated upstream
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back�� �ð����
                break;
=======
            // ��
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back�� �ð����
                break;
            // �ڵ�����
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            case 2:
                
                break;

=======
            // �ٶ󺸴� ���� ����
            case 2:
                timer += Time.deltaTime;

                // speed ���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻� ���� ����
                // speed ���� ����ӵ��� �ǹ�: ���� ���� ���� �߻�
                if (timer > speed && (player.inputVec.x != 0 || player.inputVec.y != 0))
                {
                    timer = 0f;
                    Fire2();
                }
                break;
            // ������ ���� ����
            case 3:
                timer += Time.deltaTime;

                // speed ���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻� ���� ����
                // speed ���� ����ӵ��� �ǹ�: ���� ���� ���� �߻�
                if (timer > speed)
                {
                    timer = 0f;
                    Fire3();
                }
                break;
            // �θ޶�
            case 4:
                timer += Time.deltaTime;

                // speed ���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻� ���� ����
                // speed ���� ����ӵ��� �ǹ�: ���� ���� ���� �߻�
                if (timer > speed)
                {
                    timer = 0f;
                    Fire4();
                }
                break;
            // ���� ������
            case 5:
                GameObject bullet = GameManager.instance.pool.prefabs[prefabId];
                timer += Time.deltaTime;

                // ������ ��Ÿ��
                if (timer > speed && bullet.activeSelf == false)
                {
                    timer = 0f;          
                    bullet.SetActive(!bullet.activeSelf);
                }
                // ������ �����ð�
                if (timer > magicCircleWait && bullet.activeSelf == true)
                {
                    timer = 0f;
                    bullet.SetActive(!bullet.activeSelf);
                }

                break;
            // ���� ������
            case 6:
                timer += Time.deltaTime;

                // speed ���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻� ���� ����
                // speed ���� ����ӵ��� �ǹ�: ���� ���� ���� �߻�
                if (timer > speed)
                {
                    timer = 0f;
                    Fire5();
                }
                break;
>>>>>>> Stashed changes
        }

        // .. Test Code ..
        // temporarily disabled by Jaewon
//         if (Input.GetButtonDown("Jump"))
//         {
//             LevelUp(10, 1, 0)
// ;       }
    }

    public void LevelUp(float damage, int count, float magicCircleWait)
    {
        this.damage = damage;
        this.count = count;
        this.magicCircleWait = magicCircleWait;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyAccessory", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    
    {
<<<<<<< Updated upstream
        switch (id)
        {
            case 0:
                speed = 150; 
                Batch();
                break;
            case 1:
                speed = 0.3f;
                break;
=======
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = (int)data.baseCount;
        magicCircleWait = data.baseMagicCircleWait;

        for(int index=0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // ���� ID�� ���� ������ �и�
        switch (id)
        {
            // ��
            case 0:
                speed = 150; // ����� �ð����
                Batch();
                break;
            // �ڵ�����
            case 1:
                speed = 0.3f;
                break;
            // �ٶ󺸴� ���� ����
>>>>>>> Stashed changes
            case 2:
                speed = 0.3f;
                Zoning(Vector3.zero);
                break;
<<<<<<< Updated upstream
            default:
                for (int index = 0; index < count; index++)
                {
                    Vector3 targetPos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
                    Zoning(targetPos);
                }

=======
            // ������ ���� ����
            case 3:
                speed = 1f;
                break;
            // �θ޶�
            case 4:
                speed = 1f;
                break;
            // ���� ������
            case 5:
                speed = 5f;
                break;
            // ���� ������
            case 6:
                speed = 5f;
>>>>>>> Stashed changes
                break;
        }

        player.BroadcastMessage("ApplyAccessory", SendMessageOptions.DontRequireReceiver);
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


<<<<<<< Updated upstream
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
=======
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -100�� ������ �����Ѵٴ� �ǹ̷� �ξ���
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
=======

        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        
    }
=======
        for (int index = 0; index < count; index++)
        {
            
            // ������ ������Ʈ�� Transform�� ���������� ����
            Transform bullet ;

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
            
            bullet.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
            bullet.position = transform.position;

            // źȯ�� ��ġ�� ȸ�� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ������ źȯ�� ������ ��ġ�� ��ġ
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            // Translate �Լ��� �ڽ��� �������� �̵�, �̵� ������ Space.World ��������
            bullet.Translate(bullet.up * 1.5f, Space.World);


            // ��ġ�� �Ϸ�Ǿ����Ƿ� �� �̻� źȯ�� �÷��̾ ���� �ʿ䰡 ����
            bullet.parent = GameObject.Find("PoolManager").transform; // �ٽ� poolManager�� �θ� ����



            // źȯ�� �ٶ󺸴� �������� �߻�
            Quaternion.FromToRotation(Vector3.up, bullet.transform.up);
            bullet.GetComponent<Bullet>().Init(damage, count, bullet.transform.up);


        }
        
    }
    // boomerang
    void Fire4() {
        if (!player.scanner.nearestTarget)
            return;
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        // FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�

        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        bullet.GetComponent<Bullet>().Init(damage, -100, dir); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
    }


    void Fire5()
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
                // �� ������ źȯ�� �θ�� PoolManager�̴�. �� ������ źȯ�� �÷��̾ ���󰡾� �ϹǷ� �θ� �÷��̾��� �ڽ� ������Ʈ�� Weapon 7�� �ٲ�� �Ѵ�. 
                bullet.parent = GameObject.Find("Weapon7").transform; ; // parent �Ӽ��� ���� �θ� ����
            }

            bullet.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
            bullet.position = transform.position;

            // źȯ�� ��ġ�� ȸ�� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ������ źȯ�� ������ ��ġ�� ��ġ
            Vector3 rotVec = Vector3.forward * 360 * Random.Range(0f, 30f);
            //index / count;
            bullet.Rotate(rotVec);
            // Translate �Լ��� �ڽ��� �������� �̵�, �̵� ������ Space.World ��������
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.Rotate(rotVec);


            // ��ġ�� �Ϸ�Ǿ����Ƿ� �� �̻� źȯ�� �÷��̾ ���� �ʿ䰡 ����
            bullet.parent = GameObject.Find("PoolManager").transform; // �ٽ� poolManager�� �θ� ����



            bullet.GetComponent<Bullet>().Init(damage, -90, Vector3.zero); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
            StartCoroutine(Disable(magicCircleWait, bullet));

        }
    }

    IEnumerator Disable(float duration, Transform bullet)
    {
        yield return new WaitForSeconds(duration);
        bullet.localScale = Vector3.zero;
    }

>>>>>>> Stashed changes
}
