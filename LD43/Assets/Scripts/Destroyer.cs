using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public float lifeTime;
    float currTime;

	// Use this for initialization
	void Start () {
        currTime = 0;	
	}
	
	// Update is called once per frame
	void Update () {
        currTime += Time.deltaTime;
        if (currTime > lifeTime)
            Destroy(gameObject);
	}
}
