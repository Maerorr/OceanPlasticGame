using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButtons : MonoBehaviour
{
    [SerializeField]
    Button vacuumButton;
    [SerializeField]
    Button forceCannonButton;
    
    public void OnVacuumButtonClicked()
    {
        vacuumButton.transform.GetComponent<Image>().color = Color.green;
        forceCannonButton.transform.GetComponent<Image>().color = Color.white;
    }
    
    public void OnForceCannonButtonPressed()
    {
        vacuumButton.transform.GetComponent<Image>().color = Color.white;
        forceCannonButton.transform.GetComponent<Image>().color = Color.green;
    }
}
