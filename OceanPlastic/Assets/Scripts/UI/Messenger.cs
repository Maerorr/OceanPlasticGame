using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour
{
    [SerializeField] 
    private GameObject messagePrefab;
    
    public void ShowMessage(string message, Vector3 startPosition, Color color, float time)
    {
        var messageObj = Instantiate(messagePrefab, transform);
        startPosition.z = -4f;
        messageObj.GetComponent<WarningMessage>().ShowWarningMessage(message, startPosition, color, time);
    }
}
