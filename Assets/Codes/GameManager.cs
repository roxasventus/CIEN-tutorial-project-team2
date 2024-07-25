using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ��� ����

public class GameManager : MonoBehaviour
{
    // static: �������� ����ϰڴٴ� Ű����. �ٷ� �޸𸮿� ������. � �ν��Ͻ��������� ���� ����
    // static���� ����� ������ �ν����Ϳ� ��Ÿ���� �ʴ´�
    public static GameManager instance;

    // Header: �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
    [Header("# Game control")]
    // �ð� ���� ���θ� �˷��ִ� bool ���� ����
    public bool isLive;
    // ���� ���� �ð�
    public float gameTime;
    // ���� ���� �ð�
    public float maxGameTime = 2 * 10f;
    [Header("# Player Info")]
    // �� ������ �ʿ����ġ�� ������ �迭 ���� ���� �� �ʱ�ȭ
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUpInterface uiLevelUp;

    // ���� ��� UI ������Ʈ�� ������ ���� ���� �� �ʱ�ȭ
    //public Result uiResult;
    // ���� �¸��� �� ���� �����ϴ� Ŭ���� ���� ���� �� �ʱ�ȭ
    public GameObject enemyCleaner;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        // ���� ������ �� �÷��̾� Ȱ��ȭ �� �⺻ ���� ����
        player.gameObject.SetActive(true);
        uiLevelUp.Show();
        Resume();

        // ȿ���� ����� �κи��� ����Լ� ȣ��
        //AudioManager.instance.PlayBgm(true);
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

        //uiResult.gameObject.SetActive(true);
        
        //uiResult.Lose();
        Stop();

        // ȿ���� ����� �κи��� ����Լ� ȣ��
        //AudioManager.instance.PlayBgm(false);
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        // ���� �¸� �ڷ�ƾ�� ���ݺο� �� Ŭ���ʸ� Ȱ��ȭ
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //uiResult.gameObject.SetActive(true);

        //uiResult.Win();
        Stop();

        // ȿ���� ����� �κи��� ����Լ� ȣ��
        //AudioManager.instance.PlayBgm(false);
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // �����
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {   if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

    }
    // ����ġ ���� �Լ�
    public void GetExp()
    {
        if (!isLive)
        {
            return; 
        }
        exp++;
        // if �������� �ʿ� ����ġ�� �����ϸ� ���� ���ϵ��� �ۼ�
        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) { // ���� �������� ���Ͽ� Min �Լ��� ����Ͽ� �ְ� ����ġ�� �״�� ��� ����ϵ��� ����
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        // timeScale ����Ƽ�� �ð� �ӵ�(����)
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        // timeScale ����Ƽ�� �ð� �ӵ�(����)
        Time.timeScale = 1;
    }
}
