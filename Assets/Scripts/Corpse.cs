using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour {

    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (rb.velocity.magnitude == 0)
        {
            GetComponent<Collider2D>().isTrigger = true;
            rb.angularDrag = 0.05f;
        }

    }
}
