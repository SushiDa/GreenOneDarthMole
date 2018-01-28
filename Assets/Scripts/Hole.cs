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
        if (tag == "Mole")
        {
            GetComponent<Animator>().SetBool("hasMole", true);
        } else
        {
            GetComponent<Animator>().SetBool("hasMole", false);
        }
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
        GetComponent<Animator>().SetBool("hasMole", false);
    }

    public void Pop()
    {
        tag = "Mole";
        gameObject.layer = LayerMask.NameToLayer("Mole");
        GetComponent<Animator>().SetBool("hasMole", true);
    }

    private void Death()
    {
        tag = "Hole";
        gameObject.layer = LayerMask.NameToLayer("Hole");
        GetComponent<Animator>().SetBool("hasMole", false);

    }

    
}
