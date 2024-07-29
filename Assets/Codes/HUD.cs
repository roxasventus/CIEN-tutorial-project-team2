using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// UI 컴포넌트를 사용할 때는 UnityEngine.UI 네임스페이스 사용
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 다루게 될 데이터를 미리 열거형 enum으로 선언
    public enum InfoType { Exp, Level, Kill, Time, Health }
    // 선언한 열거형을 타입으로 변수 추가 -> 유니티의 인스펙터 창 내에서 InfoType 열거형 값들 중 하나로 지정 가능
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                if (mySlider.value == 0 && GameObject.Find("LevelUp").transform.localScale == Vector3.one)
                {
                    mySlider.value = 1;
                }
                break;
            case InfoType.Level:
                // Format: 각 숫자 인자값을 지정된 형태의 문자열로 만들어주는 함수 
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); // string.Format(포맷 타입, 적용되는 데이터)
                                                                                      // 인자 값의 문자열이 들어갈 자리를 {순번:적용되는 데이터에 대한 포맷} 형태로 작성
                                                                                      // F0, F1, F2.... 소수점 자리를 지정
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec); // D0, D1, D2.... 자리 수를 지정
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
