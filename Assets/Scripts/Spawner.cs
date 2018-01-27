using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float delaySecond;
    public string item;
    public bool init;
    public int tier;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        init = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude == 0 && !init)
        {
            init = true;
            StartCoroutine("SpawnCoroutine");
        }
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(delaySecond);
        GameObject spawnedItem = GameObject.Instantiate(Resources.Load("Prefabs/" + item), transform.position, Quaternion.identity) as GameObject;

        MaterialFusion fusion = spawnedItem.GetComponent<MaterialFusion>();
        if(fusion != null)
        {
            fusion.tier = tier;
        }
        Destroy(gameObject);
    }
}
