using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D theRB;
    public Vector2 moveDir;

    public GameObject impactEffect;

    public int damageAmount;

    // Update is called once per frame
    void Update()
    {
        //direction times speed
        theRB.velocity = moveDir * bulletSpeed;
    }

    //when collision is triggered
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if(impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity); //Quaternion is for rotation, no rotation
        }
        //destroy this game object
        Destroy(gameObject);
        
    }

    private void OnBecameInvisible()
    {
        //Out of camera destroy
        Destroy(gameObject);
    }
}
