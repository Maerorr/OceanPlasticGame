using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Left = -1,
    Right = 1,
}

public class Parallax : MonoBehaviour
{
    private Camera cam;
    private Vector2 startPosition;
    [SerializeField, Range(0f, 1f)]
    float parallaxFactor = 0.5f;

    [SerializeField] 
    private Transform firstSprite;
    [SerializeField] 
    private Transform secondSprite;
    
    private float width, height;
    private float startZ;
    
    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;
        startZ = transform.position.z;
        // Assuming both sprites are the same size. which is reasonable as they are most likely the same image
        width = firstSprite.GetComponent<SpriteRenderer>().bounds.size.x;
        height = firstSprite.GetComponent<SpriteRenderer>().bounds.size.y;
    }
    
    void Update()
    {
        float distance = cam.transform.position.x * parallaxFactor;
        transform.position = new Vector3(startPosition.x + distance, transform.position.y, startZ);
        var cameraSide = GetCameraSide();
        MoveOtherSprite(cameraSide);
    }
    
    private Side GetCameraSide()
    {
        if (cam.transform.position.x > GetSpritesPosition().Item1.x)
        {
            return Side.Right;
        }

        return Side.Left;
    } 
    
    // returns a tuple with the positions (closer, farther)
    private (Vector2, Vector2) GetSpritesPosition()
    {
        float distanceToFirst = Mathf.Abs(cam.transform.position.x - firstSprite.position.x);
        float distanceToSecond = Mathf.Abs(cam.transform.position.x - secondSprite.position.x);
        if (distanceToFirst < distanceToSecond)
        {
            return (firstSprite.position, secondSprite.position);
        }
        return (secondSprite.position, firstSprite.position);
    }

    private int WhichIsCloser()
    {
        float distanceToFirst = Mathf.Abs(cam.transform.position.x - firstSprite.position.x);
        float distanceToSecond = Mathf.Abs(cam.transform.position.x - secondSprite.position.x);
        if (distanceToFirst > distanceToSecond)
        {
            return 0;
        }
        return 1;
    }

    private void MoveOtherSprite(Side side)
    {
        var whichIsCloser = WhichIsCloser();
        if (whichIsCloser == 0)
        {
            Vector3 newPos = firstSprite.position;
            newPos.x = secondSprite.position.x + (width * (float)side);
            newPos.z = startZ;
            firstSprite.position = newPos;
        }
        else
        {
            Vector3 newPos = secondSprite.position;
            newPos.x = firstSprite.position.x + (width * (float)side);
            newPos.z = startZ;
            secondSprite.position = newPos;
        }
    }
}
