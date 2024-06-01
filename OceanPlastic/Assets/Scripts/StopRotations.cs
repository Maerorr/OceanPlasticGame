using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotations : MonoBehaviour
{
    void LateUpdate ()
    {
         transform.rotation = Quaternion.identity;  
    }
}
