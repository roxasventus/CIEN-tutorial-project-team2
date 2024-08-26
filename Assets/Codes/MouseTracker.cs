using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    void Update()
    {
        RotateTowards();
    }

    void RotateTowards()
    {
        // ���콺 ��ġ ��������
        Vector3 mousePosition = Input.mousePosition;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // ������Ʈ ��ġ�� ���콺 ��ġ ���� ���� ���
        Vector3 direction = mousePosition - transform.position;

        // ���� ���͸� ������ ��ȯ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������Ʈ ȸ��
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
