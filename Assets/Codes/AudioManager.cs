using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    // 배경음과 관련된 클립, 볼륨, 오디오소스 변수 선언
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    // 효과음과 관련된 클립, 볼륨, 오디오소스 변수 선언
    public AudioClip[] sfxClips;
    public float sfxVolume;
    // 다량의 효과음을 낼 수 있도록 채널 개수 변수 선언
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform; // 배경음을 담당하는 자식 오브젝트를 생성
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = bgmVolume;
            // 레벨업 UI가 뜰 때, 효과음은 음소거 되지 않도록
            sfxPlayers[index].bypassEffects = true;
        }
    }

    // 배경음을 재생하는 함수 작성
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    // 배경음 필터를 켜고 끄는 함수 작성
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    // 효과음 재생 함수
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            // 맨 마지막에 실행했던 플레이어의 인덱스
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;  // 채널 개수만큼 순회하도록 채널인덱스 변수 활용

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            // 효과음이 2개 이상인 것은 랜덤 인덱스를 더하기
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = UnityEngine.Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            // 효과음 재생이 된 이후에는 꼭 break로 반복문 종료
            break;
        }
    }
}
