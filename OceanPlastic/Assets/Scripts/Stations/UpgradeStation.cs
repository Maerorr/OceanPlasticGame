using UnityEngine;
using UnityEngine.Events;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] 
    private UnityEvent onUpgradePanelOpen;
    [SerializeField]
    private UnityEvent onUpgradePanelClose;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerUpgrades pi = other.GetComponentInParent<PlayerUpgrades>();
        if (pi != null)
        {
            onUpgradePanelOpen.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        onUpgradePanelClose.Invoke();
    }
}
