using System.Collections;
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

    private float repairProgress = 0f;
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;
    private ToolButtons toolButtons;
    
    private Coroutine repairCoroutine;
    private float timeStep;

    private bool isRepaired = false;
    
    private LevelManager levelManager;

    private void Start()
    {
        spriteRenderer = crack.GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        pipeCracks.gameObject.SetActive(false);
        toolButtons = FindObjectOfType<ToolButtons>();
        timeStep = (1f / timeToRepair) / 100f;
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRepaired) return;
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerInside = true;
                toolButtons.EnableRepairButton();
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
                toolButtons.DisableRepairButton();
            }
        }
    }

    public void StartRepair()
    {
        if (isRepaired) return;
        repairCoroutine = StartCoroutine(Repair());
    }
    
    public void StopRepair()
    {
        if (isRepaired) return;
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
        }
        pipeCracks.gameObject.SetActive(false);
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
                toolButtons.DisableRepairButton();
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
}
