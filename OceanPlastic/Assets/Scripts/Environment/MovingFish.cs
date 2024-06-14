using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MovingFish : MonoBehaviour
{
    [SerializeField] 
    private float boundXleft;

    [SerializeField] private float speed;
    
    [SerializeField] 
    private float boundXright;
    
    [SerializeField]
    private EditorGizmoData editorDisplayData;

    private Vector3 initialPos;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        
        transform.position = new Vector3(boundXright, initialPos.y, initialPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        var lastPos = transform.position;
        transform.position += speed * Time.deltaTime * Vector3.left;
        
        var sin1 = Mathf.Sin(Time.time * 0.1f);
        var sin2 = Mathf.Sin(Time.time * 0.23f);
        
        transform.position += (sin1 + sin2 / 2.2f) * 0.1f * Time.deltaTime * Vector3.up;
        
        var deltaX = lastPos.x - transform.position.x;
        var deltaY = lastPos.y - transform.position.y;
        
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg);
        
        if (transform.position.x < (initialPos.x + boundXleft))
        {
            transform.position = new Vector3(initialPos.x + boundXright, initialPos.y, initialPos.z);
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = editorDisplayData.color;
        var centerX = transform.position.x + (boundXleft + boundXright) / 2;
        var scale = Mathf.Abs(boundXleft) + Mathf.Abs(boundXright);
        Gizmos.DrawWireCube(new Vector3(centerX, transform.position.y, transform.position.z), new Vector3(scale, 5f, 1));
        Handles.color = editorDisplayData.color;
        Handles.Label(transform.position, editorDisplayData.label);
    }
#endif
}
