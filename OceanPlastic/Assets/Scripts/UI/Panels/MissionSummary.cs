using System.Collections.Generic;
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
    
    private List<(FloatingTrashSO, int)> allTrashCollected = new List<(FloatingTrashSO, int)>();
    
    private void Awake()
    {
        FindAnyObjectByType<BoatSellZone>().onSellTrash.AddListener(AddCollectedTrash);
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

    public void HideMissionSummary()
    {
        rect.DOAnchorPos(closedPosition, 0.75f).SetEase(Ease.InQuad).OnComplete(
            () => root.SetActive(true));
    }

    public void AddCollectedTrash(FloatingTrashSO trash)
    {
        var found = allTrashCollected.Find(item => item.Item1.name == trash.name);
        if (found != default)
        {
            allTrashCollected.Remove(found);
            allTrashCollected.Add((trash, found.Item2 + 1));
        }
        else
        {
            allTrashCollected.Add((trash, 1));
        }
    }
    
    private void PopulateMissionSummary()
    {
        totalMoneyGained = 0;
        foreach (Transform child in entriesParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        Objectives objectives = FindAnyObjectByType<Objectives>();
        var colletionObjectives = objectives.objectiveEntries;

        var pipe = FindAnyObjectByType<LeakingPipe>();
        int pipeValue = 1;
        if (pipe is not null)
        {
            pipeValue = pipe.GetValue();
        }
        
        foreach (var trashCollected in allTrashCollected)
        {
            var entry = Instantiate(sellItemEntryPrefab, entriesParent.transform);
            entry.transform.SetParent(entriesParent.transform);
            entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                trashCollected.Item1.spriteVariants[0], 
                TrashColor.MaterialToColor(trashCollected.Item1.materialType),
                trashCollected.Item1.name, 
                trashCollected.Item2, 
                trashCollected.Item1.value, 
                trashCollected.Item2 * trashCollected.Item1.value
            );
            
            totalMoneyGained += trashCollected.Item2 * trashCollected.Item1.value;
        }
        
        foreach (var item in colletionObjectives)
        {
            if (item.type == ObjectiveEntryType.Pipe)
            {
                var entry = Instantiate(sellItemEntryPrefab, entriesParent.transform);
                entry.transform.SetParent(entriesParent.transform);
                int total = item.currentValue * pipeValue;
                totalMoneyGained += total;
                entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                    item.spriteImage.sprite, 
                    Color.black,
                    "Pipe Repaired", 
                    item.currentValue, 
                    pipeValue, 
                    total
                );
            }
        }
        totalMoneyText.text = $"Total Money: {totalMoneyGained}";
    }

    public void FinishLevelAddMoney()
    {
        StaticGameData.instance.money += totalMoneyGained;
    }
}
