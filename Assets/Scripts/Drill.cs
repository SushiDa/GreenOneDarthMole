using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {
    public bool Drilling;
    private Rigidbody2D rb;

    public int MaxBounceCount;
    private int BounceCount;
    public int tier = 1;
    public GameObject AmmoPrefab;
    // Use this for initialization
    void Start () {
        Drilling = false;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Bounce(Vector2 normal)
    {
        Vector3 reflected = Vector3.Reflect(transform.up, normal);
        float angle = Mathf.Atan2(reflected.y, reflected.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = transform.up * rb.velocity.magnitude;
        BounceCount++;
        if (BounceCount > MaxBounceCount)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > 0 && Drilling)
        {
            Vector2 normal = (collision.transform.position - transform.position).normalized;
            if (collision.tag == "MaterialNeutral" )
            {
                Bounce(normal);
            }

            if (collision.tag == "Mole")
            {
                GameObject.Destroy(this.gameObject);
            }

            if (collision.tag == "Corpse")
            {
                GameObject.Destroy(collision.gameObject);
            }

            if (collision.tag == "GlobalWall")
            {
                normal = Vector2.right;
                if(collision.transform.localScale.x > collision.transform.localScale.y)
                {
                    normal = Vector2.up;
                }
                Bounce(normal);
            }

            if (collision.tag == "Crystal")
            {
                //TODO Damage Crystal
                var crystal = collision.GetComponent<Crystal>();
                bool spawnAmmo = !crystal.TakeDamage(tier * 3);

                if(spawnAmmo)
                {
                    GameObject.Instantiate(AmmoPrefab, collision.transform.position, Quaternion.identity);
                }
                else
                {
                    Bounce(normal);
                }
            }
        }
    }
}
