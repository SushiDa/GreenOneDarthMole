using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour {

    Rigidbody2D rb;
    public int tier;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (tier == 1)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (tier == 2)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (tier == 3)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }


        if (rb.velocity.magnitude == 0)
        {
            GetComponent<Collider2D>().isTrigger = true;
            rb.angularDrag = 0.05f;
        }

    }
}
