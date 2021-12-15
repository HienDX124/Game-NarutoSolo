using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{

    public float moveSpeed;
    public int shurikenDirection;
    public float ShurikenDamage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shurikenDirection == Player.RIGHT_DIR) {
            transform.position = transform.position + Vector3.right*moveSpeed*Time.deltaTime;
        }
        else {
            transform.position = transform.position + Vector3.left*moveSpeed*Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player")) {
            coll.gameObject.GetComponent<Player>().TakeDamage(ShurikenDamage);
            Destroy(gameObject);
        }

    }
}
