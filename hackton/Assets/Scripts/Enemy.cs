using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public Animator anim;
    private bool isDead = false;
    private bool walk = true;
    private float deathtime;
    public float damage;

    private float currentHealth;

    public float movementSpeed;
    public float movementDirection;
    public float chaseDistance;
    public Transform ray;
    public Transform player;
    private Vector3 targetPos;
    public bool isChasing = false;
    public float speedMod = 1;
    public float hitDistance;
    private double nextAttackTime;
    public float attackSpeed;

    public HealthBar healthBar;
    public GameObject attackPoint;
    public float hitBoxWidth;
    public float hitBoxHeight;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMax(maxHealth);
        healthBar.setCurrent(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            Destroy(this.gameObject, deathtime);
        else
        {
            if (walk)
            {
                if (isChasing)
                {
                    speedMod = 1.5f;
                    if (transform.position.x - player.position.x < 0)
                    {
                        movementDirection = 1;
                    }
                    else
                        movementDirection = -1;
                }
                else
                {
                    speedMod = 1f;
                }
                transform.position += new Vector3(movementDirection, 0, 0) * (Time.deltaTime * movementSpeed * speedMod);
                anim.SetFloat("Speed", Mathf.Abs(movementSpeed));
                if (!Mathf.Approximately(0, movementDirection / 2f))
                {
                    transform.rotation = movementDirection > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                }

                RaycastHit2D hit = Physics2D.Linecast(ray.position,
                    ray.position + new Vector3(0.5f, 0, 0) * movementDirection);
               
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Wall")
                        movementDirection *= -1;
                }
                RaycastHit2D playerHit = Physics2D.Linecast(ray.position,
                    ray.position + new Vector3(chaseDistance, 0, 0) * movementDirection);
                if (playerHit.transform != null)
                {
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                }
                playerHit = Physics2D.Linecast(ray.position,
                    ray.position + new Vector3(chaseDistance, 0, 0) * -1 *movementDirection);
                if (playerHit.transform != null)
                {
                    if (playerHit.transform.tag == "Player")
                        isChasing = true;
                }
                else
                {
                    isChasing = false;
                }
                Debug.DrawLine(ray.position, ray.position+new Vector3(hitDistance,0,0)*movementDirection, Color.green);
                hit = Physics2D.Linecast(ray.position,
                    ray.position + new Vector3(hitDistance, 0, 0) * movementDirection);
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Player")
                    {
                        if (Time.time >= nextAttackTime)
                        {
                            StartCoroutine(Attack(0.5f));
                            nextAttackTime = Time.time + 1f / attackSpeed;
                        }
                    }
                }
                
            }
        }
    }

    private void HitPlayer()
    {
        anim.SetTrigger("Attack");
        float waittime = anim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(Hit(0.3f));
    }

    public void TakeDamage(float howMuch)
    {
        currentHealth -= howMuch;
        healthBar.setCurrent(currentHealth);
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();

        }
    }

    private void Die()
    {
        anim.SetTrigger("Death");
        deathtime = anim.GetCurrentAnimatorStateInfo(0).length;
        walk = false;
        StartCoroutine(wanishMe(deathtime));
    }
    
    IEnumerator wanishMe(float Count){
        yield return new WaitForSeconds(Count);
         deathtime = anim.GetCurrentAnimatorStateInfo(0).length;
         isDead = true;
        yield return null;
    }
    
    IEnumerator Hit(float Count){
        yield return new WaitForSeconds(Count);
        if (!isDead)
        {
            Collider2D[] hitedEnemies = Physics2D.OverlapBoxAll(attackPoint.transform.position, new Vector2(hitBoxWidth, hitBoxHeight), 0, enemyLayers);
            foreach (Collider2D c in hitedEnemies)
            {
                c.GetComponent<Player_Controler>().takeDamage(damage);
            }
          //  player.GetComponent<Player_Controler>().takeDamage(damage);
            walk = true;
        }

        yield return null;
    }
    
    IEnumerator Attack(float Count){
        yield return new WaitForSeconds(Count);
        walk = false;
        HitPlayer();
        yield return null;
    }

    public void MoveTo(Vector3 targetPos)
    {
        SetTargerPossition(targetPos);
    }

    private void SetTargerPossition(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
    
    private void OnDrawGizmosSelected()
    { 
        if (attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.transform.position, new Vector3(hitBoxWidth, hitBoxHeight));
    }
}
