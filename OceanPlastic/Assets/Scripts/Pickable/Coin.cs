using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                StaticGameData.instance.money += 50;
                FindAnyObjectByType<Messenger>().ShowMessage("You've found 50 coins!", transform.position, Color.yellow, 3f);
                Destroy(gameObject);
            }
        }
    }
}
