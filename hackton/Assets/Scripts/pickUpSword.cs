using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpSword : MonoBehaviour
{ 
    private bool pickUpAble;
    public GameObject hint;
    
    void Start()
    {
        hint.SetActive(false);
    }
    
    void Update()
    {
        if (pickUpAble && Input.GetKeyDown(KeyCode.E))
        {
            Player_Controler.damage = 50;
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("fdsfs");
            pickUpAble = true;
            hint.SetActive(true);
        }
    }
}
