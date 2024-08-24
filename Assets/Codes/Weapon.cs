using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    // 무기 ID, 프리펩 ID, 데미지, 개수, 속도
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    // 마법진 유지 시간
    public float magicCircleWait;

    Vector3 dir;

    float timer;
    Player player;

    private void Awake()
    {
        // GetComponentInParent 함수로 부모의 컴포넌트 가져오기
        //player = GetComponentInParent<Player>();
        player = GameManager.instance.player;

    }
    /*
    private void Start()
    {
        Init();
    }
    */
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // 무기 ID에 따라 로직을 분리
        switch (id)
        {
            // 삽
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back이 시계방향
                break;
            // 자동조준
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
            // 바라보는 방향 조준
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
            // 고정된 방향 조준
            case 3:
                timer += Time.deltaTime;

                // speed 보다 커지면 초기화하면서 발사 로직 실행
                // speed 값은 연사속도를 의미: 적을 수록 많이 발사
                if (timer > speed)
                {
                    timer = 0f;
                    Fire3();
                }
                break;
            // 부메랑
            case 4:
                timer += Time.deltaTime;

                // speed 보다 커지면 초기화하면서 발사 로직 실행
                // speed 값은 연사속도를 의미: 적을 수록 많이 발사
                if (timer > speed)
                {
                    timer = 0f;
                    Fire4();
                }
                break;
            // 고정 마법진
            case 5:
                GameObject bullet = GameManager.instance.pool.prefabs[prefabId];
                timer += Time.deltaTime;

                // 마법진 쿨타임
                if (timer > speed && bullet.activeSelf == false)
                {
                    timer = 0f;
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.final_attack);
                    bullet.SetActive(!bullet.activeSelf);
                }
                // 마법진 유지시간
                if (timer > magicCircleWait && bullet.activeSelf == true)
                {
                    timer = 0f;
                    bullet.SetActive(!bullet.activeSelf);
                }

                break;
            // 랜덤 마법진
            case 6:
                timer += Time.deltaTime;

                // speed 보다 커지면 초기화하면서 발사 로직 실행
                // speed 값은 연사속도를 의미: 적을 수록 많이 발사
                if (timer > speed)
                {
                    timer = 0f;
                    Fire5();
                }
                break;
        }

    }

    public void LevelUp(float damage, int count, float magicCircleWait)
    {
        this.damage = damage;
        this.count += count;
        this.magicCircleWait += magicCircleWait;

        if (id == 0)
            Batch();
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        // 부모 오브젝트를 플레이어로 지정
        transform.parent = player.transform;
        // 지역 위치인 localPosition을 원점으로 변경
        transform.localPosition = Vector3.zero;

        // Property Set
        // 각종 무기 속성 변수들을 스크립트블 오브젝트 데이터로 초기화
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        magicCircleWait = data.baseMagicCircleWait;



        // 고정 장판 활성화
        if (id == 5)
            prefabId = 6;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }


        // 무기 ID에 따라 로직을 분리
        switch (id)
        {
            // 삽
            case 0:
                speed = 150; // 양수가 시계방향
                Batch();
                break;
            // 자동조준
            case 1:
                speed = 0.7f;
                break;
            // 바라보는 방향 조준
            case 2:
                speed = 1f;
                break;
            // 고정된 방향 조준
            case 3:
                speed = 1.5f;
                break;
            // 부메랑
            case 4:
                speed = 1f;
                break;
            // 고정 마법진
            case 5:
                speed = 3f;
                break;
            // 랜덤 마법진
            case 6:
                speed = 3f;
                break;
        }
        // 나중에 추가된 무기도 레벨업 된 장비의 영향을 받아야 한다
        // BroadcastMessage: 특정 함수호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

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


            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -100은 무한히 관통한다는 의미로 두었다
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
        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.attack1);
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
        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.attack2);

    }
    // Fixed direction targeting
    void Fire3() {

        for (int index = 0; index < count; index++)
        {
            
            // 가져온 오브젝트의 Transform을 지역변수로 저장
            Transform bullet ;

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
            
            bullet.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
            bullet.position = transform.position;

            // 탄환의 위치와 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 생성된 탄환을 적절한 위치에 배치
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            // Translate 함수로 자신의 위쪽으로 이동, 이동 방향은 Space.World 기준으로
            bullet.Translate(bullet.up * 1.5f, Space.World);


            // 배치가 완료되었으므로 더 이상 탄환이 플레이어를 따라갈 필요가 없다
            bullet.parent = GameObject.Find("PoolManager").transform; // 다시 poolManager로 부모 변경



            // 탄환이 바라보는 방향으로 발사
            Quaternion.FromToRotation(Vector3.up, bullet.transform.up);
            bullet.GetComponent<Bullet>().Init(damage, count, bullet.transform.up);
            // 효과음 재생할 부분마다 재생함수 호출
            AudioManager.instance.PlaySfx(AudioManager.Sfx.attack3);


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
        // FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수

        bullet.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        bullet.GetComponent<Bullet>().Init(damage, -100, dir); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -1은 무한히 관통한다는 의미로 두었다

    }


    void Fire5()
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
                // 갓 생성된 탄환의 부모는 PoolManager이다. 갓 생성된 탄환은 플레이어를 따라가야 하므로 부모를 플레이어의 자식 오브젝트인 Weapon 7로 바꿔야 한다. 
                bullet.parent = GameObject.Find("Player").transform; ; // parent 속성을 통해 부모 변경
            }

            bullet.GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
            bullet.position = transform.position;

            // 탄환의 위치와 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 생성된 탄환을 적절한 위치에 배치
            Vector3 rotVec = Vector3.forward * 360 * Random.Range(0f, 30f);
            //index / count;
            bullet.Rotate(rotVec);
            // Translate 함수로 자신의 위쪽으로 이동, 이동 방향은 Space.World 기준으로
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.Rotate(rotVec);


            // 배치가 완료되었으므로 더 이상 탄환이 플레이어를 따라갈 필요가 없다
            bullet.parent = GameObject.Find("PoolManager").transform; // 다시 poolManager로 부모 변경



            bullet.GetComponent<Bullet>().Init(damage, -90, Vector3.zero); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -1은 무한히 관통한다는 의미로 두었다

            StartCoroutine(Disable(magicCircleWait, bullet));

        }
    }

    IEnumerator Disable(float duration, Transform bullet)
    {
        if(bullet.localScale != Vector3.zero)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.final_attack);
        yield return new WaitForSeconds(duration);
        bullet.localScale = Vector3.zero;
    }

}
