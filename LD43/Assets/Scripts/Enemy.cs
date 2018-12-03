using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public AudioSource audioMe;
    public AudioClip gotShot;
    public AudioClip die;

    public BarHandler healthBar;
    public BarHandler altarBar;

    public float dmg;
    public float attackDelay;
    public float speed;
    public int health;

    Rigidbody2D rb2d;
    int targetNode;
    List<Node> path;
    Transform player;
    int currTarget; // 0 = Altar, 1 = Player
    Vector2[] altarPos;
    bool isWalkingPath;
    float currDelay;
    Vector2 lastPos;

    bool canResetPath;
    bool pathDone;
    float dyingTime;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        currDelay = 0;
        player = Player.instance.transform;
        currTarget = -1;
        altarPos = new Vector2[6] { new Vector2(-1.5f, -0.5f), new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(1.5f, -0.5f), new Vector2(0.5f, -1.5f), new Vector2(-0.5f, -1.5f)};

        lastPos = rb2d.position;

        canResetPath = true;
        pathDone = false;
        dyingTime = -1;

        CheckForTarget();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (dyingTime == -1)
        {
            CheckForTarget();
            if (currDelay > 0)
            {
                currDelay -= Time.deltaTime;
            }
            if (isWalkingPath)
            {
                //Debug.Log("Walking Path");
                Vector2 direction = (Vector2)path[targetNode].position - rb2d.position;
                direction.Normalize();
                rb2d.velocity = direction * speed;
                canResetPath = false;
                if (Vector2.Distance(rb2d.position, path[targetNode].position) < 0.05f)
                {
                    //Debug.Log(path[targetNode].position);
                    rb2d.MovePosition(path[targetNode].position);
                    //Debug.Log("d");
                    if (targetNode < path.Count - 1)
                    {
                        targetNode++;
                        //Debug.Log(path[targetNode].position);
                    }
                    else
                    {
                        isWalkingPath = false;
                        rb2d.velocity = Vector2.zero;
                        pathDone = true;
                        //Debug.Log("Path Done");
                    }
                    canResetPath = true;
                }
            }
            else if (Vector2.Distance(rb2d.position, CurrTargetPos()) < 2 && currDelay <= 0)
            {
                if (currTarget == 0)
                    altarBar.ChangeValue(-dmg);
                else if (currTarget == 1)
                    healthBar.ChangeValue(-dmg);
                currDelay = attackDelay;
            }
            //Debug.Log(pathDone);
            Vector2 velocity = (Vector2)rb2d.position - lastPos;
            string trigger = null;
            //Debug.Log(velocity);
            if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y) - 0.001)
            {
                if (velocity.x > 0.001)
                    trigger = "Right";
                else if (velocity.x < -0.001)
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

            if (trigger != null)
                GetComponent<Animator>().SetTrigger(trigger);

            lastPos = rb2d.position;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            dyingTime += Time.deltaTime;
            if(dyingTime >= 2)
            {
                Destroy(gameObject);
            }
        }
    }

    Vector2 CurrTargetPos()
    {
        if (currTarget == 0)
            return NearestAltarPos();
        else
            return player.position;
    }

    void CheckForTarget()
    {
        int lastTarget = currTarget;
        if ((Vector2.Distance(rb2d.position, NearestAltarPos()) < Vector2.Distance(rb2d.position, player.position)) && Mess.instance.altarHealth > 0)
            currTarget = 0;
        else
            currTarget = 1;

        Vector2 target = CurrTargetPos();
        //Debug.Log(canResetPath);
        //Debug.Log(currTarget);
        if ((currTarget != lastTarget) || (currTarget == 1 && canResetPath)) // Weil Player bewegt sich
        {
            isWalkingPath = true;
            path = PathFinder.instance.FindPath(rb2d.position, target);
            //path = PathFinder.instance.FindPath(transform.position, new Vector2(1.8f, -0.5f));
            if (path.Count > 0)
                targetNode = 0;
            else if(currTarget == 1)
            {
                //Debug.Log("hd");
                isWalkingPath = false;
                //Vector2 direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
                //rb2d.velocity = direction * speed;
            }
            //Debug.Log(path[targetNode].position);
            pathDone = false;
        }
        /*if(pathDone && currTarget == 1)
        {
            Debug.Log("hd");
            isWalkingPath = false;
            Vector2 direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
            rb2d.velocity = direction * speed;
        }*/

        //Debug.Log(CurrTargetPos());
        //Debug.Log(path);
    }

    Vector2 NearestAltarPos()
    {
        /*int index = -1;
        float distance = 1000f; // Sehr Viel
        for(int i = 0; i < altarPos.Length; i++)
        {
            float thisDis = Vector2.Distance(transform.position, altarPos[i]);
            if (thisDis < distance)
            {
                index = i;
                distance = thisDis;
            }

        }
        if (index == -1)
            Debug.Log("THE FUCK??");
        return altarPos[index];*/

        if (Vector2.Distance(rb2d.position, new Vector2(-0.5f, -0.5f)) > Vector2.Distance(rb2d.position, new Vector2(0.5f, -0.5f)))
            return new Vector2(0.5f, -0.5f);
        else
            return new Vector2(-0.5f, -0.5f);
    }

    public void Damaged(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            if (dyingTime == -1) {
                audioMe.PlayOneShot(die);
                EnemyHandler.instance.EnemyDied();
                GetComponent<Animator>().SetTrigger("Die");
                dyingTime = 0;
            }
        }
        else
            audioMe.PlayOneShot(gotShot);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currDelay <= 0 && dyingTime == -1)
        {
            if (collision.gameObject.tag == "Altar" && currTarget == 0)
                altarBar.ChangeValue(-dmg);
             else if(collision.gameObject.tag == "Player" && currTarget == 1)
                    healthBar.ChangeValue(-dmg);
            currDelay = attackDelay;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Damaged(collision.gameObject.GetComponent<Bullet>().dmg);
            if(collision.gameObject.GetComponent<Bullet>().isFlying)
                Destroy(collision.gameObject);
        }
    }
}
