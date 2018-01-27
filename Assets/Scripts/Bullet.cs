using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float projectileSpeed;
    public float projectileLifeTime;

    private float timer;

    // Use this for initialization
    void Start () {
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.up * projectileSpeed;

        timer += Time.deltaTime;
        if (timer > projectileLifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Mole")
        {
            other.gameObject.GetComponent<Hole>().TakeDamage();
        }
    }
}
