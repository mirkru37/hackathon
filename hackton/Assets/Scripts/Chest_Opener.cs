using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Opener : MonoBehaviour
{
    
    public GameObject Chest;
    private bool openAllowed;
    public Animator anim;
    public GameObject loot;
    public GameObject hint;
    private bool opened;

    void Start()
    {
        opened = false;
        hint.SetActive(false);
    }

    void Update()
    {
        if (openAllowed && Input.GetKeyDown(KeyCode.E) && !opened)
        {
            Open();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            openAllowed = true;
            if (!opened)
            {
                hint.SetActive(true);
            }
        }
        if (collision.gameObject.tag.Equals("loot"))
        {
            loot = collision.gameObject;
            loot.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            openAllowed = false;
            hint.SetActive(false);
        }
    }

    void Open()
    {
        opened = true;
        hint.SetActive(false);
        anim.SetTrigger("keyDownE");
        anim.SetBool("open", true);
        loot.SetActive(true);
        loot.GetComponent<itemAppearance>().itemMove(loot);
    }
}
