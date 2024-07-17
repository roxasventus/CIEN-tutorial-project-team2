using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기 ID, 프리펩 ID, 데미지, 개수, 속도
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    Vector3 dir;

    float timer;
    Player player;

    private void Awake()
    {
        // GetComponentInParent 함수로 부모의 컴포넌트 가져오기
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 무기 ID에 따라 로직을 분리
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back이 시계방향
                break;
            case 1:
                timer += Time.deltaTime;

                // speed 보다 커지면 초기화하면서 발사 로직 실행
                // speed 값은 연사속도를 의미: 적을 수록 많이 발사
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
            case 2:
                timer += Time.deltaTime;

                // speed 보다 커지면 초기화하면서 발사 로직 실행
                // speed 값은 연사속도를 의미: 적을 수록 많이 발사
                if (timer > speed && (player.inputVec.x != 0 || player.inputVec.y != 0))
                {
                    timer = 0f;
                    Fire2();
                }
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
        // 무기 ID에 따라 로직을 분리
        switch (id)
        {
            // 삽
            case 0:
                speed = 150; // 양수가 시계방향
                Batch();
                break;
            // 총탄
            case 1:
                speed = 0.3f;
                break;
            case 2:
                speed = 1f;
                break;
        }
    }

    // 생성된 무기를 배치하는 함수
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {

            // 가져온 오브젝트의 Transform을 지역변수로 저장
            Transform bullet;



            // 기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
            if (index < transform.childCount)             // 자신의 자식 오브젝트 개수 확인은 childCount 속성에서
            {
                // index가 아직 childCount 범위 내라면 GetChild 함수로 가져오기
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // 갓 생성된 탄환의 부모는 PoolManager이다. 갓 생성된 탄환은 플레이어를 따라가야 하므로 부모를 플레이어의 자식 오브젝트인 Weapon 0로 바꿔야 한다. 
                bullet.parent = transform; // parent 속성을 통해 부모 변경
            }

            // 탄환의 위치와 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 생성된 탄환을 적절한 위치에 배치
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            // Translate 함수로 자신의 위쪽으로 이동, 이동 방향은 Space.World 기준으로
            bullet.Translate(bullet.up * 1.5f, Space.World);


            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -1은 무한히 관통한다는 의미로 두었다
        }
    }
    // auto targeting
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        // FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수

        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -1은 무한히 관통한다는 의미로 두었다
    }
    // present direction targeting
    void Fire2()
    {

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
        bullet.position = transform.position;

        dir = new Vector3(player.inputVec.x, player.inputVec.y, 0);

        if (dir.x != 0)
        {
            bullet.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
        if (dir.y != 0)
        {
            bullet.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }

    }
}
