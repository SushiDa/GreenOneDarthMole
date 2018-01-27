using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour {

    private int hp;

    // Use this for initialization
    void Start()
    {
        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage()
    {
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
