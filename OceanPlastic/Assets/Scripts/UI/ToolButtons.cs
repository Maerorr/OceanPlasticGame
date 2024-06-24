using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToolButtons : MonoBehaviour
{
    [SerializeField]
    Button vacuumButton;
    [SerializeField]
    Button forceCannonButton;
    [FormerlySerializedAs("repairButton")] [SerializeField]
    public Button extraButton;

    public Color selectedButtonColor;
    [SerializeField] 
    private TextMeshProUGUI extraButtonTooltip;

    private float initialButtonAlpha;
    private PlayerTools playerTools;
    private CurrentTool currentTool = CurrentTool.None;

    private void Start()
    {
        playerTools = FindObjectOfType<PlayerTools>();
        DisableExtraButton();
        initialButtonAlpha = vacuumButton.transform.GetComponent<Image>().color.a;
        selectedButtonColor.a = initialButtonAlpha;
    }

    public void OnVacuumButtonClicked()
    {
        if (currentTool == CurrentTool.Vacuum)
        {
            currentTool = CurrentTool.None;
            vacuumButton.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, initialButtonAlpha);
            forceCannonButton.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, initialButtonAlpha);
            playerTools.SetCurrentTool(currentTool);
        }
        else
        {
            currentTool = CurrentTool.Vacuum;
            vacuumButton.transform.GetComponent<Image>().color = selectedButtonColor;
            forceCannonButton.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, initialButtonAlpha);
        }
        playerTools.SetCurrentTool(currentTool);
    }
    
    public void OnForceCannonButtonPressed()
    {
        if (currentTool == CurrentTool.ForceCannon)
        {
            currentTool = CurrentTool.None;
            vacuumButton.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, initialButtonAlpha);
            forceCannonButton.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, initialButtonAlpha);
        }
        else
        {
            currentTool = CurrentTool.ForceCannon;
            vacuumButton.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, initialButtonAlpha);
            forceCannonButton.transform.GetComponent<Image>().color = selectedButtonColor;
        }
        playerTools.SetCurrentTool(currentTool);
    }

    public void EnableExtraButton()
    {
        extraButton.gameObject.SetActive(true);
    }
    
    public void DisableExtraButton()
    {
        extraButton.gameObject.SetActive(false);
    }
    
    public void SetTooltip(string text)
    {
        extraButtonTooltip.text = text;
    }

    public void ClearTooltip()
    {
        extraButtonTooltip.text = "";
    }
}
