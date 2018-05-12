using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterController : MonoBehaviour {

    public Sprite lightSide;
    public Sprite darkSide;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLightSprite()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = lightSide;
    }

    public void SetDarkSprite()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = darkSide;
    }
}
