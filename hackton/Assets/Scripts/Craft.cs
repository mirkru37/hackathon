using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    private bool craftable;
    public GameObject hint;
    public GameObject loot;
    private bool appierd = false;

    void Start()
    {
        hint.SetActive(false);
    }

    void Update()
    {
        if (appierd == false)
        {
            if (craftable && Input.GetKeyDown(KeyCode.E) && PlayerManeger.coin >= 10 )
            {
                loot.SetActive(true);
                loot.GetComponent<itemAppearance>().itemMove(loot);
                appierd = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            craftable = true;
            hint.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            craftable = false;
            hint.SetActive(false);
        }
    }
}
