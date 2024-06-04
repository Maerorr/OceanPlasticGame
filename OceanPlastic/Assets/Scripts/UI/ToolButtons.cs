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

    [SerializeField] 
    private TextMeshProUGUI extraButtonTooltip;
    
    private PlayerTools playerTools;
    private CurrentTool currentTool = CurrentTool.None;

    private void Start()
    {
        playerTools = FindObjectOfType<PlayerTools>();
        DisableExtraButton();
    }

    public void OnVacuumButtonClicked()
    {
        if (currentTool == CurrentTool.Vacuum)
        {
            currentTool = CurrentTool.None;
            vacuumButton.transform.GetComponent<Image>().color = Color.white;
            forceCannonButton.transform.GetComponent<Image>().color = Color.white;
            playerTools.SetCurrentTool(currentTool);
        }
        else
        {
            currentTool = CurrentTool.Vacuum;
            vacuumButton.transform.GetComponent<Image>().color = Color.green;
            forceCannonButton.transform.GetComponent<Image>().color = Color.white;
        }
        playerTools.SetCurrentTool(currentTool);
    }
    
    public void OnForceCannonButtonPressed()
    {
        if (currentTool == CurrentTool.ForceCannon)
        {
            currentTool = CurrentTool.None;
            vacuumButton.transform.GetComponent<Image>().color = Color.white;
            forceCannonButton.transform.GetComponent<Image>().color = Color.white;
        }
        else
        {
            currentTool = CurrentTool.ForceCannon;
            vacuumButton.transform.GetComponent<Image>().color = Color.white;
            forceCannonButton.transform.GetComponent<Image>().color = Color.green;
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
