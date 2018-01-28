using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShard : MonoBehaviour {

    public float ActivationTime;
    public float MinSpawnSpeed;
    public float MaxSpawnSpeed;
    public float LifeSpan;

	// Use this for initialization
	void Start () {
        int angle = Random.Range(0, 360);
        float speed = Random.Range(MinSpawnSpeed, MaxSpawnSpeed);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        transform.localScale = transform.localScale * .5f;
        StartCoroutine("Activation");
    }
	
	// Update is called once per frame
	void Update () {
        		
	}

    IEnumerator Activation()
    {
        yield return new WaitForSeconds(ActivationTime);
        GetComponent<Collider2D>().enabled = true;
        transform.localScale = transform.localScale / .5f;
        //changer de sprite a la place de la couleur
        GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(LifeSpan);
        Destroy(gameObject);
    }
}
