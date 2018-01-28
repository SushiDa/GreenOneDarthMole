using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour {

    Rigidbody2D rb;
    public bool spin = true;
    public float SpinSpeed;

    private float startAngle;
    private float currentAngle;
    private float period = 30f;
    private float time1 = 1f;
    private float speed1 = 2f;
    private float speed2 = 10f;
    private float speed3 = 15f;
    private bool bounce = false;
    private bool bounce2 = false;
    private float timer;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        //StartCoroutine("TurnCoroutine");

        startAngle = transform.rotation.eulerAngles.z;
        currentAngle = startAngle;
        timer = -15f; //temps avant la première
    }
	
	// Update is called once per frame
	void Update () {
        
        //Debug.Log(Time.deltaTime);
	}

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 0)
        {
            if (timer < time1)
            {
                currentAngle += speed1 * Time.fixedDeltaTime + Random.Range(-0.6f, 0) / (timer + 8f);
            } else
            {
                if (!bounce)
                {
                    currentAngle += speed2 * Time.fixedDeltaTime;

                    if (currentAngle - 90 > startAngle)
                    {
                        bounce = true;
                    }
                } else if (!bounce2)
                {
                    currentAngle -= speed3 * Time.fixedDeltaTime;
                    if (currentAngle - 89 < startAngle)
                    {
                        bounce2 = true;
                    }
                } else
                {
                    currentAngle += speed2 * Time.fixedDeltaTime;
                    if (currentAngle - 90 > startAngle)
                    {
                        currentAngle = startAngle + 90;
                        timer -= period;
                        bounce = false;
                        bounce2 = false;
                        startAngle = transform.rotation.eulerAngles.z;
                        currentAngle = startAngle;
                    }
                }
            }
            
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }


        //rb.MoveRotation(targetAngle);
        //if(spin)
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + SpinSpeed * Time.fixedDeltaTime);
    }

    /*IEnumerator TurnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            targetAngle += 90;
        }

    }*/

}
