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
        if (StaticLevelData.isInLevel)
        {
            //waterTransition.rectTransform.anchoredPosition = new Vector2(-1300, 250f);
            waterTransition.rectTransform.DOAnchorPos(new Vector2(-1300, 250f), 0f);
            waterTransition.rectTransform.DOAnchorPos(new Vector2(200f, -1200f), 2f);
            return;
        }

        if (StaticGameData.instance.justReturnedFromMission)
        {
            waterTransition.rectTransform.DOAnchorPos(new Vector2(-1300, 250f), 0f);
            waterTransition.rectTransform.DOAnchorPos(new Vector2(200f, -1200f), 2f);
            StaticGameData.instance.justReturnedFromMission = false;
        }
    }

    public void FromHubToLevel(string sceneName)
    {
        StaticLevelData.isInLevel = true;
        waterTransition.rectTransform.DOAnchorPos(new Vector2(-100, 40f), 2f)
            .OnComplete(() => SceneManager.LoadScene(sceneName));
    }

    public void FromLevelToHub()
    {
        StaticLevelData.isInLevel = false;
        waterTransition.rectTransform.DOAnchorPos(new Vector2(200f, -1200f), 0f);
        waterTransition.rectTransform.DOAnchorPos(new Vector2(-1300, 250f), 2f).OnComplete(
            () =>
            {
                SceneManager.LoadScene("TheHub");
                StaticGameData.instance.justReturnedFromMission = true;
            });
        
    }
}
