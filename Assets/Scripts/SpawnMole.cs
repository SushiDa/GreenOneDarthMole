using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMole : MonoBehaviour {


    public float spawnPeriod;
    public float spawnChance;
    public float hideChance;

    private float minx = -9f;
    private float maxx = 9f;
    private float miny = -5f;
    private float maxy = 5f;

    private float randomOffsetFactor = 2f;

    private float timer;



	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= spawnPeriod)
        {
            var moles = GameObject.FindGameObjectsWithTag("Mole");
            var holes = GameObject.FindGameObjectsWithTag("Hole");
            var totalHoles = moles.Length + holes.Length;

            int hideCount = Mathf.CeilToInt(totalHoles * hideChance * Random.Range(1 / randomOffsetFactor, randomOffsetFactor));
            int spawnCount = Mathf.CeilToInt(totalHoles * spawnChance * Random.Range(1 / randomOffsetFactor, randomOffsetFactor));
            int newCount = 0;



            List<GameObject> hidingMoles = new List<GameObject>();
            List<GameObject> spawningMoles = new List<GameObject>();
            List<GameObject> newMoles = new List<GameObject>();

            if (hideCount > moles.Length)
            {
                //tout cacher
                foreach (GameObject mole in moles)
                {
                    hidingMoles.Add(mole);
                }
                hideCount = moles.Length;
            } else
            {
                //shuffle moles
                int m = moles.Length;
                while (m > 1)
                {
                    m--;
                    int l = Random.Range(0, m + 1);
                    GameObject obj = moles[l];
                    moles[l] = moles[m];
                    moles[m] = obj;
                }

                for (int i = 0; i < hideCount; i++)
                {
                    hidingMoles.Add(moles[i]);
                }
            }

            List<GameObject> availableHoles = new List<GameObject>();
            availableHoles.AddRange(hidingMoles);
            availableHoles.AddRange(holes);
            //shuffle holes
            int n = availableHoles.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                GameObject obj = availableHoles[k];
                availableHoles[k] = availableHoles[n];
                availableHoles[n] = obj;
            }

            if (spawnCount > availableHoles.Count)
            {
                newCount = spawnCount - availableHoles.Count;
                spawnCount = availableHoles.Count;
            }


            //Debug.Log("hideCount : " + hideCount);
            //Debug.Log("spawnCount : " + spawnCount);
            //Debug.Log("newCount : " + newCount);

            //prendre les n premiers
            for (int i = 0; i < spawnCount; i++)
            {
                spawningMoles.Add(availableHoles[i]);
            }
            

            foreach (GameObject mole in hidingMoles)
            {
                //ne pas cacher si hide + spawn
                if (!spawningMoles.Contains(mole))
                {
                    HideMole(mole);
                }
            }

            foreach (GameObject mole in spawningMoles)
            {
                SpawnExistingMole(mole);
            }

            for (int i = 0; i< newCount; i++)
            {
                SpawnNewMole();
            }

            timer -= spawnPeriod;
        }
	}

    private void HideMole(GameObject mole)
    {
        mole.GetComponent<Hole>().Hide();
    }

    private void SpawnExistingMole(GameObject mole)
    {
        StartCoroutine(SpawnExistingMoleCoroutine(mole));
    }

    private void SpawnNewMole()
    {
        StartCoroutine(SpawnNewMoleCoroutine());
    }

    IEnumerator SpawnNewMoleCoroutine()
    {
        yield return new WaitForSeconds(1);
        var mole = GameObject.Instantiate(Resources.Load("Prefabs/HoleTmp"), new Vector3(Random.Range(minx, maxx), Random.Range(miny, maxy), 0), Quaternion.identity) as GameObject;
        mole.tag = "Mole";
        mole.layer = LayerMask.NameToLayer("Mole");
    }

    IEnumerator SpawnExistingMoleCoroutine(GameObject mole)
    {
        yield return new WaitForSeconds(1);
        mole.GetComponent<Hole>().Pop();
    }
}
