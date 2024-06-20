using System;
using TMPro;
using UnityEngine;

public class DepthMeter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private float maxDepth;

    private void Awake()
    {
        maxDepth = FindAnyObjectByType<Player>().GetMaxSafeDepth();
    }

    public void SetDepth(int depth)
    {
        text.text = $"{depth}m";
    }
}
