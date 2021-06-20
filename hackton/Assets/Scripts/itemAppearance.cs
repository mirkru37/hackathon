using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemAppearance : MonoBehaviour
{
    public Rigidbody2D rb;
    private float jumpForce = 5;
    public SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void itemMove(GameObject loot)
    {
        
        rb.AddForce(new Vector2(Random.Range(-2, 2), jumpForce), ForceMode2D.Impulse);
        sprite.sortingOrder = 1;
    }
    
}
