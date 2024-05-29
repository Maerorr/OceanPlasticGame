using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    // (trash type, amount, isCompleted)
    private List<(FloatingTrashSO, int, bool)> collectionObjective;
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
        // get all unique trash types
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

        var allPipes = FindObjectsOfType<LeakingPipe>();
        allRepairObjectives = allPipes.Length;
        objectives.AddRepairObjectives(allRepairObjectives);
        currentRepairObjectives = 0;
    }

    public void TrashCollected(FloatingTrashSO trash)
    {
        bool isCompleted = objectives.UpdateTrashObjectives(trash);
        if (isCompleted)
        {
            collectionObjective[collectionObjective.FindIndex(entry => entry.Item1.name == trash.name)] = (trash, 0, true);
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