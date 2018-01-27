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

        btn1Down = getBtnDown("B");
        btn1Up = getBtnUp("B");
        btn1Held = getBtn("B");


        switch (ControlType)
        {
            case PlayerControlType.MATCHER:
                MatcherUpdate();
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
