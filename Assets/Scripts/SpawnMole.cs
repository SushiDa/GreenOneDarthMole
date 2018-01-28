using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMole : MonoBehaviour {


    public float spawnPeriod;
    public float spawnChance;
    public float hideChance;

    private float maxx = 9f;
    private float maxy = 5f;
    private float emptyRadius = 1.5f;

    private float randomOffsetFactor = 2f;

    private int maxNewMole = 2;

    private float timer;
    private GameMaster GM;

    public float Tier2SpawnPctScoreMultiplier;
    public float Tier3SpawnPctScoreMultiplier;

    private float Tier1Spawn = 100f;
    [SerializeField]
    private float Tier2Spawn = 0f;
    [SerializeField]
    private float Tier3Spawn = 0f;


    // Use this for initialization
    void Start () {
        timer = 0;
        GM = GameObject.FindObjectOfType<GameMaster>();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(Tier2Spawn < 100)
        {
            Tier2Spawn +=  GM.CurrentEnergy * Tier2SpawnPctScoreMultiplier * Time.deltaTime / 10000;
            if (Tier2Spawn >= 100)
                Tier2Spawn = 100;
        }

        if(Tier3Spawn < 100)
        {
            Tier3Spawn += GM.CurrentEnergy * Tier3SpawnPctScoreMultiplier * Time.deltaTime / 10000;
            if (Tier3Spawn >= 100)
                Tier3Spawn = 100;
        }

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

                if (newCount > maxNewMole)
                {
                    newCount = maxNewMole;
                }
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
        var x = Random.Range(emptyRadius, maxx);
        var y = Random.Range(emptyRadius, maxy);
        x = x * Mathf.Sign(Random.Range(-1f, 1f));
        y = y * Mathf.Sign(Random.Range(-1f, 1f));
        var mole = GameObject.Instantiate(Resources.Load("Prefabs/HoleTmp"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        mole.tag = "Mole";
        mole.layer = LayerMask.NameToLayer("Mole");
    }

    IEnumerator SpawnExistingMoleCoroutine(GameObject mole)
    {
        yield return new WaitForSeconds(1);

        //tier random
        float tier2Pct = Tier2Spawn / (Tier2Spawn + Tier1Spawn + Tier3Spawn);
        float tier3Pct = Tier3Spawn / (Tier2Spawn + Tier1Spawn + Tier3Spawn);

        float value = Random.Range(0f, 1f);
        int resultTier = 1;
        if(value < tier2Pct)
        {
            resultTier = 2;
        }
        else if(value < tier2Pct + tier3Pct)
        {
            resultTier = 3;
        }

        mole.GetComponent<Hole>().tier = resultTier;
        mole.GetComponent<Hole>().Pop();
    }
}
