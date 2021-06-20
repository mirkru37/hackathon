using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Following : MonoBehaviour
{

    public Transform player;
    public float yPosScale = 0;

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + yPosScale,transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
