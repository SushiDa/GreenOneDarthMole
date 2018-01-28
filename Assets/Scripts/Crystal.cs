using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour {

    [SerializeField]
    private int hp;
    public int tier = 1;

    public Sprite[] tierSprites;
    // Use this for initialization
    void Start()
    {
        hp = tier*3;
        GetComponent<SpriteRenderer>().sprite = tierSprites[tier - 1];
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
        else
        {
            //trust me
            int index = (hp - 1) / 3;

            if (index < 0) index = 0;
            if (index > 2) index = 2;

            GetComponent<SpriteRenderer>().sprite = tierSprites[index];

        }
        return alive;
    }
}
