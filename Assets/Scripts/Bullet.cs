using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float projectileSpeed;
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
            Destroy(gameObject);
        }

        if (other.tag == "Crystal")
        {
            other.gameObject.GetComponent<Crystal>().TakeDamage(1);
            Destroy(gameObject);
        }

    }

    public void SetProjectileSpeed(float speed)
    {
        projectileSpeed = speed;
    }
}
