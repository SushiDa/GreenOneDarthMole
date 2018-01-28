using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour {

    private int hp;
    public int tier = 1;

    // Use this for initialization
    void Start()
    {
        hp = tier*3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TakeDamage(int damage)
    {
        bool alive = true;
        hp-=damage;
        if (hp <= 0)
        {
            alive = false;
            Destroy(gameObject);
        }
        return alive;
    }
}
