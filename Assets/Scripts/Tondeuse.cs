using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tondeuse : MonoBehaviour
{

    private Rigidbody2D rb;

    public float thrust;
    public float brakeForce;
    public float turnSpeed;
    public float drillStunForce;

    private bool drillStun = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        var throttle = Input.GetButton("Fire1");
        var throttleDown = Input.GetButton("Fire1");
        var throttleUp = Input.GetButton("Fire1");

        var brake = Input.GetButton("Fire2");
        var brakeDown = Input.GetButton("Fire2");
        var brakeUp = Input.GetButton("Fire2");

        var horizontal = Input.GetAxis("Horizontal");

        if (throttle && !drillStun)
        {
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }

        if (brake)
        {
            rb.AddForce(-transform.up * brakeForce, ForceMode2D.Impulse);
        }

        rb.MoveRotation(rb.rotation - Time.fixedDeltaTime * turnSpeed * horizontal);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Mole":
                MoleBump(other.gameObject);
                break;
            case "Drill":
                DrillStun(other.gameObject);
                break;
            case "Corpse":
                break;

        }

    }

    private void DrillStun(GameObject drill)
    {
        StartCoroutine("DrillStunCoroutine");
        drill.GetComponent<Rigidbody2D>().AddForce(-transform.up * drillStunForce, ForceMode2D.Impulse);
    }

    private void MoleBump(GameObject mole)
    {

        /*var normal = rb.position - mole.GetComponent<Rigidbody2D>().position;
        var inDir = new Vector2(transform.up.x, transform.up.y);
        var outDir = Vector2.Reflect(inDir, normal);
        Debug.Log(outDir);
        Debug.Log(Vector2.Angle(Vector2.right, outDir));
        rb.rotation = -Vector2.Angle(Vector2.right, outDir);*/
        rb.rotation = rb.rotation + 180;
        rb.velocity = Vector2.zero;
    }

    IEnumerator DrillStunCoroutine()
    {
        drillStun = true;
        yield return new WaitForSeconds(1);
        drillStun = false;
    }
}
