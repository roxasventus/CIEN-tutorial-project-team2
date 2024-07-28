using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Stage : MonoBehaviour
{
    public Transform[] stages;

    private void Awake()
    {
        stages = GetComponentsInChildren<Transform>();
    }

    public void DisableStage()
    {
        stages[GameManager.instance.stageNum].gameObject.SetActive(false); //deactivate current stage
    }

    public void ActivateStage()
    {
        stages[GameManager.instance.stageNum].gameObject.SetActive(true); //activate current stage
    }

    public void ChangeStage()
    {
        stages[GameManager.instance.stageNum].gameObject.SetActive(false); //deactivate current stage
        GameManager.instance.stageNum = Mathf.Min(GameManager.instance.stageNum + 1, stages.Length);
        stages[GameManager.instance.stageNum].gameObject.SetActive(true); //activate next stage
    }
}
