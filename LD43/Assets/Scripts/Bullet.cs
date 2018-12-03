using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public int dmg;
    Vector2 border;
    public bool isFlying;

	// Use this for initialization
	void Start () {
        if (isFlying)
        {
            float rotation = transform.rotation.eulerAngles.z;
            Vector2 direction = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad)).normalized;
            //Debug.Log(direction);
            GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);

            border = new Vector2(Mess.instance.worldDim.x + 1, Mess.instance.worldDim.y + 1);
        }
	}

    private void Update()
    {
        if (isFlying)
        {
            if (Mathf.Abs(transform.position.x) > border.x || Mathf.Abs(transform.position.y) > Mathf.Abs(border.y))
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
