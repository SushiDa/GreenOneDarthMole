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
    public bool MaterialTriggered;

    public int tier;

    public int BaseHp = 1;
    public int TierMultiplier = 3;

    private float attacPeriod = 5f;
    private float attacTimer;

    private float stunTimer;

    // Use this for initialization
    void Start()
    {
        hp = BaseHp + tier * TierMultiplier;

        attacTimer = 0;
        MaterialTriggered = false;
        stunTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(tier == 1)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (tier == 2)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (tier == 3)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        attacTimer += Time.deltaTime;
        if (attacTimer > attacPeriod)
        {

            CameraSFXPlayer.PlayClip("M ATTAC");
            GetComponent<Animator>().SetBool("isAttac", true);
            attacTimer -= attacPeriod;
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        } else
        {
            stunTimer = 0;
            GetComponent<Animator>().SetBool("isStunned", false);
        }



        if (tag == "Mole")
        {
            GetComponent<Animator>().SetBool("hasMole", true);
        } else
        {
            GetComponent<Animator>().SetBool("hasMole", false);
        }

        
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            GetComponent<Animator>().SetBool("isDead", false);
        }

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attac"))
        {
            GetComponent<Animator>().SetBool("isAttac", false);
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

        CameraSFXPlayer.PlayClip("M SPAWN");
        tag = "Mole";
        MaterialTriggered = false;
        gameObject.layer = LayerMask.NameToLayer("Mole");
        GetComponent<Animator>().SetBool("hasMole", true);
        hp = BaseHp + tier * TierMultiplier;
        attacTimer = 0;
    }

    public void Stun()
    {
        CameraSFXPlayer.PlayClip("M STUN");
        stunTimer = 20f;
        GetComponent<Animator>().SetBool("isStunned", true);
    }

    private void Death()
    {

        CameraSFXPlayer.PlayClip("M DEATH");
        tag = "Hole";
        gameObject.layer = LayerMask.NameToLayer("Hole");
        GetComponent<Animator>().SetBool("hasMole", false);
        GetComponent<Animator>().SetBool("isDead", true);

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
        drill.GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
        drill.GetComponent<Drill>().tier = tier;

        float ranAngleCorpse = Random.Range(-CorpseSpawnAngle, CorpseSpawnAngle);
        float ranSpeedCorpse= Random.Range(CorpseSpawnMinSpeed, CorpseSpawnMaxSpeed);
        float angleCorpse = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90 + ranAngleCorpse;
        Quaternion corpseRotation = Quaternion.AngleAxis(angleCorpse, Vector3.forward);
        GameObject corpse = Instantiate(CorpsePrefab, transform.position, corpseRotation);
        corpse.GetComponent<Rigidbody2D>().velocity = corpse.transform.up * ranSpeedCorpse;
        corpse.GetComponent<Collider2D>().isTrigger = false;
        corpse.GetComponent<Rigidbody2D>().angularDrag = 5;
        corpse.GetComponent<Rigidbody2D>().AddTorque(1200, ForceMode2D.Impulse);
        corpse.GetComponent<Corpse>().tier = tier;

        for(int i=0; i<3*tier;i++)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Energy"), transform.position, Quaternion.identity);
        }
    }
    
}
