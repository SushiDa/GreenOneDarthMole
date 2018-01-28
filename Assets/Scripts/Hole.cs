using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

    private int hp;
    public GameObject CorpsePrefab;
    public GameObject DrillPrefab;
    public float CorpseSpawnMaxSpeed;
    public float CorpseSpawnMinSpeed;
    public float DrillSpawnMaxSpeed;
    public float DrillSpawnMinSpeed;

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

    public bool TakeDamage()
    {
        bool alive = true;
        hp--;
        if (hp <= 0)
        {
            alive = false;
            Death();
        }
        return alive;
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

    public void SpawnCorpses(Vector2 direction)
    {
        Vector2 drillDirection = direction.normalized;
        
    }
    
}
