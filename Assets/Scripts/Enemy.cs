using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerInputSystem playerInputSystem;
    Transform player;
    public Animator animator;

    public bool isBoss = false;

    public float detectionDistance = 10f;
    public float movementSpeed = 1f;

    public Vector2 movement;

    public int health = 10;

    public bool hasProjectile = false;
    public AudioSource shootSfx;
    public AudioSource deathSfx;
    bool hasPlayedDeathSound = false;
    public Transform projectileToShoot;
    public float cooldownFromShoot = 3f;
    float cooldownTimer = 0f;

    public int damageOnTouch = 5;
    public float knockbackPlayerForce = 10f;

    public float enemyScale = 1.4f;

    public float scaleModifier = 1f;

    public bool pauseAnimationOnFinish = false;

    bool randomWalk = false;
    Vector2 randomWalkDirection;
    SpriteRenderer spriteRenderer;

    void Start() {
        cooldownTimer -= Random.Range(0f, 3f);
        playerInputSystem = FindObjectOfType<PlayerInputSystem>();
        player = playerInputSystem.transform;

        randomWalkDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(isBoss) {
            transform.localScale *= 2f;
            damageOnTouch *= 2;
            cooldownFromShoot *= 0.5f;
            enemyScale *= 2f;
            health = 40;
        }
    }
    
    void Update() {
        if(pauseAnimationOnFinish && spriteRenderer.color.a <= 0.01f) {
            animator.enabled = false;
        }

        if(health <= 0 && !hasPlayedDeathSound) {
            deathSfx.pitch= Random.Range(0.8f, 1.2f);
            deathSfx.Play();
            hasPlayedDeathSound = true;
        }

        if(health <= 0) {
            animator.SetBool("Happy", true);
            return;
        }

        if(playerInputSystem.health >  0 && Vector3.Distance(transform.position, player.position) < detectionDistance) {
            if(hasProjectile) {
                cooldownTimer += Time.deltaTime;

                if(cooldownTimer > cooldownFromShoot) {
                    ShootyThing st = Instantiate(projectileToShoot, transform.position, Quaternion.identity).GetComponent<ShootyThing>();
                    st.direction = (player.transform.position - transform.position);
                    st.direction.Normalize();

                    if(shootSfx) {
                        shootSfx.Play();
                    }

                    if(isBoss) {
                        st.damage *= 2;
                    }

                    cooldownTimer = 0f;
                }
            }
            
            Vector2 dir = (player.position - transform.position);
            dir.Normalize();

            movement = dir * movementSpeed;

            transform.Translate(movement * Time.deltaTime);
        } else {
            randomWalk = true;
            movement = randomWalkDirection * movementSpeed;
            transform.Translate(movement * Time.deltaTime);
        }

        if(Mathf.Abs(movement.x) > 0f) {
            transform.localScale = new Vector3((movement.x > 0.1f ? 1f : -1f) * scaleModifier, 1f, 1f) * enemyScale;
        } 

        animator.SetFloat("MovementX", Mathf.Abs(movement.x));
        animator.SetFloat("MovementY", movement.y);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(health <= 0) {
            return;
        }

        bool doKnockback = false;

        if(randomWalk) {
            randomWalkDirection *= -1;
        }

        if(collision.collider.GetComponent<ShootyThing>()) {
            ShootyThing thing = collision.collider.GetComponent<ShootyThing>();
            
            if(thing.tagToHit.Equals(gameObject.tag)) {
                health -= thing.damage;
                doKnockback = true;
            }
        } else if(collision.collider.tag == "Player" && collision.collider.GetComponent<PlayerInputSystem>().movement.magnitude < 0.1f) {
            doKnockback = true;
        }

        if(doKnockback) {
            Vector2 dir = transform.position - player.position;
            dir.Normalize();

            transform.Translate(dir * knockbackPlayerForce);
        }
    }
}
