using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    Transform Circle;
    void Start()
    {
        Circle = GameObject.Find("Circle").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Circle.transform.position;
    }
}
