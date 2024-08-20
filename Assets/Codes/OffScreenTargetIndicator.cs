using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenTargetIndicator : MonoBehaviour
{
    public GameObject[] targets;
    public GameObject indicatorPrefab;

    private SpriteRenderer spriter;
    private float spriteWidth;
    private float spriteHeight;

    private Camera _camera;

    private Dictionary<GameObject, GameObject> targetIndicators = new Dictionary<GameObject, GameObject>();

    private void Start()
    {
        _camera = Camera.main;
        spriter = indicatorPrefab.GetComponent<SpriteRenderer>();


        Bounds bounds = spriter.bounds;
        spriteHeight = bounds.size.y / 2f;
        spriteWidth = bounds.size.x / 2f;

        foreach(GameObject target in targets)
        {
            GameObject indicator = Instantiate(indicatorPrefab, transform);
            targetIndicators.Add(target, indicator);
        }
    }

    public void AddTarget(GameObject target)
    {
        GameObject indicator = Instantiate(indicatorPrefab, transform);
        targetIndicators.Add(target, indicator);
    }

    public void RemoveTarget(GameObject target)
    {
        targetIndicators.Remove(target);
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        foreach(KeyValuePair<GameObject, GameObject> pair in targetIndicators)
        {
            GameObject target = pair.Key;
            GameObject indicator = pair.Value;

            UpdateTarget(target, indicator);
        }
    }

    private void UpdateTarget(GameObject target, GameObject indicator)
    {
        //calculate the indicator's position based on viewport space

        Vector3 screenPos = _camera.WorldToViewportPoint(target.transform.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= 1 || screenPos.y <= 0 || screenPos.y >= 1;
        if(isOffScreen)
        {
            indicator.SetActive(true);
            Vector3 spriteSizeInViewPort = _camera.WorldToViewportPoint(new Vector3(spriteWidth, spriteHeight, 0)) - _camera.WorldToViewportPoint(Vector3.zero);

            screenPos.x = Mathf.Clamp(screenPos.x, spriteSizeInViewPort.x, 1 - spriteSizeInViewPort.x); //clamp : return the value between max and min
            screenPos.y = Mathf.Clamp(screenPos.y, spriteSizeInViewPort.y, 1 - spriteSizeInViewPort.y); 

            Vector3 worldPosition = _camera.ViewportToWorldPoint(screenPos);
            worldPosition.z = 0;
            indicator.transform.position = worldPosition;

            Vector3 dir = target.transform.position - indicator.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
