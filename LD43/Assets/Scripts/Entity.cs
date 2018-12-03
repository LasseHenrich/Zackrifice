using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    int addition;
	// Use this for initialization
	void Start () {
        if (gameObject.tag == "Weapon")
            addition = 1;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt((transform.position.y * 100f)) * -1 + addition;
    }
}
