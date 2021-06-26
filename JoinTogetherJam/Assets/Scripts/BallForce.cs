using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallForce : MonoBehaviour
{
    public bool Launch;
    public Vector2 forces;
    Rigidbody2D rb;
    Vector2 initialPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPos = transform.position;
    }

    void Update()
    {
        if (Launch)
        {
            Launch = false;
            rb.AddForce(forces);
            rb.gravityScale = 1;
        }
    }
    public void renit()
    {
        rb.velocity = Vector3.zero;
        rb.position = initialPos;
        rb.gravityScale = 0;
    }
}
