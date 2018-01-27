using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {
    public bool Drilling;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        Drilling = false;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            if(collision.tag == "MaterialNeutral")
            {
                Vector2 normal = (collision.transform.position - transform.position).normalized;
                Vector3 reflected = Vector3.Reflect(transform.up, normal);
            }
        }
    }
}
