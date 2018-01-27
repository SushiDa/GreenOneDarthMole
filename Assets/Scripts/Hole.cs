using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

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
            Death();
        }
    }

    public void Hide()
    {
        tag = "Hole";
        gameObject.layer = LayerMask.NameToLayer("Hole");

        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void Pop()
    {
        tag = "Mole";
        gameObject.layer = LayerMask.NameToLayer("Mole");

        GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void Death()
    {
        tag = "Hole";
        gameObject.layer = LayerMask.NameToLayer("Hole");

        GetComponent<SpriteRenderer>().color = Color.black;

    }

    
}
