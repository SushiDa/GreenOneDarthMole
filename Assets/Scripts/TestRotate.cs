using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour {

    Rigidbody2D rb;
    private float targetAngle;
    public bool spin = true;
    public float SpinSpeed;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("TurnCoroutine");
    }
	
	// Update is called once per frame
	void Update () {
        
        //Debug.Log(Time.deltaTime);
	}

    private void FixedUpdate()
    {

        //rb.MoveRotation(targetAngle);
        if(spin)
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + SpinSpeed * Time.fixedDeltaTime);
    }

    IEnumerator TurnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            targetAngle += 90;
        }

    }

}
