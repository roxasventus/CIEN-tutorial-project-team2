using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        Vector3 playerPos = GameManager.instance.player.transform.position;
        rect.position = Camera.main.WorldToScreenPoint(playerPos - new Vector3(0f, 0.7f, 0f));
    }
}
