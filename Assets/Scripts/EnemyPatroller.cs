using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;
    public float moveSpeed, waitAtPoints;
    private float waitCounter;
    public float jumpForce;
    public Animator anim;
    public Rigidbody2D theRB;


    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;
        currentPoint = 0;
        foreach(Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If not at patrol point, locate the next point and move towards it
        if(Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > 0.2f )
        {
            if(transform.position.x < patrolPoints[currentPoint].position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
            }

            //If the patrol point is higher than the enemy, jump to climb obstacles
            if ((transform.position.y < patrolPoints[currentPoint].position.y - 0.5f) && 
                (theRB.velocity.y < 0.1f))
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);

            }
        }
        //Else if at patrol point wait, then head to next patrol point
        else
        {
            theRB.velocity = new Vector2(0f, theRB.velocity.y);

            waitCounter -= Time.deltaTime;
            if(waitCounter <= 0)
            {
                waitCounter = waitAtPoints;

                if (currentPoint >= patrolPoints.Length - 1)
                {
                    currentPoint = 0;
                }
                else
                {
                    currentPoint++;
                }
            }
        }
        //Update speed in the animator
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
