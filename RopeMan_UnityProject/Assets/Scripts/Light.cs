using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    Player player;
    public void SetPlayer(Player p)
    {
        player = p;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            player.GetGarbage(collision.GetComponent<Garbage>());
        }
    }
}
