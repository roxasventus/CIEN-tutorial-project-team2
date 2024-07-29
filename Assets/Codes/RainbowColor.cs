using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    public float speed = 1.0f; // ���� ��ȭ �ӵ�
    private Renderer objRenderer;
    private float hue;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        hue = 0;
    }

    void Update()
    {
        // ���� ��ȭ
        hue += Time.deltaTime * speed;
        if (hue > 1) hue -= 1; // 0~1 ������ ����

        Color color = Color.HSVToRGB(hue, 1, 1); // ä���� ���� ����
        objRenderer.material.color = color;
    }
}
