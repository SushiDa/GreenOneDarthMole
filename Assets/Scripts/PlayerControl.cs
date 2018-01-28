using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public GameMaster GM;
    public int PlayerNumber;

    public PlayerControlType ControlType;
    
    private Dictionary<string, string> controlMapping;
    private bool ps4;

    private Rigidbody2D rb;

    private Vector2 currentMovement;
    private Vector2 latestLookDirection;

    private bool btn1Held;
    private bool btn1Down;
    private bool btn1Up;
    private bool btn2Held;
    private bool btn2Down;
    private bool btn2Up;

    public RigidBodyParams[] rigidbodyParams;

    [Header("Matcher Parameters")]
    public float MatcherPlayerSpeed;
    public float MatcherThrowSpeed;
    public float MaterialPickupOffset;
    private Transform HeldMaterial = null;
    //private Vector2 playerSpeedDamp;

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

    [Header("Shooter Parameters")]
    public float ShooterPlayerSpeed;
    public float baseAmmoTimer;
    public float timerMalusPerAmmo;
    public float baseFireRate;
    public float bonusFireRate;
    public float baseProjectileSpeed;
    public float bonusProjectileSpeed;
    private float ammoTimer;
    private float shootTimer;
    private List<AmmoType> currentAmmo;


    [Header("Driller Parameters")]
    public float DrillerPlayerSpeed;
    public Rigidbody2D HeldDrill = null;
    public SpriteRenderer AimSpriteRenderer;
    public float DrillPickupOffset;
    public float DrillLaunchSpeed;
    public float DrillLifeTime;



    [System.Serializable]
    public struct RigidBodyParams
    {
        public PlayerControlType controlType;
        public string layer;
        public float mass;
        public float linearDrag;
        public float angularDrag;
        public float gravityScale;
        public bool freezeRotation;
    }

    public enum PlayerControlType
    {
        SHOOTER,
        MOWER,
        MATCHER,
        DRILLER
    }
    
    void Start () {
        GM = GameObject.FindObjectOfType<GameMaster>();
        rb = GetComponent<Rigidbody2D>();
        ChangeControlType(ControlType);
        ps4 = false;
        initControlMapping();
        if (Input.GetJoystickNames().Length >= PlayerNumber && Input.GetJoystickNames()[PlayerNumber - 1].StartsWith("Wireless"))
        {
            switchToPS4Controller();
        }
        
        currentAmmo = new List<AmmoType>(); 
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Inputs
        currentMovement = new Vector2(getAxis("H") + getAxis("HD"), getAxis("V") + getAxis("VD"));
        if (currentMovement.magnitude > 1)
            currentMovement.Normalize();
        if (currentMovement.magnitude > 0.2)
            latestLookDirection = currentMovement.normalized;

        btn1Down = getBtnDown("A");
        btn1Up = getBtnUp("A");
        btn1Held = getBtn("A");

        btn2Down = getBtnDown("B");
        btn2Up = getBtnUp("B");
        btn2Held = getBtn("B");
        
        if(getBtnDown("Y"))
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().Play();
        }

        switch (ControlType)
        {
            case PlayerControlType.MATCHER:
                MatcherUpdate();
                break;
            case PlayerControlType.DRILLER:
                DrillerUpdate();
                break;
            case PlayerControlType.SHOOTER:
                ShooterUpdate();
                break;
        }

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
            case PlayerControlType.DRILLER:
                DrillerFixedUpdate();
                break;
            case PlayerControlType.SHOOTER:
                ShooterFixedUpdate();
                break;
            default:
                break;
        }
    }

    public void ChangeControlType(PlayerControlType controlType)
    {
        if(controlType != ControlType)
        {
            //Drop Held Item if necessary
            
        }
        ControlType = controlType;
        RigidBodyParams param = new List<RigidBodyParams>(rigidbodyParams).Find(x => x.controlType == ControlType);

        rb.mass = param.mass;
        rb.drag = param.linearDrag;
        rb.angularDrag = param.angularDrag;
        rb.gravityScale = param.gravityScale;
        rb.freezeRotation = param.freezeRotation;
        gameObject.layer = LayerMask.NameToLayer(param.layer);
    }

    #region Matcher
    private void MatcherFixedUpdate()
    {
        rb.velocity = currentMovement * MatcherPlayerSpeed;
        float angle = Mathf.Atan2(latestLookDirection.y, latestLookDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Vector2.SmoothDamp(rb.velocity, currentMovement * MatcherPlayerSpeed, ref playerSpeedDamp, 0.02f , MatcherPlayerSpeed, Time.fixedDeltaTime);
    }

    private void MatcherUpdate()
    {
        if (HeldMaterial != null)
            HeldMaterial.position = transform.position;
        if (btn1Down)
        {
            if (HeldMaterial == null)
            {
                Debug.DrawRay(transform.position, latestLookDirection, Color.green, 1);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, latestLookDirection, MaterialPickupOffset, LayerMask.GetMask("Material"));

                if (hit)
                {
                    HeldMaterial = hit.collider.transform;
                    var colliders = HeldMaterial.GetComponents<Collider2D>();
                    foreach (Collider2D col in colliders)
                    {
                        col.enabled = false;
                    }
                }
            }
            else
            {

                HeldMaterial.position = transform.position + new Vector3(latestLookDirection.x* MaterialPickupOffset, latestLookDirection.y* MaterialPickupOffset);
                Rigidbody2D otherRb = HeldMaterial.GetComponent<Rigidbody2D>();
                if(otherRb != null)
                {
                    otherRb.velocity = latestLookDirection * currentMovement.magnitude * MatcherThrowSpeed;
                }
                var colliders = HeldMaterial.GetComponents<Collider2D>();
                foreach (Collider2D col in colliders)
                {
                    col.enabled = true;
                }

                HeldMaterial = null;
            }

        }
    }
    #endregion


    #region Shooter
    private void ShooterUpdate()
    {
        if (btn1Held)
        {
            
            if (ammoTimer == 0)
            {
                ammoTimer = GetAmmoTimerIncreaseValue();
                shootTimer = 0;
            }

            if (shootTimer > 0)
            {
                int nbShot = GetNbShot();
                float totalAngle = nbShot * 10;
                for (int i = 0; i < nbShot; i++)
                {
                    var angleOffset = -totalAngle / 2 + i * 10;
                    var bullet = GameObject.Instantiate(Resources.Load("Prefabs/ProjectileTmp"), transform.position + transform.up * 0.5f, Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90f + angleOffset))) as GameObject;
                    bullet.GetComponent<Bullet>().SetProjectileSpeed(GetShootProjectileSpeed());
                }
                shootTimer -= GetShootFireTime();
            }

            shootTimer += Time.deltaTime;

        }

        if (ammoTimer > 0)
        {
            ammoTimer -= Time.deltaTime;
            if (ammoTimer <= 0)
            {
                currentAmmo.Clear();
                ammoTimer = 0;
            }
        } 
    }

    private void ShooterFixedUpdate()
    {
        if(currentMovement.magnitude > 0 && !btn1Held)
        {
            rb.velocity = currentMovement * ShooterPlayerSpeed;
            transform.rotation = Quaternion.Euler(0, 0, -90 + Mathf.Atan2(latestLookDirection.normalized.y, latestLookDirection.normalized.x) * 180 / Mathf.PI);
        } else
        {
            rb.velocity = Vector2.zero;
        }

        if (btn1Held && currentMovement.magnitude > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90 + Mathf.Atan2(latestLookDirection.normalized.y, latestLookDirection.normalized.x) * 180 / Mathf.PI);
        }
        
    }

    private float GetAmmoTimerIncreaseValue()
    {
        float increaseTimer = baseAmmoTimer;
        for (int i = 1; i < currentAmmo.Count; i++)
        {
            increaseTimer = increaseTimer * (1 - timerMalusPerAmmo);
        }
        return increaseTimer;
    }

    private float GetShootFireTime()
    {
        int nbRapidFireAmmo = 0;
        foreach(AmmoType type in currentAmmo)
        {
            if (type == AmmoType.RAPIDFIRE)
            {
                nbRapidFireAmmo++;
            }
        }
        float fireRate = baseFireRate + nbRapidFireAmmo * bonusFireRate;
        float fireTime = 1f / fireRate;

        return fireTime;
    }

    private int GetNbShot()
    {
        int nbMultiShotAmmo = 0;
        foreach (AmmoType type in currentAmmo)
        {
            if (type == AmmoType.MULTISHOT)
            {
                nbMultiShotAmmo++;
            }
        }

        return 1 + nbMultiShotAmmo * 2;
    }

    private float GetShootProjectileSpeed()
    {
        int nbRapidFireAmmo = 0;
        foreach (AmmoType type in currentAmmo)
        {
            if (type == AmmoType.RAPIDFIRE)
            {
                nbRapidFireAmmo++;
            }
        }
        float projectileSpeed = baseProjectileSpeed + nbRapidFireAmmo * bonusProjectileSpeed;

        return projectileSpeed;
    }

    private void LoadAmmo(GameObject ammo)
    {
        currentAmmo.Add(ammo.GetComponent<Ammo>().type);
        Destroy(ammo);

        if (ammoTimer > 0)
        {
            ammoTimer += GetAmmoTimerIncreaseValue();
        }
    }

    public enum AmmoType
    {
        RAPIDFIRE,
        MULTISHOT,
        SPLASH
    }

    #endregion

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

        gameObject.GetComponent<Animator>().SetFloat("mouvementX", transform.up.x);
        gameObject.GetComponent<Animator>().SetFloat("mouvementY", transform.up.y);
    }

    private void DrillStun(GameObject drill)
    {
        StartCoroutine("DrillStunCoroutine");
        drill.GetComponent<Rigidbody2D>().AddForce(-transform.up * drillStunForce, ForceMode2D.Impulse);
    }

    private void MoleBump(GameObject mole)
    {
        Vector2 offset = mole.GetComponent<Collider2D>().offset;
        Vector3 normal = (mole.transform.position - transform.position + new Vector3(offset.x,offset.y)).normalized;
        Vector3 reflected = Vector3.Reflect(transform.up, normal);
        float angle = Mathf.Atan2(reflected.y, reflected.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = Vector2.zero;

        mole.GetComponent<Hole>().Stun();
        gameObject.GetComponent<Animator>().SetFloat("mouvementX", transform.up.x);
        gameObject.GetComponent<Animator>().SetFloat("mouvementY", transform.up.y);
    }
    

    private void SpawnMaterial(GameObject corpse)
    {
        int corpseTier = corpse.GetComponent<Corpse>().tier;
        for(int i = 0; i < nbSpawnMaterial; i++)
        {
            var randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
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
                spawner.GetComponent<Spawner>().item = "Material";

                //TODO récupérer le bon tier
                spawner.GetComponent<Spawner>().tier = corpseTier;
            }
        }

        for (int i = 0; i < 3 + corpseTier; i++)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Energy"), transform.position, Quaternion.identity);
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

    #region Driller

    private void DrillerFixedUpdate()
    {
        float angle = Mathf.Atan2(latestLookDirection.y, latestLookDirection.x) * Mathf.Rad2Deg - 90;
       transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (HeldDrill == null)
        {
            rb.velocity = currentMovement * DrillerPlayerSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
            HeldDrill.transform.position = transform.position + transform.up * DrillPickupOffset;
            HeldDrill.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void DrillerUpdate()
    {
        if (HeldMaterial != null)
            HeldDrill.position = transform.position;

        if(btn1Down && HeldDrill == null)
        {
            Debug.DrawRay(transform.position, latestLookDirection, Color.red, 1);
            //look for drill to hold

            RaycastHit2D hit = Physics2D.Raycast(transform.position, latestLookDirection, DrillPickupOffset, LayerMask.GetMask("Drill"));
            if (hit && hit.transform.tag == "Drill")
            {
                AimSpriteRenderer.enabled = true;
                HeldDrill = hit.collider.GetComponent<Rigidbody2D>();
                HeldDrill.mass = 1;
                HeldDrill.drag = 0;
                var colliders = HeldDrill.GetComponents<Collider2D>();
                foreach (Collider2D col in colliders)
                {
                    col.enabled = false;
                }
            }
        }
        else if (btn1Up && HeldDrill != null)
        {
            //release drill in the direction
            FireDrill();
        }

        //other cases are invalid
    }

    private void FireDrill()
    {
        AimSpriteRenderer.enabled = false;

        var colliders = HeldDrill.GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }
        HeldDrill.velocity = this.transform.up * DrillLaunchSpeed;
        HeldDrill.GetComponent<Drill>().Drilling = true;
        //TO CHANGE ?
        GameObject.Destroy(HeldDrill.gameObject, DrillLifeTime);
        HeldDrill = null;
    }

    #endregion

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (ControlType)
        {
            case PlayerControlType.MOWER:
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
                    case "Ammo":
                        Destroy(other.gameObject);
                        break;

                }
                break;
            case PlayerControlType.SHOOTER:
                switch (other.gameObject.tag)
                {
                    case "Ammo":
                        LoadAmmo(other.gameObject);
                        break;
                }
                break;
            default:
                break;
        }

        if(other.gameObject.tag == "Energy")
        {
            //
            if(GM != null)
                GM.DoScore();
            //Play PickupSound
            Destroy(other.gameObject);
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
