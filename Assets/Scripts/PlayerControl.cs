using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public int PlayerNumber;

    public PlayerControlType ControlType;

    private Dictionary<string, string> controlMapping;
    private bool ps4;

    private Rigidbody2D rb;

    private Vector2 currentMovement;
    private bool btn1Held;
    private bool btn1Down;
    private bool btn1Up;
    private bool btn2Held;
    private bool btn2Down;
    private bool btn2Up;

    [Header("Matcher Parameters")]
    public float MatcherPlayerSpeed;
    private Vector2 playerSpeedDamp;

    [Header("Mower Parameters")]
    public float thrust;
    public float brakeForce;
    public float turnSpeed;
    public float drillStunForce;
    private bool drillStun = false;
    private int nbSpawnMaterial = 3;
    private float spawnForce = 5;
    private float materialSpawnDelay = 2;
    private float neutralMaterialChance = 0.1f;



    public enum PlayerControlType
    {
        SHOOTER,
        MOWER,
        MATCHER,
        DRILLER
    }
    
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        ps4 = false;
        initControlMapping();
        if (Input.GetJoystickNames().Length >= PlayerNumber && Input.GetJoystickNames()[PlayerNumber - 1].StartsWith("Wireless"))
        {
            switchToPS4Controller();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Inputs
        currentMovement = new Vector2(getAxis("H") + getAxis("HD"), getAxis("V") + getAxis("VD"));
        if (currentMovement.magnitude > 1)
            currentMovement.Normalize();

        btn1Down = getBtnDown("A");
        btn1Up = getBtnUp("A");
        btn1Held = getBtn("A");

        btn2Down = getBtnDown("B");
        btn2Up = getBtnUp("B");
        btn2Held = getBtn("B");
        

    }

    private void FixedUpdate()
    {
        switch(ControlType)
        {
            case PlayerControlType.MATCHER:
                MatcherFixedUpdate();
                break;
            case PlayerControlType.MOWER:
                MowerFixedUpdate();
                break;
            default:
                break;
        }
    }

    private void MatcherFixedUpdate()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, currentMovement * MatcherPlayerSpeed, ref playerSpeedDamp, 1, 0.1f, Time.deltaTime);
    }


    private void MatcherUpdate()
    {

    }

    #region Mower
    private void MowerFixedUpdate()
    {
        if (btn1Held && !drillStun)
        {
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }

        if (btn2Held)
        {
            rb.AddForce(-transform.up * brakeForce, ForceMode2D.Impulse);
        }

        rb.MoveRotation(rb.rotation - Time.fixedDeltaTime * turnSpeed * currentMovement.x);
    }

    private void DrillStun(GameObject drill)
    {
        StartCoroutine("DrillStunCoroutine");
        drill.GetComponent<Rigidbody2D>().AddForce(-transform.up * drillStunForce, ForceMode2D.Impulse);
    }

    private void MoleBump(GameObject mole)
    {

        /*var normal = rb.position - mole.GetComponent<Rigidbody2D>().position;
        var inDir = new Vector2(transform.up.x, transform.up.y);
        var outDir = Vector2.Reflect(inDir, normal);
        Debug.Log(outDir);
        Debug.Log(Vector2.Angle(Vector2.right, outDir));
        rb.rotation = -Vector2.Angle(Vector2.right, outDir);*/
        rb.rotation = rb.rotation + 180;
        rb.velocity = Vector2.zero;

        //TODO le stun
    }

    private void SpawnMaterial(GameObject corpse)
    {
        for(int i = 0; i < nbSpawnMaterial; i++)
        {
            var randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Debug.Log(randomOffset.normalized);
            var randomForce = spawnForce/2 + spawnForce * Random.value;
            var spawner = GameObject.Instantiate(Resources.Load("Prefabs/SpawnerTmp"), transform.position - transform.up * 0.5f + randomOffset.normalized * 0.4f, Quaternion.identity) as GameObject;
            var spawnDir3 = spawner.transform.position - transform.position;
            var spawnDir = new Vector2(spawnDir3.x, spawnDir3.y);
            spawner.GetComponent<Rigidbody2D>().AddForce(spawnDir.normalized * randomForce, ForceMode2D.Impulse);
            spawner.GetComponent<Spawner>().delaySecond = materialSpawnDelay;
            if (Random.value < neutralMaterialChance)
            {
                spawner.GetComponent<Spawner>().item = "MaterialNeutral";
            } else
            {
                var randomItem = Random.Range(1, 3);
                spawner.GetComponent<Spawner>().item = "MaterialTmp" + randomItem;
            }
        }
        Destroy(corpse);
    }

    IEnumerator DrillStunCoroutine()
    {
        drillStun = true;
        yield return new WaitForSeconds(1);
        drillStun = false;
    }

    #endregion

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Mole":
                MoleBump(other.gameObject);
                break;
            case "Drill":
                DrillStun(other.gameObject);
                break;
            case "Corpse":
                SpawnMaterial(other.gameObject);
                break;

        }

    }


    void switchToPS4Controller()
    {
        ps4 = true;
        controlMapping.Clear();
        controlMapping.Add("H", "H");
        controlMapping.Add("VD", "HPS4");
        controlMapping.Add("V", "V");
        controlMapping.Add("HD", "VD");
        controlMapping.Add("Y", "Y");
        controlMapping.Add("A", "B");
        controlMapping.Add("X", "A");
        controlMapping.Add("B", "X");
        controlMapping.Add("Start", "Start");
        controlMapping.Add("Back", "Back");

    }

    void initControlMapping()
    {
        controlMapping = new Dictionary<string, string>();
        controlMapping.Add("H", "H");
        controlMapping.Add("HD", "HD");
        controlMapping.Add("V", "V");
        controlMapping.Add("VD", "VD");
        controlMapping.Add("Y", "Y");
        controlMapping.Add("A", "A");
        controlMapping.Add("X", "X");
        controlMapping.Add("B", "B");
        controlMapping.Add("Start", "Start");
        controlMapping.Add("Back", "Back");

    }

    float getAxis(string name)
    {
        string axis = getInputName(name);
        return Input.GetAxis(axis);
    }

    bool getBtnDown(string name)
    {
        string btn = getInputName(name);
        return Input.GetButtonDown(btn);
    }

    bool getBtn(string name)
    {
        string btn = getInputName(name);
        return Input.GetButton(btn);
    }

    bool getBtnUp(string name)
    {
        string btn = getInputName(name);
        return Input.GetButtonUp(btn);
    }

    string getInputName(string name)
    {
        if (controlMapping.ContainsKey(name))
            return "P" + PlayerNumber + controlMapping[name];
        else
            return "P" + PlayerNumber + name;
    }
}
