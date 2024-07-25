using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpInterface : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.GetExp();
        }
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    void Next(){
        // deactive all items
        foreach (Item item in items){
            item.gameObject.SetActive(false);
        }

        // active randam 3 items (no duplicate)
        int[] rand = new int[3];
        while (true){
            rand[0] = Random.Range(0, items.Length);
            rand[1] = Random.Range(0, items.Length);
            rand[2] = Random.Range(0, items.Length);

            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[2] != rand[0]){
                break;
            }
        }

        for (int i = 0; i < 3; i++){
            Item ranItem = items[rand[i]];
            
            // case of maximum item level - test logic by Jaewon 
            // move to next index, if the index is duplicated, repeat
            while (ranItem.level == ranItem.data.damages.Length){
                rand[i] += 1;
                // if the index is out of range, set it to 0
                if (rand[i] == ranItem.data.damages.Length){
                    rand[i] = 0;
                }

                // if index is duplicated, continue
                if (rand[i] == rand[(i+1)%3] || rand[i] == rand[(i+2)%3]){
                    continue;
                }

                // if new item is not maximum level, break
                ranItem = items[rand[i]];
            }

            ranItem.gameObject.SetActive(true);
        }
    }
}
