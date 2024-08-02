using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    public float speed = 1.0f; // 색상 변화 속도
    private Renderer objRenderer;
    private float hue;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        hue = 0;
    }

    void Update()
    {
        // 색상 변화
        hue += Time.deltaTime * speed;
        if (hue > 1) hue -= 1; // 0~1 범위로 제한

        Color color = Color.HSVToRGB(hue, 1, 1); // 채도와 밝기는 고정
        objRenderer.material.color = color;
    }
}
