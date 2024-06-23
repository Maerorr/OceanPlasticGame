using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    // (trash type, amount, isCompleted)
    public List<(MaterialType, int, bool)> collectionObjective;
    private int allRepairObjectives;
    private int currentRepairObjectives;
    int allTrashCount;
    
    private Objectives objectives;
    private GameObject finishLevelArea;

    private void Start()
    {
        finishLevelArea = FindObjectOfType<FinishLevel>().transform.parent.gameObject;
        finishLevelArea.SetActive(false);
        FindAnyObjectByType<BoatSellZone>().onSellTrash.AddListener(TrashCollected);
        
        objectives = FindObjectOfType<Objectives>();
        var allTrash = FindObjectsOfType<FloatingTrash>();
        allTrashCount = allTrash.Length;

        var trashTypes = new HashSet<MaterialType>();
        foreach (var trash in allTrash)
        {
            trashTypes.Add(trash.GetData().materialType);
        }
        
        collectionObjective = new List<(MaterialType, int, bool)>();
        foreach (var trashType in trashTypes)
        {
            int allTrashOfType = allTrash.Sum(trash => trash.GetData().materialType == trashType ? 1 : 0);
            collectionObjective.Add(
                (trashType, Random.Range(
                    allTrashOfType / 2,
                    Mathf.FloorToInt(allTrashOfType * 0.9f)),
                    false
                )
            );
        }
        
        foreach (var obj in collectionObjective)
        {
            var firstTrashOfType = allTrash.First(trash => trash.GetData().materialType == obj.Item1);
            objectives.AddTrashObjective(firstTrashOfType.GetData(), obj.Item2);
        }
        
        
        // get all unique trash types
        /*
        var trashTypes = new HashSet<FloatingTrashSO>();
        foreach (var trash in allTrash)
        {
            trashTypes.Add(trash.GetData());
        }
        collectionObjective = new List<(FloatingTrashSO, int, bool)>();
        foreach (var trashType in trashTypes)
        {
            int allTrashOfType = allTrash.Sum(trash => trash.GetData().name == trashType.name ? 1 : 0);
            collectionObjective.Add(
                (trashType, Random.Range(
                    allTrashOfType / 2,
                    Mathf.FloorToInt(allTrashOfType * 0.9f)),
                    false
                )
            );
        }

        foreach (var obj in collectionObjective)
        {
            objectives.AddTrashObjective(obj.Item1, obj.Item2);
        }
        */
        if (StaticGameData.instance.inTutorial) return;
        var allPipes = FindObjectsOfType<LeakingPipe>();
        allRepairObjectives = allPipes.Length;
        objectives.AddRepairObjectives(allRepairObjectives);
        currentRepairObjectives = 0;
    }

    public void AddCollectionObjective(FloatingTrashSO trash)
    {
        collectionObjective.Add(
            (trash.materialType, 1, false)
        );
    }
    
    public void TrashCollected(FloatingTrashSO trash)
    {
        objectives ??= FindAnyObjectByType<Objectives>();
        bool isCompleted = objectives.UpdateTrashObjectives(trash);
        if (isCompleted)
        {
            collectionObjective
            [collectionObjective.FindIndex(
                entry => entry.Item1 == trash.materialType)] = (trash.materialType, 0, true);
        }
        
        if (CheckIfAllObjectivesCompleted())
        {
            finishLevelArea.SetActive(true);
        }
    }

    public void PipeRepaired()
    {
        currentRepairObjectives++;
        objectives.UpdateRepairObjective();
        if (CheckIfAllObjectivesCompleted())
        {
            finishLevelArea.SetActive(true);
        }
    }
    
    private bool CheckIfAllObjectivesCompleted()
    {
        return collectionObjective.All(entry => entry.Item3) && currentRepairObjectives == allRepairObjectives;
    }
}