using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portalscript : MonoBehaviour
{

    public GameObject press;

    private bool stay = false;

    // Start is called before the first frame update
    void Start()
    {
        press.SetActive(true);
    }
    
    
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
            SceneManager.LoadScene("Boss");
        }
    }
}
