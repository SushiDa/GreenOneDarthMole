using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFusion : MonoBehaviour {

    [System.Serializable]
    public struct TierColor
    {
        public int tier;
        public Sprite sprite;
    }
    public TierColor[] tiersAndColors;

    public float detectWaitTime;
    public bool canFuse = true;
    public int resourceType = -1;
    public int tier = 0;
    public GameObject Crystal;
    public Collider2D AuraCollider;
    public int currentChainCombo = 1;

    public float CircleCastRadius;
    public float CircleCastForce;


    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("DetectOthers");


        TierColor result = tiersAndColors[0];

        if (resourceType <0 || resourceType>=tiersAndColors.Length )
        {
            //get random per tier
            List<TierColor> list = new List<TierColor>(tiersAndColors);
            List<TierColor> filtered = list.FindAll(x => x.tier == tier);

            if (filtered.Count > 0)
            {
                resourceType = Random.Range(0, filtered.Count);
            }
            else
            {
                resourceType = 0;
            }
            result = tiersAndColors[resourceType];
        }
        else
        {
            // get per resource type
            result = tiersAndColors[resourceType];
        }

        GetComponent<SpriteRenderer>().sprite = result.sprite;
        tier = result.tier;
    }
	
	// Update is called once per frame
	void Update () {
		if(rb.velocity.magnitude == 0)
        {
            currentChainCombo = 1;
        }
	}

    IEnumerator DetectOthers()
    {
        while (true)
        {
            if (canFuse)
            {
                Collider2D[] results = new Collider2D[20];
               AuraCollider.OverlapCollider(new ContactFilter2D()
                {
                    useTriggers = true,
                    layerMask = LayerMask.GetMask("Material")
                }, results);


                MaterialFusion other1 = null;
                MaterialFusion other2 = null;

                foreach(Collider2D col in results)
                {
                    if (col != null)
                    {
                        MaterialFusion mat = col.GetComponent<MaterialFusion>();
                        if (mat != null && mat.resourceType == this.resourceType && mat.canFuse && mat != this && mat != other1 && mat != other2)
                        {
                            //Match 
                            if (other1 == null)
                                other1 = mat;
                            else if (other2 == null)
                            {
                                other2 = mat;
                                break;
                            }
                        }
                    }
                }

                if(other1 != null && other1.canFuse && other2 != null && other2.canFuse)
                {
                    int chainCombo = Mathf.Max(other1.currentChainCombo, other2.currentChainCombo, currentChainCombo);

                    other1.canFuse = false;
                    other2.canFuse = false;
                    this.canFuse = false;

                    // Spawn stuff
                    Vector3 targetPosition = (other1.transform.position + other2.transform.position + transform.position) / 3;

                    GameObject.Destroy(other1.gameObject);
                    GameObject.Destroy(other2.gameObject);
                    GameObject.Instantiate(Crystal, targetPosition, Quaternion.identity);

                    var hits = Physics2D.CircleCastAll(targetPosition, CircleCastRadius, Vector2.up, CircleCastRadius,LayerMask.GetMask("Material"));

                    foreach(RaycastHit2D hit in hits)
                    {
                        Rigidbody2D otherRb = hit.collider.GetComponent<Rigidbody2D>();
                        if(otherRb != null)
                        {
                            otherRb.velocity = (otherRb.transform.position - targetPosition).normalized * CircleCastForce;
                        }
                        MaterialFusion otherMat = hit.collider.GetComponent<MaterialFusion>();
                        if(otherMat != null)
                        {
                            otherMat.currentChainCombo = chainCombo + 1;
                        }
                    }
                    
                    for(int i =0; i < 1+2*chainCombo;i++)
                    {
                        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Energy"), transform.position, Quaternion.identity);
                    }


                    GameObject.Destroy(this.gameObject);

                }

            }


            yield return new WaitForSeconds(detectWaitTime);
        }
    }
}
