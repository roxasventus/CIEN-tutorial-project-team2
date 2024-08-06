using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        titles[0].SetActive(true);
        titles[1].SetActive(false);
        titles[3].SetActive(false);
        titles[2].SetActive(true);
    }

    public void Continue() {
        titles[1].SetActive(true);
        titles[0].SetActive(false);
        titles[3].SetActive(true);
        titles[2].SetActive(false);
    }

    public void Win()
    {
        titles[1].SetActive(true);
        titles[0].SetActive(false);
        titles[3].SetActive(false);
        titles[2].SetActive(true);
    }
}
