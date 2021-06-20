using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpClay : MonoBehaviour
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
            gameObject.SetActive(true);
            PlayerManeger.coin++;
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            pickUpAble = true;
            hint.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            pickUpAble = false;
            hint.SetActive(false);
        }
    }
    
}
