using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {

    private float duration = 0.4f;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer < duration)
        {
            for (int i = 0; i < 5; i++)
            {
                var blood = GameObject.Instantiate(Resources.Load("Prefabs/blood"), transform.position - transform.up * 0.2f, transform.rotation) as GameObject;
                blood.transform.Rotate(0, 0, Random.Range(-45, 45));
                blood.GetComponent<Rigidbody2D>().velocity = -blood.transform.up * 2;
                Destroy(blood, 0.2f);
            }
            
        }
	}
}
