using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFusion : MonoBehaviour {

    public float detectWaitTime;
    public bool canFuse = true;
    public int resourceType;
    public GameObject Crystal;

    public float CircleCastRadius;
    public float CircleCastForce;

	// Use this for initialization
	void Start () {
        StartCoroutine("DetectOthers");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DetectOthers()
    {
        while (true)
        {
            if (canFuse)
            {
                Collider2D[] results = new Collider2D[20];
                GetComponent<CircleCollider2D>().OverlapCollider(new ContactFilter2D()
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
                    }
                    


                    GameObject.Destroy(this.gameObject);

                }

            }


            yield return new WaitForSeconds(detectWaitTime);
        }
    }
}
