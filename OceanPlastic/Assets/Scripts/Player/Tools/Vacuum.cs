using System;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    private RaycastHit2D[] hits = new RaycastHit2D[16];
    private Transform[] consideredTransforms = new Transform[16];
    [SerializeField]
    private float vacuumRange = 3f;
    [SerializeField]
    private float vacuumPower = 5f;
    [SerializeField]
    private float maxConeAngle = 45f;
    [SerializeField]
    LayerMask layerMask;

    [SerializeField] private Transform circleCastStart;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Array.Clear(hits, 0, hits.Length);
        Array.Clear(consideredTransforms, 0, consideredTransforms.Length);
        int hitCount = Physics2D.CircleCastNonAlloc(circleCastStart.position, 1f, circleCastStart.right, hits, vacuumRange - 0.5f, layerMask);
        for (int i = 0; i < hitCount; i++)
        {
            var hit = hits[i];
            float angle = Vector2.Angle(transform.right, hit.transform.position - circleCastStart.position);
            
            if (angle < maxConeAngle)
            {
                Vector2 distance = circleCastStart.position - hit.transform.position;
                Vector2 velocity = distance.normalized / distance.magnitude;
                hit.transform.GetComponent<FloatingTrash>().MoveTowardsVacuum(velocity * vacuumPower);
                Debug.DrawLine(circleCastStart.position, hit.transform.position, Color.green);
            }
            else
            {
                Debug.DrawLine(circleCastStart.position, hit.transform.position, Color.red);
            }
        }
        // draw the cone
        //Debug.DrawLine(circleCastStart.position, circleCastStart.position + transform.right * vacuumRange, Color.red);
        Debug.DrawLine(circleCastStart.position, circleCastStart.position + circleCastStart.right * vacuumRange, Color.blue);
        Debug.DrawLine(circleCastStart.position, circleCastStart.position + Quaternion.Euler(0, 0, maxConeAngle) * transform.right * vacuumRange, Color.red);
        Debug.DrawLine(circleCastStart.position, circleCastStart.position + Quaternion.Euler(0, 0, -maxConeAngle) * transform.right * vacuumRange, Color.red);
        
    }
}
