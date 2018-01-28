using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float projectileSpeed;
    public float projectileLifeTime;

    private float timer;
    private int nbSplash;

    private bool triggered;

    // Use this for initialization
    void Start () {
        timer = 0;

        triggered = false;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.right * projectileSpeed;

        timer += Time.deltaTime;
        if (timer > projectileLifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered)
        {
            if (other.tag == "Mole")
            {
                Debug.Log("trigger" + other + "/" + gameObject);
                if (!other.gameObject.GetComponent<Hole>().TakeDamage())
                {
                    other.gameObject.GetComponent<Hole>().SpawnCorpses(transform.up);
                }
                if (nbSplash > 0)
                {
                    Splash();
                }
                triggered = true;
                Destroy(gameObject);
            }

            if (other.tag == "Crystal")
            {
                other.gameObject.GetComponent<Crystal>().TakeDamage(1);
                if (nbSplash > 0)
                {
                    Splash();
                }
                triggered = true;
                Destroy(gameObject);
            }

            if (other.tag == "GlobalWall")
            {
                triggered = true;
                Destroy(gameObject);
            }
        }
        

    }

    private void Splash()
    {
        Debug.Log("splash");
        var splash = GameObject.Instantiate(Resources.Load("Prefabs/Explosion"), transform.position + transform.up * 0.1f, Quaternion.identity) as GameObject;
        splash.transform.localScale = new Vector3(0.5f + nbSplash * 0.5f, 0.5f + nbSplash * 0.5f, 1);
        Destroy(splash, 0.2f);
    }

    public void SetProjectileSpeed(float speed)
    {
        projectileSpeed = speed;
    }

    public void SetNbSplash(int nb)
    {
        nbSplash = nb;
    }
}
