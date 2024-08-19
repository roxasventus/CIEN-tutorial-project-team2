using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    public static GameManager instance;

    
    [Header("# Game control")]
    public bool isLive;
    public bool isStageClear;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public float stageTime; //duration of a stage -sw
    public int maxStageNum = 3; // max number of stages -sw
    public int stageNum = 0; //current stage -sw

    [Header("# Player Info")]
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
    public Result uiResult;
    public GameObject enemyCleaner;
    public GameObject hitEffect;

    public Stage stage; //stage script -sw

    private void Awake()
    {
        instance = this;
        stageTime = maxGameTime / maxStageNum; //initiate stageTime -sw
        maxGameTime = stageTime;

    }

    void Start()
    {
        health = maxHealth;

    }


    public void GameStart(int id)
    {
        stage.ActivateStage();

        playerId = id;
        health = maxHealth;
        player.gameObject.SetActive(true);
       
        uiLevelUp.Selected(playerId);

        Resume();

        
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        //AudioManager.instance.EffectBgm(false);
    }

    public void GameContinue()
    {
        health = maxHealth;
        player.gameObject.SetActive(true);

        isStageClear = false;
        

        // 오브젝트 위치 재조정
        GameManager.instance.player.transform.position = new Vector3(13.42f, 7.37f, 0);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(13.42f, 7.37f, 0);

        // 필드에 생성된 아이템들 삭제
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("BarrierPotion");
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        objectsToDestroy = GameObject.FindGameObjectsWithTag("HealPotion");
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        // 결과창 숨김
        GameObject.Find("GameResult").SetActive(false);
        // 시간 초기화
        GameManager.instance.gameTime = 0;

        enemyCleaner.SetActive(false);
        GameObject.Find("BossSpawner").GetComponent<Spawner>().isBossSpawn = false;


        // 스테이지 바꾸기
        stage.ChangeStage();

        // bgm 바꾸기
        AudioManager.instance.bgmIndex += 1;
        AudioManager.instance.Init();

        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
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
       
        //enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(1f);

        uiResult.gameObject.SetActive(true);

        if (stageNum == maxStageNum-1)
        {
            uiResult.Win();
        }
        else
        {
            uiResult.Continue();
        }
        Stop();

        
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

   
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
            if(isStageClear == false)
                StartCoroutine(CleanEnemy());
            //GameVictory();
        }

    }

    public void GetExp(int point = 1)
    {
        if (!isLive)
        {
            return; 
        }
        exp += point;
        
        if (exp >= nextExp[Mathf.Min(level, nextExp.Length-1)]) { 
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;

        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        // 에디터 모드에서 작동 확인을 위해 에디터도 종료
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // 빌드된 게임에서 작동
                Application.Quit();
        #endif
    }

    IEnumerator CleanEnemy()
    {
        enemyCleaner.SetActive(true);
        isStageClear = true;
        yield return new WaitForSeconds(0.1f);
        enemyCleaner.SetActive(false);
    }
}
