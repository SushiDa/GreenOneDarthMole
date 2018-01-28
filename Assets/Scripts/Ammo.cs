using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public PlayerControl.AmmoType type;

    public Sprite[] Sprites;

	// Use this for initialization
	void Start () {
		switch(type)
        {
            case PlayerControl.AmmoType.RAPIDFIRE: GetComponent<SpriteRenderer>().sprite = Sprites[0]; break;
            case PlayerControl.AmmoType.MULTISHOT: GetComponent<SpriteRenderer>().sprite = Sprites[1]; break;
            case PlayerControl.AmmoType.SPLASH: GetComponent<SpriteRenderer>().sprite = Sprites[2]; break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
