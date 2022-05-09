using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoorTeleporter : MonoBehaviour
{
    public Vector2Int teleportTo;

    public void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player") {
            collider.transform.position = new Vector3(teleportTo.x, teleportTo.y);
        }
    }
}
