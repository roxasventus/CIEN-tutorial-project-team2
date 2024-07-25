using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { experience, level, health, time }
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            // Experience Bar - Background and fill is not set in the inspector, need to be set in the sprite asset
            case InfoType.experience:
                int exp = GameManager.instance.exp;
                int nextExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = (float)exp / nextExp;
                break;

            case InfoType.level:
                myText.text = string.Format("Level: {0:F0}", GameManager.instance.level);   
                break;

            case InfoType.health:
                int health = (int)GameManager.instance.health;
                int maxHealth = (int)GameManager.instance.maxHealth;
                mySlider.value = (float)health / maxHealth;
                break;

            case InfoType.time:
                int minute = (int)GameManager.instance.gameTime / 60;
                int second = (int)GameManager.instance.gameTime % 60;
                myText.text = string.Format("{0:D2}:{1:D2}", minute, second);
                break;
        }
    }
}
