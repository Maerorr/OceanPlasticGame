using TMPro;
using UnityEngine;

public class DepthMeter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    
    public void SetDepth(int depth)
    {
        text.text = $"{depth}m";
    }
}
