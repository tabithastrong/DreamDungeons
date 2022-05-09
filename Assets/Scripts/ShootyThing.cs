using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyThing : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 10f;
    public string tagToHit = "Enemy";
    public int damage = 2;

    void Update() {
        transform.Translate(new Vector3(direction.x, direction.y) * speed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag == "Enemy Fire" || collision.collider.tag == "Player Fire") {
            return;
        }
        
        Destroy(gameObject);
    }
}
