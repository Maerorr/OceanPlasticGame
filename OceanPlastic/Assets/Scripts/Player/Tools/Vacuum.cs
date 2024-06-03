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

    [SerializeField] private Vector2 boxCastSize;

    [SerializeField] private Transform castStart;

    // Update is called once per frame
    void Update()
    {
        Array.Clear(hits, 0, hits.Length);
        
        int hitCount = Physics2D.BoxCastNonAlloc(castStart.position, boxCastSize, 0f, castStart.right, hits, vacuumRange - boxCastSize.x / 2f, layerMask);
        for (int i = 0; i < hitCount; i++)
        {
            var hit = hits[i];
            float angle = Vector2.Angle(transform.right, hit.transform.position - castStart.position);
            
            if (angle < maxConeAngle)
            {
                Vector2 distance = castStart.position - hit.transform.position;
                Vector2 velocity = distance.normalized / distance.magnitude;
                velocity = Vector2.ClampMagnitude(velocity, 10f);
                hit.transform.GetComponent<FloatingTrash>().MoveTowardsVacuum(velocity * vacuumPower);
                Debug.DrawLine(castStart.position, hit.transform.position, Color.green);
            }
            else
            {
                Debug.DrawLine(castStart.position, hit.transform.position, Color.red);
            }
        }
        
        // draw the cone
        //Debug.DrawLine(circleCastStart.position, circleCastStart.position + transform.right * vacuumRange, Color.red);
        Debug.DrawLine(castStart.position, castStart.position + castStart.right * vacuumRange, Color.blue);
        Debug.DrawLine(castStart.position, castStart.position + Quaternion.Euler(0, 0, maxConeAngle) * transform.right * vacuumRange, Color.red);
        Debug.DrawLine(castStart.position, castStart.position + Quaternion.Euler(0, 0, -maxConeAngle) * transform.right * vacuumRange, Color.red);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(castStart.position, boxCastSize);
    }
#endif
}
