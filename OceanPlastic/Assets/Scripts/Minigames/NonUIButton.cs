using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NonUIButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent buttonClicked;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonClicked.Invoke();
    }
}
