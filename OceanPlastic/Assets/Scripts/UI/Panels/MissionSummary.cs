using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionSummary : MonoBehaviour
{
    private GameObject root;
    private RectTransform rect;
    
    [SerializeField]
    private Vector2 openPosition;
    [SerializeField]
    private Vector2 closedPosition;
    
    [SerializeField]
    private GameObject entriesParent;
    [SerializeField]
    private GameObject sellItemEntryPrefab;

    [SerializeField] 
    private TextMeshProUGUI totalMoneyText;

    private int totalMoneyGained = 0;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        root = transform.gameObject;
        rect.DOAnchorPos(closedPosition, 0.75f).SetEase(Ease.OutQuad).OnComplete(
            () => root.SetActive(false)
        );
    }
    
    public void ShowMissionSummary()
    {
        root.SetActive(true);
        rect.DOAnchorPos(openPosition, 0.75f).SetEase(Ease.OutQuad);
        PopulateMissionSummary();
    }
    
    private void PopulateMissionSummary()
    {
        foreach (Transform child in entriesParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        Objectives objectives = FindAnyObjectByType<Objectives>();
        var colletionObjectives = objectives.objectiveEntries;

        int pipeValue = FindAnyObjectByType<LeakingPipe>().GetValue();
        
        foreach (var item in colletionObjectives)
        {
            var entry = Instantiate(sellItemEntryPrefab, entriesParent.transform);
            entry.transform.SetParent(entriesParent.transform);
            if (item.type == ObjectiveEntryType.Trash)
            {
                int total = item.currentValue * item.trash.value;
                totalMoneyGained += total;
                entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                    item.spriteImage.sprite,
                    item.trashName,
                    item.currentValue,
                    item.trash.value,
                    total
                );
            }

            if (item.type == ObjectiveEntryType.Pipe)
            {
                int total = item.currentValue * pipeValue;
                totalMoneyGained += total;
                entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                    item.spriteImage.sprite, 
                    "Pipe Repaired", 
                    item.currentValue, 
                    pipeValue, 
                    total
                );
            }
        }
        totalMoneyText.text = $"Total Money: {totalMoneyGained}";
    }

    public void ToTheHub()
    {
        StaticGameData.instance.money += totalMoneyGained;
        SceneManager.LoadScene("TheHub");
    }
}
