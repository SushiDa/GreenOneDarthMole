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
    public float CorpseSpawnAngle;
    public float DrillSpawnMaxSpeed;
    public float DrillSpawnMinSpeed;
    public float DrillSpawnAngle;

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

        float ranAngleDrill = Random.Range(-DrillSpawnAngle, DrillSpawnAngle);
        float ranSpeedDrill = Random.Range(DrillSpawnMinSpeed, DrillSpawnMaxSpeed);
        float angleDrill = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90 + ranAngleDrill;
        Quaternion drillRotation = Quaternion.AngleAxis(angleDrill, Vector3.forward);
        GameObject drill = Instantiate(DrillPrefab, transform.position, drillRotation);
        drill.GetComponent<Rigidbody2D>().velocity = drill.transform.up * ranSpeedDrill;
        drill.GetComponent<Collider2D>().isTrigger = false;
        drill.GetComponent<Rigidbody2D>().angularDrag = 5;

        float ranAngleCorpse = Random.Range(-CorpseSpawnAngle, CorpseSpawnAngle);
        float ranSpeedCorpse= Random.Range(CorpseSpawnMinSpeed, CorpseSpawnMaxSpeed);
        float angleCorpse = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90 + ranAngleCorpse;
        Quaternion corpseRotation = Quaternion.AngleAxis(angleCorpse, Vector3.forward);
        GameObject corpse = Instantiate(CorpsePrefab, transform.position, corpseRotation);
        corpse.GetComponent<Rigidbody2D>().velocity = corpse.transform.up * ranSpeedCorpse;
        corpse.GetComponent<Collider2D>().isTrigger = false;
        corpse.GetComponent<Rigidbody2D>().angularDrag = 5;
    }
    
}
