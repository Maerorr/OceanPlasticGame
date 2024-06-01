using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionEffect : MonoBehaviour
{
    private Image waterTransition;
    
    private void Start()
    {
        waterTransition = GetComponent<Image>();
        Debug.Log(waterTransition.rectTransform.position);
        if (StaticLevelData.isInLevel)
        {
            Debug.Log("IS IN LEVEL");
            waterTransition.rectTransform.DOAnchorPos(new Vector2(-1300, 170f), 0f);
            waterTransition.rectTransform.DOAnchorPos(new Vector2(200f, -1200f), 2f);
        }
    }

    public void FromHubToLevel(string sceneName)
    {
        StaticLevelData.isInLevel = true;
        waterTransition.rectTransform.DOAnchorPos(new Vector2(-100, 40f), 2f)
            .OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
