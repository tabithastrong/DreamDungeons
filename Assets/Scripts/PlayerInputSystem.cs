using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputSystem : MonoBehaviour
{
    public Vector2 movement = Vector2.zero;
    public float movementSpeed = 1f;
    public float playerScale = 1.4f;
    public Animator animator;

    public Transform thingToFire;
    public Vector2 lookDirection;

    public int startingHealth = 20;

    public int health = 20;
    public float healthRegenerateTime = 3f;
    float healthRegenerateTimer = 0f;

    float timeSinceLastHit = 0f;
    public float cooldownOnHits = 1f;

    bool backToMainMenu = false;
    public Animator faderAnimator;

    public AudioSource hurtSfx;
    public AudioSource shootSfx;
    public AudioSource deathSfx;

    public Accessory[] accessoriesList;
    public Transform accessoryTransform;
    Accessory accessory;
    public SpriteRenderer accessoryRenderer;

    void Start() {
        health = startingHealth;

        string accessoryEquipped = PlayerPrefs.GetString("AccessoryEquipped", "");

        if(accessoryEquipped != "") {
            accessory = accessoriesList[0];

            for(int i = 0; i < accessoriesList.Length; i++) {
                if(accessoriesList[i].name == accessoryEquipped) {
                    accessory = accessoriesList[i];
                    break;
                }
            }

            accessoryTransform.localPosition = new Vector3(accessory.positionOffset.x, accessory.positionOffset.y, -2f);
            accessoryTransform.localScale = accessory.scale;
            accessoryRenderer.sprite = accessory.sprite;
            accessoryRenderer.enabled = true;
        } else {
            accessoryRenderer.enabled = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Started && health > 0) {
            GameObject go = Instantiate(thingToFire, transform.position, Quaternion.identity).gameObject;
            shootSfx.Play();

            Vector2 dir = (Camera.main.ScreenToWorldPoint(lookDirection) - transform.position);
            dir.Normalize();
            //dir *= (1f/dir.x);

            go.GetComponent<ShootyThing>().direction = dir;
        }
    }

    public void OnLookDirectionChanged(InputAction.CallbackContext context) {
        lookDirection = context.ReadValue<Vector2>();
    }

    public void FixedUpdate() {
        animator.SetInteger("Health", health);
        if(health <= 0) {
            transform.localScale = new Vector3(1f, 1f, 1f) * playerScale;
            return;
        }

        transform.Translate(movement * movementSpeed * Time.fixedDeltaTime);

        animator.SetFloat("MovementX", Mathf.Abs(movement.x));
        animator.SetFloat("MovementY", movement.y);

        if(Mathf.Abs(movement.x) > 0f) {
            transform.localScale = new Vector3(movement.x > 0.1f ? 1f : -1f, 1f, 1f) * playerScale;
        } 

        if(accessory) {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
                accessoryRenderer.enabled = false;
            } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk Sideways")) {
                if(accessory.hideOnSideAndBack) {
                    accessoryRenderer.enabled = false;
                } else if(accessory.sideAndBackSprites) {
                    accessoryRenderer.enabled = true;
                    accessoryRenderer.sprite = accessory.sprite;
                }
            } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk Backwards")) {
                if(accessory.hideOnSideAndBack) {
                    accessoryRenderer.enabled = false;
                }else if(accessory.sideAndBackSprites) {
                    accessoryRenderer.enabled = true;
                    accessoryRenderer.sprite = accessory.backSprite;
                }
            } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk Front") || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                accessoryRenderer.enabled = true;

                if(accessory.hideOnSideAndBack) {
                    accessoryRenderer.sprite = accessory.sprite;
                }else if(accessory.sideAndBackSprites) {
                    accessoryRenderer.sprite = accessory.frontSprite;
                }
            } 
        }
    }

    public IEnumerator BackToMainMenu(float delay, int level) {
        yield return new WaitForSeconds(delay * 2f);
            faderAnimator.SetBool("Fade Out", true);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(level);
    }

    void Update() {
        healthRegenerateTimer += Time.deltaTime;

        if(health <= 0 && !backToMainMenu) {
            backToMainMenu = true;
            deathSfx.Play();
            StartCoroutine(BackToMainMenu(1f, 0));
        }

        if(health > 0 && healthRegenerateTimer > healthRegenerateTime) {
            health = Mathf.Min(startingHealth, health + 1);
            healthRegenerateTimer = 0f;
        }

        timeSinceLastHit += Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(health <= 0 || timeSinceLastHit < cooldownOnHits) {
            return;
        }
        
        if(collision.collider.tag == "Enemy") {
            Enemy enemy = collision.collider.GetComponent<Enemy>();

            if(enemy.health > 0) {
                health -= enemy.damageOnTouch;
                hurtSfx.Play();
                timeSinceLastHit = 0f;
            }
        } else if(collision.collider.tag == "Enemy Fire") {
            ShootyThing fire = collision.collider.GetComponent<ShootyThing>();

            health -= fire.damage;
            hurtSfx.Play();
            timeSinceLastHit = 0f;
        }
    }
}
