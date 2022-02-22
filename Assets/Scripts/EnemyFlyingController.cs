using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    public float rangeToStartChase;
    private bool isChasing;

    public float moveSpeed;
    public float turnSpeed;

    public Animator anim;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.transform;
        isChasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasing)
        {   
            //If enemys position is within range of players position
            if (Vector3.Distance(transform.position, player.position) < rangeToStartChase)
            {
                isChasing = true;
                anim.SetBool("isChasing", isChasing);
            }
        }
        else
        {
            //If enemys position is within range of players position
            if (Vector3.Distance(transform.position, player.position) > rangeToStartChase)
            {
                isChasing = false;
                anim.SetBool("isChasing", isChasing);
            }

            //If player isn't dead
            if (player.gameObject.activeSelf)
            {
                Vector3 direction = transform.position - player.position;
                //Returns rad of y/x then converts to degrees
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);


                transform.position += -transform.right * moveSpeed * Time.deltaTime;
            }
        }
    }
}
