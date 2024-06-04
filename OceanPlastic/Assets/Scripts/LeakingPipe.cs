using System.Collections;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeakingPipe : MonoBehaviour
{
    [SerializeField]
    PipeCracks pipeCracks;

    [SerializeField] 
    private GameObject crack;
    
    bool isPlayerInside = false;
    
    [SerializeField]
    float timeToRepair = 2f;

    [SerializeField]
    private int value = 10;

    [SerializeField] 
    private GameObject repairMinigamePrefab;
    
    private GameObject repairMinigame;

    private float repairProgress = 0f;
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;
    private ToolButtons toolButtons;
    
    private Coroutine repairCoroutine;
    private float timeStep;

    private bool isRepaired = false;
    private bool repairStarted = false;

    private LevelManager levelManager;
    private GameUIController gameUIController;

    private void Start()
    {
        spriteRenderer = crack.GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        pipeCracks.gameObject.SetActive(false);
        toolButtons = FindObjectOfType<ToolButtons>();
        timeStep = (1f / timeToRepair) / 100f;
        levelManager = FindObjectOfType<LevelManager>();
        
        gameUIController = FindObjectOfType<GameUIController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRepaired) return;
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerInside = true;
                toolButtons.EnableExtraButton();
                toolButtons.SetTooltip("Tap to start repairing.");
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isRepaired) return;
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerInside = false;
                toolButtons.DisableExtraButton();
                toolButtons.ClearTooltip();
            }
        }
    }

    public void StartRepair()
    {
        if (!isPlayerInside) return;
        if (isRepaired || repairStarted) return;
        repairStarted = true;
        gameUIController.MoveAside();
        repairMinigame = Instantiate(repairMinigamePrefab, Vector3.zero, Quaternion.identity);
        repairMinigame.transform.parent = Camera.main.transform;
        repairMinigame.transform.localPosition = new Vector3(0f, -12f, 1f);
        repairMinigame.transform.DOLocalMove(Vector3.forward, 1f).SetEase(Ease.OutQuad).OnComplete(
            () => Time.timeScale = 0f);
        repairMinigame.GetComponent<RepairMinigame>().onFinished.AddListener(MinigameCompleted);
        
    }
    
    public void StopRepair()
    {
        if (isRepaired) return;
        
        // if (repairCoroutine != null)
        // {
        //     StopCoroutine(repairCoroutine);
        // }
        // pipeCracks.gameObject.SetActive(false);
    }

    IEnumerator Repair()
    {
        while (isPlayerInside && repairProgress < 1f)
        {
            pipeCracks.gameObject.SetActive(true);
            repairProgress += timeStep;
            repairProgress = Mathf.Clamp01(repairProgress);
            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_FixValue", repairProgress);
            spriteRenderer.SetPropertyBlock(propertyBlock);
            pipeCracks.moveValue = repairProgress;
            if (repairProgress >= 1f)
            {
                pipeCracks.gameObject.SetActive(false);
                toolButtons.DisableExtraButton();
                isRepaired = true;
                levelManager.PipeRepaired();
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public int GetValue()
    {
        return value;
    }

    private void MinigameCompleted()
    {
        Time.timeScale = 1f;
        toolButtons.DisableExtraButton();
        isRepaired = true;
        levelManager.PipeRepaired();
        gameUIController.MoveToNormal();
        crack.SetActive(false);
        repairMinigame.transform.DOLocalMove(new Vector3(0f, -12f, 1f), 1f)
            .OnComplete( 
                () => Destroy(repairMinigame)
            );
    }
}
