using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlotScript : MonoBehaviour
{

    public GameObject plotFull;

    public GameObject press;

    public String text;

    public TextMeshProUGUI fullText;

    private bool stay = false;
    private bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        press.SetActive(false);
      
    }

    // private void OnTrigerStay2D(Collider2D other)
    // {
    //     Debug.Log("SSSS");
    //     if (other.transform.tag == ("Player"))
    //     {
    //         press.SetActive(true);
    //     }
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        stay = false;
        press.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            stay = true;
            press.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("E")&& stay)
        {
            PauseMenu.isPaused = true;
            Time.timeScale = 0;
            fullText.text = text;
            plotFull.SetActive(true);
            open = true;
        }
        if(Input.GetButtonDown("Escape" )&& open)
        {
            open = false;
            //PauseMenu.isPaused = false;
            Time.timeScale = 1;
            plotFull.SetActive(false);
        }
    }
}
