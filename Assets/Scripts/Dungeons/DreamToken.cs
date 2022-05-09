using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamToken : MonoBehaviour
{
    public bool pickedUp = false;
    float timeSincePickedUp = 0f;
    AudioSource source;

    [Range(0f, 1f)]
    float speedGoesToPlayer = 1.2f;

    Transform player;

    void Start() {
        player = FindObjectOfType<PlayerInputSystem>().transform;
        source = GetComponent<AudioSource>();
    }

    void Update() {
        if(timeSincePickedUp > 1f) {
            Destroy(gameObject);
        }
        
        if(pickedUp) {
            timeSincePickedUp += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, player.position, speedGoesToPlayer * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, speedGoesToPlayer * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag == "Player" && !pickedUp) {
            int tokens = PlayerPrefs.GetInt("Tokens", 0);
            PlayerPrefs.SetInt("Tokens", tokens + 1);
            pickedUp = true;
            GetComponent<Rigidbody2D>().simulated = false;
            source.Play();
        }
    }
}
