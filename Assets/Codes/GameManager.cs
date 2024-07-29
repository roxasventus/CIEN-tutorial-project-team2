using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 장면 관리

public class GameManager : MonoBehaviour
{
    // static: 정적으로 사용하겠다는 키워드. 바로 메모리에 얹어버림. 어떤 인스턴스에서든지 접근 가능
    // static으로 선언된 변수는 인스펙터에 나타나지 않는다
    public static GameManager instance;

    // Header: 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
    [Header("# Game control")]
    // 시간 정지 여부를 알려주는 bool 변수 선언
    public bool isLive;
    // 게임 진행 시간
    public float gameTime;
    // 게임 종료 시간
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    // 각 레벨의 필요경험치를 보관할 배열 변수 선언 및 초기화
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    public bool isInvincible;
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    // 게임 결과 UI 오브젝트를 저장할 변수 선언 및 초기화
    public Result uiResult;
    // 게임 승리할 때 적을 정리하는 클리너 변수 선언 및 초기화
    public GameObject enemyCleaner;

    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        health = maxHealth;

    }


    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        // 게임 시작할 때 플레이어 활성화 후 기본 무기 지급
        player.gameObject.SetActive(true);
        //uiLevelUp.Selected(playerId % 2);

        // 임시 스크립트 (첫번재 캐릭터 선택)
        uiLevelUp.Selected(0);

        Resume();

        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlayBgm(true);
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        //AudioManager.instance.EffectBgm(false);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    { 
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        
        uiResult.Lose();
        Stop();

        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        // 게임 승리 코루틴의 전반부에 적 클리너를 활성화
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);

        uiResult.Win();
        Stop();

        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // 재시작
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {   
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

    }
    // 경험치 증가 함수
    public void GetExp()
    {
        if (!isLive)
        {
            return; 
        }
        exp++;
        // if 조건으로 필요 경험치에 도달하면 레벨 업하도록 작성
        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) { // 무한 레벨업을 위하여 Min 함수를 사용하여 최고 경험치를 그대로 계속 사용하도록 변경
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        // timeScale 유니티의 시간 속도(배율)
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        // timeScale 유니티의 시간 속도(배율)
        Time.timeScale = 1;
    }
}
