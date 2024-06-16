using System.Collections;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeakingPipe : MinigameTrigger
{
   [SerializeField] 
    private GameObject crack;
    
    [SerializeField]
    float timeToRepair = 2f;

    [SerializeField]
    private int value = 10;

    private float repairProgress = 0f;
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;
    
    private Coroutine repairCoroutine;

    public GameObject particles;
    
    private void Start()
    {
        MinigameTriggerInit();
        onWin.AddListener(OnRepairFinished);
    }

    public void OnRepairFinished()
    {
        particles.SetActive(false);
        crack.SetActive(false);
    }

    public int GetValue()
    {
        return value;
    }
}
