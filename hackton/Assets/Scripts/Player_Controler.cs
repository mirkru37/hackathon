using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using System;
using UnityEditor;
using Random = UnityEngine.Random;


public class Player_Controler : MonoBehaviour
{
    public GameObject deathScreen;
    public int extraJumpCount = 1;
    private int currentJump = 0;
    public static bool isPlaying;
    public float movementSpeed = 1;
    public float jumpForce = 1;
    public LayerMask whatIsGround;
    private Rigidbody2D rb;
    public Animator anim;
    public bool isGround;
    private BoxCollider2D _Cl;

    public Transform attackPoint;
    public float hitBoxWidth = 0.5f;
    public float hitBoxHeight = 0.5f;
    public LayerMask enemyLayers;
    public static float damage = 25;
    public float attackSpeed;
    private float nextAttackTime = 0f;
    public float health = 100;
    public static bool isDead = false;
    public float dashSpeed;
    public float dashCooldown;
    private float startDashTime = 0;
    
    public GameObject healthBar;
    public bool moving = true;
  //  private bool dashing = false;
    private float movement;
    private bool isBlocking = false;

    public GameObject splash;
    public Transform splashPos;

    // for ladder
    public float distance;
    public LayerMask whatIsLadder;
    public LayerMask whatIsPlatform;
    public static bool isClimbing;
    private float inputVertical;
    private bool isLadder;
    private bool isPlatform;
    
    void Start()
    {
        health = 100;
        isBlocking = false;
        moving = true;
        isDead = false;
        
        healthBar.GetComponent<HealthBar>().setMax(health);
        healthBar.GetComponent<HealthBar>().setCurrent(health);
        isPlaying = false;
        rb = GetComponent<Rigidbody2D>();
        _Cl = GetComponent<BoxCollider2D>();
    }

    
    

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !PauseMenu.isPaused)
        {
            if (moving)
            {
                movement = Input.GetAxis("Horizontal");
                isGround = Physics2D.IsTouchingLayers(_Cl, whatIsGround);
                isLadder = Physics2D.IsTouchingLayers(_Cl, whatIsLadder);
                isPlatform = Physics2D.IsTouchingLayers(_Cl, whatIsPlatform);
                if (isGround)
                {
                    currentJump = 0;
                    anim.SetBool("onGround", true);
                }
                else
                {
                    anim.SetBool("onGround", false);
                }

                //if (isPlaying)
                //{
              
                transform.position += new Vector3(movement, 0, 0) * (Time.deltaTime * movementSpeed);
                if (movement != 0 && Input.GetButtonDown("Dash"))
                {
                    if (Time.time >= startDashTime)
                    {
                        Dash(movement);
                        startDashTime = Time.time + dashCooldown;
                    }
                }

                anim.SetFloat("Speed", Mathf.Abs(movement));

                if (!Mathf.Approximately(0, movement))
                {
                    transform.rotation = movement < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                }

                if (Input.GetButtonDown("Jump") && currentJump < extraJumpCount) //Mathf.Abs(rb.velocity.y) < 0.001f)
                {
                    currentJump++;
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    anim.SetTrigger("Jump");
                }
                // }

                if (Input.GetMouseButtonDown(0) && isGround && Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackSpeed;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    block();
                }
                
                RaycastHit2D hitInfoUp = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
                RaycastHit2D hitInfoDown = Physics2D.Raycast(transform.position, Vector2.down, distance, whatIsLadder);
                if (hitInfoUp.collider != null || hitInfoDown.collider != null)
                {
                    Debug.Log("true");
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        isClimbing = true;
                    }
                }else
                {
                    isClimbing = false;
                }
        
                if (isClimbing)
                {
                    inputVertical = Input.GetAxisRaw("Vertical");
                    rb.velocity = new Vector2(rb.velocity.x, inputVertical * movementSpeed / 2);
                    rb.gravityScale = 0;
                }
                else
                {
                    rb.gravityScale = 2;
                }

                if (isGround || isPlatform)
                {
                    rb.gravityScale = 2;
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(1) && isBlocking)
                {
                    unBlock();
                }
            }
        }
    }

    private void unBlock()
    {
        anim.SetBool("isBlocking", false);
        isBlocking = false;
        moving = true;
    }

    private void block()
    {
        anim.SetBool("isBlocking", true);
        isBlocking = true;
        moving = false;
    }

    public void takeDamage(float howMuch)
    {
        anim.SetTrigger("isDamaged");
        if (isBlocking)
            howMuch /= 3;
        health -= howMuch;
        healthBar.GetComponent<HealthBar>().setCurrent(health);
        if (health <= 0)
        {
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
            isDead = true;
            anim.SetTrigger("isDead");
        }
    }
    private void Attack()
    {
        int type = Random.Range(1, 4);
        
        //animate
        anim.SetTrigger("Hit" + type);
        //detect
        Collider2D[] hitedEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(hitBoxWidth, hitBoxHeight), 0, enemyLayers);
        //damage
        foreach (Collider2D enemy in hitedEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    { 
        if (attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(hitBoxWidth, hitBoxHeight));
    }

    private void Dash(float mov)
    {
        leaveSplash();
        rb.velocity = new Vector2(mov, 0);
       rb.AddForce(new Vector2(dashSpeed*mov, 0f), ForceMode2D.Impulse);
       anim.SetTrigger("Dash");
    }

    private void leaveSplash()
    {
        Quaternion rot = Quaternion.identity;
        if (movement < 0)
            rot = Quaternion.Euler(0, 180, 0);
        Instantiate(splash, splashPos.position, rot);
    }
}
