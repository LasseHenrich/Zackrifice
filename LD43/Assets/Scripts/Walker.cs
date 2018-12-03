using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {

    public AudioSource audioS;

	// Update is called once per frame
	void Update () {
        if (GetComponent<Rigidbody2D>().velocity.magnitude == 0)
            audioS.Stop();
        else if (audioS.isPlaying)
            audioS.Play();
	}
}
