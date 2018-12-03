using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;

    public AudioSource audioShot;

    public GameObject smallBulletPref;
    public GameObject bigBulletPref;
    public GameObject tinyBulletPref;
    public GameObject beamPref;
    public GameObject lightningPref;
    public AudioClip shot;

    Animator[] anims;
    Rigidbody2D rb2d;
    int speed;
    public int currWeapon;
    float reloadingTime;
    bool canOpenAltar;
    public float maxReloadingTime;
    string lastDirection;

    public float health;

    // Use this for initialization
    public void Start () {
        instance = this;
        rb2d = GetComponent<Rigidbody2D>();
        speed = 3;
        canOpenAltar = false;

        if(anims == null)
            anims = GetComponentsInChildren<Animator>();
        Debug.Log(anims.Length);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetVelocity.Normalize();
        if(Mess.instance.isPlaying)
            rb2d.velocity = targetVelocity * speed;
        //Debug.Log(rb2d.velocity);


        string trigger = null;
        Vector2 velocity = rb2d.velocity;
        if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0)
                trigger = "Right";
            else if (velocity.x < 0)
                trigger = "Left";
            else
                trigger = "Stand";
        }
        else
        {
            if (velocity.y > 0)
                trigger = "Up";
            else if (velocity.y < 0)
                trigger = "Down";
        }

        if(trigger != null)
        {
            anims[0].SetTrigger(trigger);
            anims[currWeapon + 1].SetTrigger(trigger);
            if(trigger != "Stand")
                lastDirection = trigger;
        }


        if (reloadingTime > 0)
            reloadingTime -= Time.deltaTime;
        else if (Input.GetMouseButton(0) && !AltarMenu.instance.isOpen)
            Shoot(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if(Input.GetKeyUp("space") && canOpenAltar)
            AltarMenu.instance.Open();
    }

    void Shoot(Vector2 start, Vector2 destination)
    {
        if (Mess.instance.isPlaying)
        {
            Vector2 direction = (destination - start).normalized;
            //Debug.Log(direction);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (currWeapon >= 0 && currWeapon <= 2)
            {
                GameObject bullet;
                if (currWeapon == 0)
                    bullet = smallBulletPref;
                else if (currWeapon == 1)
                    bullet = bigBulletPref;
                else
                    bullet = tinyBulletPref;

                Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));

                audioShot.PlayOneShot(shot);
            }
            else if (currWeapon == 3) // beam
            {
                Instantiate(beamPref, transform.position, Quaternion.Euler(0, 0, angle));
            }
            else if (currWeapon == 4) // lightning
            {
                Instantiate(lightningPref, destination, Quaternion.identity);
            }
            else
                Debug.Log("wrong currWeapon: " + currWeapon);
        }
        reloadingTime = maxReloadingTime;
    }

    public void SetWeapon(int weapon, float maxReloadingTime)
    {
        currWeapon = weapon;
        for(int i = 1; i < anims.Length; i++) // anim von Player überspringen
        {
            if (i == weapon + 1)
                anims[i].gameObject.SetActive(true);
            else
                anims[i].gameObject.SetActive(false);
        }
        this.maxReloadingTime = maxReloadingTime;
        reloadingTime = maxReloadingTime;
        anims[currWeapon + 1].SetTrigger(lastDirection);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Altar")
            canOpenAltar = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Altar")
            canOpenAltar = false;
    }
}
