using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {


    List<GameObject> targets;

	// Use this for initialization
	void Start () {
        targets = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
            if (other.tag == "Mole")
            {
                Debug.Log("damage");
                if (!other.gameObject.GetComponent<Hole>().TakeDamage())
                {
                    other.gameObject.GetComponent<Hole>().SpawnCorpses(transform.up);
                }
            }

            if (other.tag == "Crystal")
            {
                other.gameObject.GetComponent<Crystal>().TakeDamage(1);
            }
        }

    }
}
