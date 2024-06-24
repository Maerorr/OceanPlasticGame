using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SellItemsList : MonoBehaviour
{
    private GameObject root;
    [SerializeField]
    private GameObject entriesParent;
    [SerializeField]
    private GameObject sellItemEntryPrefab;

    [SerializeField]
    private Vector2 openPosition;
    [SerializeField]
    private Vector2 closedPosition;
    
    private RectTransform rect;
    
    [SerializeField] 
    private UnityEvent onSell;

    public RectTransform leftShredder;
    public RectTransform rightShredder;
    public float shredderSpeed = 1f;
    public RectTransform trashSpawnPoint;
    public RectTransform machineRect;
    
    public List<Sprite> trashSprites;
    
    private List<GameObject> spawnedTrash = new List<GameObject>();
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        root = transform.gameObject;
        rect.DOAnchorPos(closedPosition, 0.75f).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(
            () => root.SetActive(false)
        );
    }

    private void Update()
    {
        var currentRot = leftShredder.rotation.eulerAngles;
        currentRot.z += Time.deltaTime * shredderSpeed;
        leftShredder.rotation = Quaternion.Euler(currentRot);
        currentRot = rightShredder.rotation.eulerAngles;
        currentRot.z -= Time.deltaTime * shredderSpeed;
        rightShredder.rotation = Quaternion.Euler(currentRot);
    }

    public void ShowSellItemsList()
    {
        root.SetActive(true);
        rect.DOAnchorPos(openPosition, 0.75f).SetUpdate(true).SetEase(Ease.OutQuad);
        PopulateSellItemsList();
        PlayerManager.Instance.Player.SetCanBreathe(true);
    }
    
    public void HideSellItemsList()
    {
        PlayerManager.Instance.Player.SetCanBreathe(false);
        rect.DOAnchorPos(closedPosition, 0.75f).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(
            () => root.SetActive(false)
        );
    }

    public void SellItems()
    {
        StartCoroutine(SpawnTrash());
        onSell.Invoke();
        //HideSellItemsList();
    }

    IEnumerator SpawnTrash()
    {
        int trashToSpawn = PlayerManager.Instance.PlayerInventory.GetInventory().Sum(tuple => tuple.Item2);
        int random = Random.Range(5, 9);
        for (int i = 0; i < trashToSpawn; i++)
        {
            var trash = new GameObject("trash_to_shred", typeof(Image), typeof(CircleCollider2D), typeof(Rigidbody2D));
            trash.transform.SetParent(trashSpawnPoint);
            trash.transform.localPosition = Vector3.zero;
            Vector3 randOffset = new Vector3(Random.Range(-130f, 130f), Random.Range(-5f, 5f), 0f);
            trash.layer = 12;
            trash.transform.localPosition = randOffset;
            trash.transform.localScale = new Vector3(3f, 3f, 1f);
            trash.GetComponent<Rigidbody2D>().gravityScale = 120f;
            trash.GetComponent<Rigidbody2D>().mass = 15f;
            trash.GetComponent<CircleCollider2D>().radius = 33f;
            trash.GetComponent<Image>().sprite = trashSprites[Random.Range(0, trashSprites.Count)];
            spawnedTrash.Add(trash);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.7f);
        for (int i = 0; i < (int)(20 + trashToSpawn * 1.4f); i++)
        {
            machineRect.DOAnchorPosX(Random.Range(-15f, 15f), 0.03f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.03f);
        }
        machineRect.DOAnchorPosX(0f, 0.03f).SetUpdate(true);
        
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (var go in spawnedTrash)
        {
            Destroy(go);
        }
        spawnedTrash.Clear();
        HideSellItemsList();
    }
    
    private void PopulateSellItemsList()
    {
        foreach (Transform child in entriesParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var item in PlayerManager.Instance.PlayerInventory.GetInventory())
        {
            var entry = Instantiate(sellItemEntryPrefab, entriesParent.transform);
            entry.transform.SetParent(entriesParent.transform);
            entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                item.Item1.spriteVariants[0], 
                TrashColor.MaterialToColor(item.Item1.materialType),
                item.Item1.name, 
                item.Item2, 
                item.Item1.value, 
                item.Item2 * item.Item1.value
                );
        }
    }

    public void AbandonMission()
    {
        SceneManager.LoadScene("TheHub");
    }
    
    private void OnDestroy()
    {
        DOTween.Kill(rect);
    }
}
